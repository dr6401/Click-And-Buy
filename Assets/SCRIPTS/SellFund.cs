using UnityEngine;

public class SellFund : MonoBehaviour
{
    public void SellCurrentFund()
    {
        FundManager.Instance.SellFund();
    }
}
