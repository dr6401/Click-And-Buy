using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/EventSchedule")]
public class EventSchedule : Augment
{
    public PriceMoveEvent priceMoveEvent;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        LevelManager.Instance.PlayPriceMoveEvent(priceMoveEvent);
        //LevelManager.Instance.price = Mathf.Min(LevelManager.Instance.price, LevelManager.Instance.maxPrice);
    }
}