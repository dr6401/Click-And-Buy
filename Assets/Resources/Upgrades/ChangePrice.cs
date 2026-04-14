using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/ChangePrice")]
public class ChangePrice : Augment
{
    public float priceIncrease;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.price += priceIncrease;
        string sign = priceIncrease > 0 ? "+" : "-";
        string priceIncreaseString = sign + priceIncrease;
        LevelManager.Instance.SpawnTextDamageNumbers(priceIncreaseString);
        //LevelManager.Instance.price = Mathf.Min(LevelManager.Instance.price, LevelManager.Instance.maxPrice);
    }
}