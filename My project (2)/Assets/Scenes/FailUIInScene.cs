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
        BackToMenuButton.onClick.AddListener(OnBackToMenu);
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
            PlayerSizeFailure failure = player.GetComponent<PlayerSizeFailure>();
            if (failure != null) failure.ResetFailureState();
        }
    }

    public void OnBackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
    
