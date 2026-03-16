using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class TeleportWithFade : MonoBehaviour
{
    [Header("白屏过渡图片")]
    public Image fadeImage;

    [Header("过渡时长")]
    public float fadeDuration = 1f;

    [Header("目标传送点")]
    public Transform targetTeleportPoint;

    private const string PlayerTag = "Player";

    public void StartTeleport()
    {
        GameObject player = GameObject.FindWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogError("找不到 Tag 为 Player 的玩家对象！");
            return;
        }

        StartCoroutine(FadeAndTeleportCoroutine(player.transform));
    }

    private IEnumerator FadeAndTeleportCoroutine(Transform playerTransform)
    {
        // 1. 淡入白屏
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // 2. 传送前：重置链接状态（关键修复）
        LinkShapeShrink linkShrink = playerTransform.GetComponent<LinkShapeShrink>();
        if (linkShrink != null)
        {
            linkShrink.ResetLinkStateForTeleport(); // 调用公开方法
        }

        // 3. 执行传送
        playerTransform.position = targetTeleportPoint.position;
        Debug.Log($"玩家已传送到：{targetTeleportPoint.name}");

        // 4. 淡出白屏
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            StartTeleport();
        }
    }
}
