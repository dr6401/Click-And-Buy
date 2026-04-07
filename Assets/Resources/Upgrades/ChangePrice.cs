using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/ChangePrice")]
public class ChangePrice : Augment
{
    public float priceIncrease;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.price += priceIncrease;
        //LevelManager.Instance.price = Mathf.Min(LevelManager.Instance.price, LevelManager.Instance.maxPrice);
    }
}