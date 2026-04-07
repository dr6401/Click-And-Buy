using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/NextTradeFree")]
public class NextTradeFree : Augment
{
    public int freebieTrades;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.numberOfFutureFreebieTrades += freebieTrades;
    }
}