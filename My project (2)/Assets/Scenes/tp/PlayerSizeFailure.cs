using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement; // 必须添加，否则 SceneManager 报错

public class PlayerSizeFailure : MonoBehaviour
{
    [SerializeField] private bool _isFailed = false; // 统一变量名
    private Vector3 _originalScale;
    private float _failScale = 0.3f;

    public void ResetFailureState()
    {
        _isFailed = false; // 重置失败标记（和 Update 里判断的变量一致）
        transform.localScale = Vector3.one; // 恢复大小
        Debug.Log("玩家失败状态已重置！");
    }

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        if (_isFailed) return;

        float currentScale = transform.localScale.x / _originalScale.x;
        if (currentScale <= _failScale)
        {
            OnGameFail();
        }
    }

    void OnGameFail()
    {
        _isFailed = true;
        Debug.Log("游戏失败！尺寸过小");
        SceneManager.LoadScene("Fail"); // 现在可以正常调用
    }
}
