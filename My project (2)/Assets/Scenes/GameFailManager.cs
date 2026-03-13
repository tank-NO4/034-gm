using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFailManager : MonoBehaviour
{
    // 单例
    public static GameFailManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 【核心方法】阶段失败 / 主动重开
    public void RestartCurrentStage()
    {
        // 1. 获取当前正在玩的场景名字
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 2. 重新加载它，实现完全重置
        SceneManager.LoadScene(currentSceneName);

        // 🔥 如果你有玩家数据重置需求（如血量、分数），可以在这里写
        // GameDataManager.Instance.ResetStageData();
    }
}

