using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 必须继承自 MonoBehaviour 才能挂载到 GameObject 上
public class UIRestart : MonoBehaviour
{
    // 重开按钮点击事件
    public void OnRestartButtonClick()
    {
        // 回到游戏场景（请将 "SampleScene" 替换为你的关卡场景名）
        SceneManager.LoadScene("SampleScene");

        // 可选：重置时间缩放（解决游戏暂停后重启的问题）
        Time.timeScale = 5f;
    }
}

