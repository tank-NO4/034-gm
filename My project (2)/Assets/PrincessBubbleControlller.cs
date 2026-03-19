using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PrincessBubbleController : MonoBehaviour
{
    [Header("气泡设置")]
    public GameObject bubbleUI;
    public Image circleImg;
    public Image triangleImg;
    public Image squareImg;
    public Color inactiveColor = Color.gray;
    public Color activeColor = Color.green;
    public float changeInterval = 5f;
    public float responseTime = 4f;

    [Header("摄像头与小地图")]
    public Camera princessCamera;
    public RawImage minimapDisplay;
    public float cameraFollowSpeed = 5f;

    [Header("玩家引用")]
    public LinkShapeShrink playerScript;

    [Header("时间显示")]
    public Text timeText;

    [Header("进度条")] // 新增进度条字段
    public Image progressBar;

    private Transform _playerTransform;
    private int _currentActiveShape = -1;
    private bool _isPlayerInContact = false;

    private float _changeTimer;
    private float _responseTimer;
    private bool _isInResponsePhase;

    void Start()
    {
        if (playerScript != null)
            _playerTransform = playerScript.transform;
        else
            Debug.LogError("请在Inspector拖入 playerScript!");

        SetAllShapesInactive();
        StartCoroutine(RandomShapeBlink());

        if (princessCamera != null)
            princessCamera.enabled = true;
        if (minimapDisplay != null && princessCamera != null)
            minimapDisplay.texture = princessCamera.targetTexture;

        // 初始化计时器
        _changeTimer = changeInterval;
        _responseTimer = responseTime;
    }

    void Update()
    {
        if (_playerTransform == null) return;

        if (!_isInResponsePhase)
        {
            // 等待阶段计时
            _changeTimer -= Time.deltaTime;
            if (timeText != null)
                timeText.text = $"公主等待: {_changeTimer:F1}s";

            // 更新进度条（等待阶段）
            if (progressBar != null)
                progressBar.fillAmount = _changeTimer / changeInterval;

            if (_changeTimer <= 0)
            {
                _isInResponsePhase = true;
                _responseTimer = responseTime; // 重置响应阶段计时器
            }
        }
        else
        {
            // 响应阶段计时
            _responseTimer -= Time.deltaTime;
            if (timeText != null)
                timeText.text = $"公主响应: {_responseTimer:F1}s";

            // 更新进度条（响应阶段）
            if (progressBar != null)
                progressBar.fillAmount = _responseTimer / responseTime;

            if (_responseTimer <= 0)
            {
                _isInResponsePhase = false;
                _changeTimer = changeInterval; // 重置等待阶段计时器
            }
        }

        // 相机跟随
        if (princessCamera != null && !_isPlayerInContact)
        {
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 2f, princessCamera.transform.position.z);
            princessCamera.transform.position = Vector3.Lerp(princessCamera.transform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
        }

        // 判断是否靠近玩家
        float distToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        _isPlayerInContact = distToPlayer < 1.5f;

        if (_isPlayerInContact)
            HideBubbleAndCamera();
        else
            ShowBubbleAndCamera();
    }

    IEnumerator RandomShapeBlink()
    {
        while (true)
        {
            if (_isPlayerInContact || _playerTransform == null || playerScript == null)
            {
                yield return null;
                continue;
            }

            int newShape;
            do
            {
                newShape = Random.Range(0, 3);
            } while (newShape == _currentActiveShape);

            SetAllShapesInactive();
            ActivateShape(newShape);
            _currentActiveShape = newShape;
            Debug.Log($"本轮要求形状: {newShape}");

            yield return new WaitForSeconds(responseTime);

            int playerShape = GetPlayerCurrentShape();
            bool isMatch = playerShape == _currentActiveShape;
            Debug.Log($"玩家形状: {playerShape}  |  要求: {_currentActiveShape}  |  是否匹配: {isMatch}");

            if (!isMatch && !_isPlayerInContact)
            {
                Transform respawnPoint = GameObject.Find("RespawnPoint")?.transform;
                if (respawnPoint != null)
                {
                    _playerTransform.position = respawnPoint.position;
                    _playerTransform.localScale = Vector3.one;
                    Debug.Log("形状不匹配，传送回复活点");
                }
                else
                {
                    Debug.LogError("场景里找不到名为 RespawnPoint 的物体");
                }
            }
            else if (isMatch)
            {
                Debug.Log("形状匹配成功！");
            }

            yield return new WaitForSeconds(changeInterval - responseTime);
        }
    }

    void SetAllShapesInactive()
    {
        if (circleImg != null) circleImg.color = inactiveColor;
        if (triangleImg != null) triangleImg.color = inactiveColor;
        if (squareImg != null) squareImg.color = inactiveColor;
    }

    void ActivateShape(int shapeIndex)
    {
        SetAllShapesInactive();
        switch (shapeIndex)
        {
            case 0: if (circleImg != null) circleImg.color = activeColor; break;
            case 1: if (triangleImg != null) triangleImg.color = activeColor; break;
            case 2: if (squareImg != null) squareImg.color = activeColor; break;
        }
    }

    private int GetPlayerCurrentShape()
    {
        if (playerScript == null) return -1;
        return playerScript.currentShape;
    }

    void HideBubbleAndCamera()
    {
        if (bubbleUI != null) bubbleUI.SetActive(false);
        if (princessCamera != null) princessCamera.enabled = false;
        if (minimapDisplay != null) minimapDisplay.gameObject.SetActive(false);
    }

    void ShowBubbleAndCamera()
    {
        if (bubbleUI != null) bubbleUI.SetActive(true);
        if (princessCamera != null) princessCamera.enabled = true;
        if (minimapDisplay != null) minimapDisplay.gameObject.SetActive(true);
    }

    public int GetCurrentActiveShape()
    {
        return _currentActiveShape;
    }
}