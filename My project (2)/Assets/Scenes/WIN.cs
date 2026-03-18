using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class BossTrophyController : MonoBehaviour
{
    [Header("灯光设置")]
    public Light2D trophyLight;
    public float targetIntensity = 2f;
    public float targetOuterRadius = 5f;
    public float lightAnimDuration = 1.5f;

    [Header("摄像机设置")]
    public CameraController cameraController;

    [Header("退出设置")]
    public float exitDelay = 5f;
    public string targetSceneName = "StartScene";

    public void OnLinked()
    {
        StartCoroutine(PlayCelebrationAndExit());
    }

    private System.Collections.IEnumerator PlayCelebrationAndExit()
    {
        Coroutine lightAnim = null;
        if (trophyLight != null)
            lightAnim = StartCoroutine(AnimateLight());

        Coroutine camAnim = null;
        if (cameraController != null)
            camAnim = StartCoroutine(cameraController.RaiseCameraCoroutine());

        if (lightAnim != null) yield return lightAnim;
        if (camAnim != null) yield return camAnim;

        Debug.Log($"动画完成，等待 {exitDelay} 秒后退出...");
        yield return new WaitForSeconds(exitDelay);

        Debug.Log($"加载场景: {targetSceneName}");
        SceneManager.LoadScene(targetSceneName);
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

            trophyLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            trophyLight.pointLightOuterRadius = Mathf.Lerp(startRadius, targetOuterRadius, t);

            yield return null;
        }

        trophyLight.intensity = targetIntensity;
        trophyLight.pointLightOuterRadius = targetOuterRadius;
    }
}