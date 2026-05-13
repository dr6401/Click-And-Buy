using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/DontDoIt")]
public class DontDoIt : Augment
{
    public float min;
    public float max;
    public override void Apply()
    {
        if (LevelManager.Instance == null) return;
        int moneyIncrease = Mathf.CeilToInt(Random.Range(min, max));
        LevelManager.Instance.cash += moneyIncrease;
        LevelManager.Instance.SpawnReceivedMoneyDamageNumbers(moneyIncrease);
    }

    private void OnEnable()
    {
        description = $"Gain between {NumberFormatter.FormatNumber(min)}$ and {NumberFormatter.FormatNumber(max)}$";
    }
}