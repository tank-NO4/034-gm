using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("位置设置")]
    public Vector3 raisedPosition = new Vector3(0, 5, -10); // 目标位置（抬高Y轴）
    public float moveDuration = 2f;

    [Header("大小设置（可选）")]
    public bool changeSize = false;                // 是否同时改变摄像机大小
    public float targetSize = 8f;                  // 目标正交大小（越大视野越广）
    public float sizeDuration = 2f;

    private Vector3 originalPosition;
    private float originalSize;
    private bool isAnimating = false;

    void Start()
    {
        originalPosition = transform.position;
        if (Camera.main != null)
            originalSize = Camera.main.orthographicSize;
    }

    public void RaiseCamera()
    {
        if (!isAnimating)
            StartCoroutine(AnimateCamera());
    }

    public void ResetCamera()
    {
        if (!isAnimating)
            StartCoroutine(AnimateCamera(true));
    }

    private System.Collections.IEnumerator AnimateCamera(bool reset = false)
    {
        isAnimating = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = reset ? originalPosition : raisedPosition;

        float startSize = Camera.main.orthographicSize;
        float targetSizeValue = reset ? originalSize : targetSize;

        float elapsed = 0f;
        float maxDuration = Mathf.Max(moveDuration, changeSize ? sizeDuration : 0f);

        while (elapsed < maxDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / maxDuration;

            // 移动位置
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 改变大小（如果启用）
            if (changeSize)
                Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSizeValue, t);

            yield return null;
        }

        // 确保最终值准确
        transform.position = targetPos;
        if (changeSize)
            Camera.main.orthographicSize = targetSizeValue;

        isAnimating = false;
    }
}