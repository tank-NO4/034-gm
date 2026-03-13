using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BreathingIconPhased : MonoBehaviour
{
    [Header("呼吸设置")]
    public float speed = 1.5f;              // 呼吸速度
    [Range(0f, 1f)]
    public float intensity = 0.3f;           // 呼吸幅度
    [Range(0f, 360f)]
    public float phaseOffset = 0f;           // 相位偏移（度），0~360
    public Color baseColor = Color.white;     // 基础颜色（含透明度）
    public bool useAlphaBreath = false;       // true: 透明度呼吸；false: 亮度呼吸

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = baseColor;
        sr.color = originalColor;
    }

    void Update()
    {
        // 将相位偏移转为弧度，并加入正弦计算
        float phaseRad = phaseOffset * Mathf.Deg2Rad;
        float factor = 1f + Mathf.Sin(Time.time * speed + phaseRad) * intensity;

        if (useAlphaBreath)
        {
            // 仅改变透明度
            Color newColor = originalColor;
            newColor.a = originalColor.a * factor;
            sr.color = newColor;
        }
        else
        {
            // 改变亮度（RGB 乘性变化）
            Color newColor = originalColor * factor;
            newColor.a = originalColor.a;
            sr.color = newColor;
        }
    }
}