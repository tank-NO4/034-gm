using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Process : MonoBehaviour
{
    [Header("进度条")]
    public Image progressBar;

    [Header("时间设置")]
    public float ChangeInterval = 20f;
    public float ResponseTime = 6f;

    private float _timer;
    private bool _isResponseStage;

    void Start()
    {
        StartNormalStage();
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        // 更新进度条
        if (progressBar != null)
        {
            if (_isResponseStage)
                progressBar.fillAmount = _timer / ResponseTime;
            else
                progressBar.fillAmount = _timer / ChangeInterval;
        }

        // 时间到，切换阶段
        if (_timer <= 0)
        {
            if (_isResponseStage)
                StartNormalStage();
            else
                StartResponseStage();
        }
    }

    void StartNormalStage()
    {
        _isResponseStage = false;
        _timer = ChangeInterval;
        SetBubbleInactive();
    }

    void StartResponseStage()
    {
        _isResponseStage = true;
        _timer = ResponseTime;
        SetBubbleActive();
    }

    void SetBubbleActive()
    {
        // 这里写你气泡激活逻辑（变色/显示等）
        Debug.Log("进入响应阶段");
    }

    void SetBubbleInactive()
    {
        // 这里写气泡恢复逻辑
        Debug.Log("恢复正常阶段");
    }
}
