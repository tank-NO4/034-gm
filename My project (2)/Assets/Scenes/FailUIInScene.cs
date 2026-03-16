using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FailUIInScene : MonoBehaviour
{
    public Button RetryButton;
    public Button BackToMenuButton;

    void Awake()
    {
        RetryButton.onClick.AddListener(OnRetry);
    }

    public void OnRetry()
    {
        Time.timeScale = 1;
        // 先卸载 Fail 场景，再异步加载 SampleScene
        SceneManager.UnloadSceneAsync(gameObject.scene);
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("SampleScene");
        // 监听加载完成
        loadOp.completed += OnSampleSceneLoaded;
    }

    // SampleScene 加载完成后才去重置玩家
    void OnSampleSceneLoaded(AsyncOperation op)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform respawnPoint = GameObject.Find("RespawnPoint")?.transform;

        if (player != null && respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
            player.transform.localScale = Vector3.one;

            // 安全获取组件并调用方法
            if (player.TryGetComponent<PlayerSizeFailure>(out var failure))
            {
                failure.ResetFailureState();
            }
            else
            {
                Debug.LogWarning("Player 物体未挂载 PlayerSizeFailure 组件");
            }
        }
    }

    private bool _isFailed = false;

    // 必须是 public，才能被外部脚本调用
    public void ResetFailureState()
    {
        // 在这里写重置失败状态的逻辑
        _isFailed = false;
        // 比如重置大小、恢复血量、清除标记等
        transform.localScale = Vector3.one;
        Debug.Log("玩家失败状态已重置");
    }

}
    
