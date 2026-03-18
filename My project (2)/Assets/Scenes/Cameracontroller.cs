using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("位置设置")]
    public Vector3 raisedPosition = new Vector3(0, 5, -10); // 目标位置（抬高Y轴）
    public float moveDuration = 2f;                         // 位置动画时长

    [Header("大小设置")]
    public bool changeSize = true;                           // 是否改变正交大小
    public float targetSize = 8f;                            // 目标大小（越大视野越广）
    public float sizeDuration = 2f;                          // 大小变化时长

    private Vector3 originalPosition;
    private float originalSize;
    private bool isAnimating = false;

    void Start()
    {
        originalPosition = transform.position;
        if (Camera.main != null)
            originalSize = Camera.main.orthographicSize;
    }

    /// <summary>
    /// 启动摄像机动画（同时移动位置和改变大小），返回协程供外部等待
    /// </summary>
    public IEnumerator RaiseCameraCoroutine()
    {
        if (isAnimating) yield break;
        yield return StartCoroutine(AnimateCamera());
    }

    private IEnumerator AnimateCamera()
    {
        isAnimating = true;
        Vector3 startPos = transform.position;
        float startSize = Camera.main.orthographicSize;
        float elapsed = 0f;
        float maxDuration = Mathf.Max(moveDuration, changeSize ? sizeDuration : 0f);

        while (elapsed < maxDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / maxDuration;

            // 移动位置
            transform.position = Vector3.Lerp(startPos, raisedPosition, t);

            // 改变大小（如果启用）
            if (changeSize)
                Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // 确保最终值准确
        transform.position = raisedPosition;
        if (changeSize)
            Camera.main.orthographicSize = targetSize;

        isAnimating = false;
    }
}