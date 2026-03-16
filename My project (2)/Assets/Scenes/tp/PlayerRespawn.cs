using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
   
    private Transform _tpPoint;

    private void Start()
    {
        // 找到重生点
        _tpPoint = GameObject.Find("RespawnPoint").transform;

        if (_tpPoint != null)
        {
            // 传送到重生点
            transform.position = _tpPoint.position;
        }

        // 大小强制恢复 1
        transform.localScale = Vector3.one;
    }
}
