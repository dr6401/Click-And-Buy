using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/DivineLuckIncrease")]
public class DivineLuckIncrease : Augment
{
    public int increasePercent;

    private void OnEnable()
    {
        description = $"Increase Divine Luck by {increasePercent}%";
    }

    public override void Apply()
    {
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.divineLuck += increasePercent / 100f;
    }
}