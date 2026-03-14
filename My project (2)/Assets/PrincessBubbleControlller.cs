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
        // 安全获取玩家
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
    }

    void Update()
    {
        if (_playerTransform == null) return;

        // 相机跟随
        if (princessCamera != null && !_isPlayerInContact)
        {
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + 2f, princessCamera.transform.position.z);
            princessCamera.transform.position = Vector3.Lerp(princessCamera.transform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
        }

        // 判断是否靠近
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
            // 玩家靠近时暂停逻辑
            if (_isPlayerInContact || _playerTransform == null || playerScript == null)
            {
                yield return null;
                continue;
            }

            // 1. 随机形状
            int newShape = Random.Range(0, 3);
            SetAllShapesInactive();
            ActivateShape(newShape);
            _currentActiveShape = newShape;

            Debug.Log($"本轮要求形状: {newShape}");

            // 2. 给玩家反应时间
            yield return new WaitForSeconds(responseTime);

            // 3. 【关键】判断玩家当前形状
            int playerShape = GetPlayerCurrentShape();
            bool isMatch = playerShape == _currentActiveShape;

            Debug.Log($"玩家形状: {playerShape}  |  要求: {_currentActiveShape}  |  是否匹配: {isMatch}");

            // 4. 不匹配且玩家不在身边 → 传送
            if (!isMatch && !_isPlayerInContact)
            {
                Transform respawnPoint = GameObject.Find("RespawnPoint")?.transform;
                if (respawnPoint != null)
                {
                    _playerTransform.position = respawnPoint.position;
                    _playerTransform.localScale = Vector3.one;
                    Debug.Log(" 形状不匹配，传送回复活点");
                }
                else
                {
                    Debug.LogError("场景里找不到名为 RespawnPoint 的物体");
                }
            }
            else if (isMatch)
            {
                Debug.Log(" 形状匹配成功！");
            }

            // 5. 下一轮间隔
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

    // 获取玩家形状
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