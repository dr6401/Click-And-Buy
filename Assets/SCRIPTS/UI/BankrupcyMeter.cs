using UnityEngine;
using UnityEngine.UI;

public class RiskMeter : MonoBehaviour
{
    private Slider riskSlider;
    private float lerpSpeed = 50f;
    [SerializeField] private GameObject fillArea;
    void Start()
    {
        riskSlider = GetComponent<Slider>();
        if (LevelManager.Instance == null) return;
        riskSlider.value = 1 - LevelManager.Instance.effectiveCash / LevelManager.Instance.cash;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance == null) return;
        float targetSliderValue = 1 - LevelManager.Instance.effectiveCash / LevelManager.Instance.cash;
        riskSlider.value = Mathf.Lerp(riskSlider.value, targetSliderValue, Time.unscaledDeltaTime * lerpSpeed);
        fillArea.SetActive(riskSlider.value >= 0.01f);
    }
}
