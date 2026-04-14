using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/ChangePrice")]
public class ChangePrice : Augment
{
    public float priceIncrease;
    private string sign;
    private Color textColor;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.price += priceIncrease;
        if (priceIncrease > 0)
        {
            sign = "+";
            textColor = GameConstants.greenColor;
        }
        else if (priceIncrease < 0)
        {
            sign = "";
            textColor = GameConstants.redColor;
        }

        //textColor = ColorAdjuster.DarkenColor(textColor, 0.2f);
        string priceIncreaseString = sign + priceIncrease;
        LevelManager.Instance.SpawnTextDamageNumbers(priceIncreaseString, color: textColor, spawnAtLatestCandle: true, scatterTextOnSpawn: true);
        //LevelManager.Instance.price = Mathf.Min(LevelManager.Instance.price, LevelManager.Instance.maxPrice);
    }
}