using UnityEngine;
using UnityEngine.Rendering.Universal; // 使用 Light2D 需要这个命名空间

public class BossTrophyController : MonoBehaviour
{
    [Header("灯光设置")]
    public Light2D trophyLight;            // 拖入奖杯上的 Light2D 组件
    public float targetLightIntensity = 2f; // 目标亮度
    public float targetLightRadius = 5f;    // 目标半径（Point Light 2D 的 Outer Radius）
    public float lightAnimDuration = 1.5f;  // 灯光动画时长

    [Header("摄像机设置（可选）")]
    public CameraController cameraController; // 如果摄像机脚本单独管理，可拖入

    /// <summary>
    /// 当玩家链接到奖杯时调用此方法
    /// </summary>
    public void OnLinked()
    {
        // 1. 播放灯光变大动画
        if (trophyLight != null)
            StartCoroutine(AnimateLight());

        // 2. 通知摄像机抬高（方式一：直接调用摄像机静态方法）
        if (Camera.main != null)
        {
            Camera.main.GetComponent<CameraController>()?.RaiseCamera();
        }

        // 方式二：通过拖拽的引用调用
        // if (cameraController != null) cameraController.RaiseCamera();
    }

    private System.Collections.IEnumerator AnimateLight()
    {
        float startIntensity = trophyLight.intensity;
        float startRadius = trophyLight.pointLightOuterRadius;
        float elapsed = 0f;

        while (elapsed < lightAnimDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lightAnimDuration;

            trophyLight.intensity = Mathf.Lerp(startIntensity, targetLightIntensity, t);
            trophyLight.pointLightOuterRadius = Mathf.Lerp(startRadius, targetLightRadius, t);

            yield return null;
        }

        // 确保最终值准确
        trophyLight.intensity = targetLightIntensity;
        trophyLight.pointLightOuterRadius = targetLightRadius;
    }
}