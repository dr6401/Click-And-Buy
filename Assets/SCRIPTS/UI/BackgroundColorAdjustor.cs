using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorAdjustor : MonoBehaviour
{
    [SerializeField] private float riskLevelPercent;
    [SerializeField] private float profitPercent;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color greenColor;
    [SerializeField] private Color redColor;
    
    private Image image;
    private Color targetColor;

    [SerializeField] private float colorTransitionSpeed = 5f; 

    private void Awake()
    {
        image =  GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseColor = image.color;
    }
    
    void Update()
    {
        riskLevelPercent = LevelManager.Instance.unrealizedLoss / (LevelManager.Instance.effectiveCash + LevelManager.Instance.unrealizedLoss + 0.0001f); // Add this small value so the variable doesnt become NaN
        riskLevelPercent = Mathf.SmoothStep(0f, 1f, riskLevelPercent);
        if (riskLevelPercent > 0)
        {
            targetColor = Color.Lerp(baseColor, redColor, riskLevelPercent);
        }
        else if (LevelManager.Instance.unrealizedProfit >= 0)
        {
            profitPercent = LevelManager.Instance.openProfitAndLoss / 100f;
            profitPercent = Mathf.SmoothStep(0f, 1f, profitPercent);
            targetColor = Color.Lerp(baseColor, greenColor, profitPercent);
        }
        
        image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * colorTransitionSpeed);
    }
}
