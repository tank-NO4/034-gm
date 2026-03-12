using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkShapeShrink : MonoBehaviour
{
    [Header("绳索设置")]
    public float ropeDefaultLength = 2f;
    public float ropeForce = 15f;

    [Header("大小参数")]
    public float playerSize = 1f;
    private float _targetPlayerSize;
    private float _shapeChangeBaseSize; // 每次链接的缩放宽基

    [Header("变形设置")]
    public Sprite defaultPlayerSprite;
    public float shapeChangeDelay = 0.5f;
    private Vector3 _targetScale;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _isShapeChanged = false;

    [Header("链接缩小规则")]
    public float linkDurationThreshold = 1f;
    public float sizeReductionPerSecond = 0.1f;
    public float shrinkAnimationSpeed = 2f;

    [Header("标签")]
    public string groundTag = "Ground";
    public string wallTag = "Wall";

    private Rigidbody2D _rb;
    private GameObject _linkedTarget;
    private bool _isLinking;
    private LineRenderer _line;
    private float _currentLinkDuration = 0f;
    public int currentShape = 0;
    

    void Awake()
    {
        Debug.Log("LinkShapeShrink脚本已加载！");
        _rb = GetComponent<Rigidbody2D>();
        _line = GetComponent<LineRenderer>();
        if (_line == null) _line = gameObject.AddComponent<LineRenderer>();
        _line.positionCount = 2;
        _line.startWidth = 0.1f;
        _line.enabled = false;

        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (_playerSpriteRenderer != null)
            defaultPlayerSprite = _playerSpriteRenderer.sprite;

        _targetPlayerSize = transform.localScale.x;
        _shapeChangeBaseSize = _targetPlayerSize;
        _targetScale = Vector3.one * _targetPlayerSize;
       
            Debug.Log("_rb: " + (_rb != null));
            Debug.Log("_line: " + (_line != null));
            Debug.Log("_playerSpriteRenderer: " + (_playerSpriteRenderer != null));
      
    }
    void TryLinkTarget()
    {
            // 正确获取鼠标世界坐标的写法（2D游戏必加）
            Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);


        // 可视化点选位置（画一个小十字）
        Debug.DrawLine(mouseWorldPos - Vector2.up * 0.1f, mouseWorldPos + Vector2.up * 0.1f, Color.green, 2f);
        Debug.DrawLine(mouseWorldPos - Vector2.right * 0.1f, mouseWorldPos + Vector2.right * 0.1f, Color.green, 2f);

        // 纯点选：只检测鼠标点位置
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        // ✅ 把 RaycastHit2D 改成 Collider2D，接收 OverlapPoint 的返回值
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos, layerMask);

        if (hitCollider != null)
        {
            // ✅ 直接用 hitCollider.gameObject / hitCollider.tag，不需要 .collider
            Debug.Log("🎯 点选命中：" + hitCollider.gameObject.name + "，标签：" + hitCollider.tag);
            if (hitCollider.CompareTag("Linkable"))
            {
                _linkedTarget = hitCollider.gameObject;
                _line.enabled = true;
                Debug.Log("✅ 成功链接！");
            }
            else
            {
                Debug.Log("⚠️ 命中物体，但标签不是 Linkable");
            }
        }
        else
        {
            Debug.Log("❌ 点选模式：鼠标位置没有任何碰撞体");
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryLinkTarget();
        
        if (Input.GetMouseButton(0) && _linkedTarget != null)
        {
            _isLinking = true;
            UpdateRopeVisual();
            Debug.Log("检测到鼠标左键正在点击");
        }
        else
        {
            BreakLink();
        }
    }

    void FixedUpdate()
    {
        if (_isLinking && _linkedTarget != null)
        {
            ApplyLinkForce();
            UpdateLinkDurationAndShrink();
        }
        else
        {
            _currentLinkDuration = 0f;
        }

        // 平滑缩放
        playerSize = Mathf.Lerp(playerSize, _targetPlayerSize, shrinkAnimationSpeed * Time.fixedDeltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, shrinkAnimationSpeed * Time.fixedDeltaTime);
    }

   
    




    void ApplyLinkForce()
    {
        Rigidbody2D targetRb = _linkedTarget.GetComponent<Rigidbody2D>();
        LinkableTarget linkable = _linkedTarget.GetComponent<LinkableTarget>();
        if (targetRb == null || linkable == null) return;

        Vector2 dir = _linkedTarget.transform.position - transform.position;
        float dist = dir.magnitude;
        Vector2 dirNorm = dir.normalized;
        float offset = Mathf.Max(dist - ropeDefaultLength, 0);
        Vector2 force = dirNorm * offset * ropeForce;

        float totalSize = playerSize + linkable.targetSize;
        _rb.AddForce(force * (linkable.targetSize / totalSize));
        targetRb.AddForce(-force * (playerSize / totalSize));
    }

    void UpdateRopeVisual()
    {
        if (_linkedTarget != null)
        {
            _line.SetPosition(0, transform.position);
            _line.SetPosition(1, _linkedTarget.transform.position);
        }
    }

    void UpdateLinkDurationAndShrink()
    {
        if (_linkedTarget == null) return;
        if (_linkedTarget.CompareTag(groundTag) || _linkedTarget.CompareTag(wallTag))
        {
            _currentLinkDuration = 0f;
            return;
        }

        _currentLinkDuration += Time.fixedDeltaTime;

        // 超过阈值开始缩小
        if (_currentLinkDuration > linkDurationThreshold)
        {
            float minSize = _shapeChangeBaseSize * 0.9f;
            _targetPlayerSize -= sizeReductionPerSecond * Time.fixedDeltaTime;
            _targetPlayerSize = Mathf.Max(_targetPlayerSize, minSize);
            _targetScale = Vector3.one * _targetPlayerSize;
        }

        // 满足延迟 → 变形（每次链接都能变）
        if (_currentLinkDuration > linkDurationThreshold + shapeChangeDelay && !_isShapeChanged)
        {
            ChangeToLinkedShape();
        }
    }

    void ChangeToLinkedShape()
    {
        if (_linkedTarget == null) return;
        SpriteRenderer sr = _linkedTarget.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            _playerSpriteRenderer.sprite = sr.sprite;
            _isShapeChanged = true;
            // 根据目标物体标签设置形状（需给 Linkable 物体打标签：Circle/Triangle/Square）
            if (_linkedTarget.CompareTag("Circle")) currentShape = 0;
            else if (_linkedTarget.CompareTag("Triangle")) currentShape = 1;
            else if (_linkedTarget.CompareTag("Square")) currentShape = 2;
        }
    }

    void BreakLink()
    {
        _isLinking = false;
        _linkedTarget = null;
        _line.enabled = false;
        _currentLinkDuration = 0f;
        _isShapeChanged = false;

       
    }
    public void ForceShrink(float scaleMultiplier)
    {
        _targetPlayerSize *= scaleMultiplier;
        _targetScale = Vector3.one * _targetPlayerSize;
        _shapeChangeBaseSize = _targetPlayerSize; // 更新基准，避免后续缩放异常
    }
}