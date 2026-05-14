using UnityEngine;

public class ActivePriceEvent
{
    public PriceMoveEvent data;

    public float startPrice;
    public float startTime;
    public float elapsed;

    public bool isTargetHigherThanCurrentPrice;

    public bool active;
}
