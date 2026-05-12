using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "Augments/ComboFever")]
public class ComboFever : Augment
{
    public int comboIncrease;
    public float duration;
    public override void Apply()
    {
        if (LevelManager.Instance == null || CoroutineManager.Instance == null) return;
        CoroutineManager.Instance.StartCoroutine(IncreaseComboForDuration());
    }

    private IEnumerator IncreaseComboForDuration()
    {
        LevelManager.Instance.comboFeverIncrease += comboIncrease;
        yield return new WaitForSeconds(duration);
        LevelManager.Instance.comboFeverIncrease -= comboIncrease;
    } 
}