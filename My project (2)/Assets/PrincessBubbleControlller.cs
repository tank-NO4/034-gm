using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;



using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    private Transform _playerTransform;

    private int _currentActiveShape = -1;
    private bool _isPlayerInContact = false;

    void Start()
    {
        _playerTransform = playerScript.GetComponent<Transform>();
        SetAllShapesInactive();
        StartCoroutine(RandomShapeBlink());

        if (princessCamera != null)
            princessCamera.enabled = true;
        if (minimapDisplay != null)
            minimapDisplay.texture = princessCamera.targetTexture;
    }

    void Update()
    {
        if (princessCamera != null && !_isPlayerInContact)
        {
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 2f, princessCamera.transform.position.z);
            princessCamera.transform.position = Vector3.Lerp(princessCamera.transform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
        }

        float distToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
        if (distToPlayer < 1.5f)
        {
            _isPlayerInContact = true;
            HideBubbleAndCamera();
        }
        else
        {
            _isPlayerInContact = false;
            ShowBubbleAndCamera();
        }
    }

    IEnumerator RandomShapeBlink()
    {
        while (true)
        {
            if (_isPlayerInContact)
            {
                yield return null;
                continue;
            }

            // 1. 随机出新形状
            int newShape = Random.Range(0, 3);

            // 2. 先全部变灰，再把新形状变绿【给玩家看提示】
            SetAllShapesInactive();
            ActivateShape(newShape);

            _currentActiveShape = newShape;

            // 3. 等待玩家反应

            yield return new WaitForSeconds(responseTime + 0.5f);


            // 4. 检查是否匹配
            bool isMatch = GetPlayerCurrentShape() == _currentActiveShape;

            if (!isMatch && !_isPlayerInContact)
            {
                // 不匹配 → 传送
                Transform respawnPoint = GameObject.Find("RespawnPoint")?.transform;
                if (respawnPoint != null)
                {
                    _playerTransform.position = respawnPoint.position;
                    _playerTransform.localScale = Vector3.one;
                    Debug.Log(" 形状不匹配，传送至复活点");
                }
            }
            else
            {
                Debug.Log(" 形状匹配成功！");
            }

            // 5. 等待到下一轮
            yield return new WaitForSeconds(changeInterval - responseTime);
        }
    }

    void SetAllShapesInactive()
    {
        circleImg.color = inactiveColor;
        triangleImg.color = inactiveColor;
        squareImg.color = inactiveColor;
    }

    void ActivateShape(int shapeIndex)
    {
        SetAllShapesInactive();
        switch (shapeIndex)
        {
            case 0: circleImg.color = activeColor; break;
            case 1: triangleImg.color = activeColor; break;
            case 2: squareImg.color = activeColor; break;
        }
    }

    private int GetPlayerCurrentShape()
    {
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