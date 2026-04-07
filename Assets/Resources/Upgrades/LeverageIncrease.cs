using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/LeverageIncrease")]
public class LeverageIncrease : Augment
{
    public float leverageIncrease;
    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.maxLeverage += leverageIncrease;
    }
}