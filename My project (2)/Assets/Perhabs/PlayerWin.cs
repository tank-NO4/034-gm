using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWin : MonoBehaviour
{
    // 胜利UI
    public GameObject winPanel;

    // 当角色碰到物体时触发
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到的物体标签是 "WinPoint"
        if (other.CompareTag("WinPoint"))
        {
            // 显示胜利界面
            winPanel.SetActive(true);

          
        }
    }
}

