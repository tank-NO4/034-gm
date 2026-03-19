using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class FailOnScaleDown : MonoBehaviour
{
    // 失败阈值：玩家缩放小于等于此值时触发重启
    [Header("失败缩放阈值")]
    public float failScaleThreshold = 0.3f;

    void Update()
    {
        // 检测玩家当前缩放（取 x 轴，2D/3D 通用）
        float currentScale = transform.localScale.x;

        // 当缩放小于等于阈值时，触发失败逻辑
        if (currentScale <= failScaleThreshold)
        {
            OnPlayerFailed();
        }
    }

    // 失败处理逻辑
    void OnPlayerFailed()
    {
        Debug.Log("玩家缩小到阈值以下，触发重启！");
        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}