using UnityEngine;

[CreateAssetMenu(menuName = "Price Manipulation/Price Move Event")]
public class PriceMoveEvent : ScriptableObject
{
    public string eventName;
    [Header("Target Price")]
    public float targetPrice;
    public float targetPricePercentIncrease;
    [Header("Duration")]
    public float duration;
    
    [Header("Movement")]
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Noise")]
    public float maxNoise = 1f;
    
    [Header("Max Step")]
    public float maxStep = 5f;
    

}
