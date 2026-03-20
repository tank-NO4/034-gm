using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailOnScaleDown : MonoBehaviour
{
    [Header("失败缩放阈值")]
    public float failScaleThreshold = 0.3f;

    [Header("失败后等待时间（秒）")]
    public float waitBeforeReturn = 5f;

    [Header("场景名称")]
    public string failSceneName = "Fail";          // 失败场景
    public string mainSceneName = "SampleScene";   // 主场景

    void Update()
    {
        if (transform.localScale.x <= failScaleThreshold)
        {
            OnPlayerFailed();
        }
    }

    void OnPlayerFailed()
    {
        Debug.Log("缩放小于阈值，进入失败流程");
        // 停止可能重复触发的协程（防止多次调用）
        StopAllCoroutines();
        StartCoroutine(FailSequence());
    }

    IEnumerator FailSequence()
    {
        // 1. 加载失败场景
        SceneManager.LoadScene(failSceneName);

        // 2. 等待指定秒数（这里用场景时间，不受 Time.timeScale 影响）
        yield return new WaitForSeconds(waitBeforeReturn);

        // 3. 加载主场景，重置游戏
        SceneManager.LoadScene(mainSceneName);
    }
}