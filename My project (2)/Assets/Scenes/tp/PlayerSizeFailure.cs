using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSizeFailure : MonoBehaviour
{
    [Header("失败缩放阈值")]
    public float failureScaleThreshold = 0.05f;

    // 失败UI场景名称（需和你创建的场景名一致）
    [Header("Fail")]
    public string failUISceneName = "FailUI";

    private bool isFailed = false;

    void Update()
    {
        if (!isFailed)
        {
            CheckPlayerSize();
        }
    }

    void CheckPlayerSize()
    {
        // 检测玩家当前缩放（等比缩放取x轴即可）
        float currentScale = transform.localScale.x;

        if (currentScale <= failureScaleThreshold)
        {
            OnPlayerFailed();
        }
    }

    void OnPlayerFailed()
    {
        isFailed = true;
        Debug.Log(" 玩家缩放过小，触发失败！");

        // 加载失败UI场景（叠加加载，不销毁当前游戏场景）
        SceneManager.LoadScene(failUISceneName, LoadSceneMode.Additive);

        // 暂停游戏逻辑（可选，根据需求决定是否保留）
        Time.timeScale = 0;
    }
    // 重置失败状态，供重试时调用
    public void ResetFailureState()
    {
        isFailed = false;
    }
}
