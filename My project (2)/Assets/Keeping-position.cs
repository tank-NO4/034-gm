using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;          // 拖入玩家对象
    public Vector3 offset;             // 相对于玩家的偏移（世界坐标）

    void LateUpdate()
    {
        if (player != null)
            transform.position = player.position + offset;
    }
}