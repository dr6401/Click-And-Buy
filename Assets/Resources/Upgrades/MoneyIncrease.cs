using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/MoneyIncrease")]
public class MoneyIncrease : Augment
{
    public float moneyIncrease;
    public override void Apply(GameObject player)
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.cash += moneyIncrease;
        LevelManager.Instance.SpawnReceivedMoneyDamageNumbers(moneyIncrease);
    }
}