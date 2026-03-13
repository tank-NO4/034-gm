using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSizeCheck : MonoBehaviour
{
    public GameUI gameUI;      // 拖你的Canvas
    private Vector3 startScale;
    private bool isFailed = false;

    void Start()
    {
        // 记录一开始的大小
        startScale = transform.localScale;
    }

    void Update()
    {
        if (isFailed) return;

        // 当前大小 ÷ 初始大小 = 现在的比例
        float currentScale = transform.localScale.x / startScale.x;

        // 当缩小到 初始的 0.05 倍时失败
        if (currentScale <= 0.05f)
        {
            Fail();
        }
    }

    void Fail()
    {
        isFailed = true;
        gameUI.ShowFail();  // 弹出失败界面
    }
}


