using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/PercentMoneyIncrease")]
public class PercentMoneyIncrease : Augment
{
    public float percentMoneyIncrease;
    public override void Apply(GameObject player)
    {
        if (LevelManager.Instance == null) return;
        float increase = LevelManager.Instance.cash * (percentMoneyIncrease * 0.01f);
        LevelManager.Instance.cash += increase;
        LevelManager.Instance.SpawnReceivedMoneyDamageNumbers(increase);
    }
}