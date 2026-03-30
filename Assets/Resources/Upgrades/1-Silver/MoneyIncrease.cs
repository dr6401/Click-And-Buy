using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/MoneyIncrease")]
public class MoneyIncrease : Augment
{
    public override void Apply(GameObject player)
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.cash += 500;
    }
}