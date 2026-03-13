using UnityEngine;
using UnityEngine.Rendering.Universal;   // 引入 Light2D 命名空间

[RequireComponent(typeof(Light2D))]       // 自动添加 Light2D 组件
public class BreathingRotate2D : MonoBehaviour
{
    [Header("旋转设置")]
    [Tooltip("每秒旋转角度（度/秒），绕 Z 轴")]
    public float rotateSpeed = 30f;

    [Header("呼吸设置")]
    [Tooltip("呼吸速度")]
    public float breathSpeed = 1.5f;
    [Tooltip("呼吸强度变化范围 (0~1)，例如 0.2 表示强度在 0.8~1.2 倍之间波动")]
    [Range(0f, 1f)]
    public float breathIntensity = 0.2f;
    [Tooltip("是否同时改变光源颜色（可选）")]
    public bool changeColor = false;
    [Tooltip("基础颜色（仅在 changeColor 为 true 时使用）")]
    public Color lightColor = Color.white;

    private Light2D light2D;
    private float baseIntensity;
    private Color baseColor;

    void Start()
    {
        // 获取 Light2D 组件
        light2D = GetComponent<Light2D>();
        baseIntensity = light2D.intensity;
        baseColor = light2D.color;

        // 如果指定了 changeColor，则设置初始颜色
        if (changeColor)
        {
            light2D.color = lightColor;
        }
    }

    void Update()
    {
        // 绕 Z 轴旋转（2D 平面常用）
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // 计算呼吸因子：在 1 ± breathIntensity 之间正弦波动
        float factor = 1f + Mathf.Sin(Time.time * breathSpeed) * breathIntensity;

        // 应用强度变化
        light2D.intensity = baseIntensity * factor;

        // 如果需要颜色变化，可以同时微调颜色（例如色调偏移）
        if (changeColor)
        {
            // 简单的亮度倍增，保持色调
            light2D.color = baseColor * factor;
        }
    }
}