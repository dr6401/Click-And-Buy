using System;
using UnityEngine;
using UnityEngine.UI;

public class RiskMeter : MonoBehaviour
{
    private Slider riskSlider;
    private float lerpSpeed = 50f;
    [SerializeField] private GameObject fillArea;
    private Image fillAreaImage;
    private float rawTargetSliderValue;

    [SerializeField] private Color minRiskColor;
    [SerializeField] private Color maxRiskColor;
    void Start()
    {
        riskSlider = GetComponent<Slider>();
        if (LevelManager.Instance == null) return;
        riskSlider.value = 1 - LevelManager.Instance.effectiveCash / LevelManager.Instance.cash;
        fillAreaImage = fillArea.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance == null) return;
        rawTargetSliderValue = LevelManager.Instance.unrealizedLoss / (LevelManager.Instance.effectiveCash + LevelManager.Instance.unrealizedLoss + 0.0001f); // Add this small value so the variable doesnt become NaN
        riskSlider.value = Mathf.Lerp(riskSlider.value, rawTargetSliderValue, Time.unscaledDeltaTime * lerpSpeed);
        Debug.Log($"rawTargetSliderValue: {rawTargetSliderValue}, riskSlider.value: {riskSlider.value}");
    }

    private void LateUpdate()
    {
        fillAreaImage.color = Color.Lerp(minRiskColor, maxRiskColor, riskSlider.value);
        fillArea.SetActive(riskSlider.value >= 0.01f);
    }
}
