using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/InsiderTip")]
public class InsiderTip : Augment
{
    public float secondIntoTheFuture;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.currentChart.PredictPriceAfterSeconds(secondIntoTheFuture);
    }
}