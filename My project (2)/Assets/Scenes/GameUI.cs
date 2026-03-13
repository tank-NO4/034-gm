using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject winPanel;   // 拖胜利面板
    public GameObject failPanel;  // 拖失败面板

    // 显示胜利
    public void ShowWin()
    {
        winPanel.SetActive(true);
    }

    // 显示失败
    public void ShowFail()
    {
        failPanel.SetActive(true);
    }

    // 重新开始当前关卡
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 返回开始界面
    public void BackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}

