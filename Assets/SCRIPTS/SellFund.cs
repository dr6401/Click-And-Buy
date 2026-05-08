using UnityEngine;
using UnityEngine.EventSystems;

public class SellFund : MonoBehaviour
{
    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject areYouSurePanel;
    
    public void SellCurrentFund()
    {
        FundManager.Instance.SellFund();
        areYouSurePanel?.SetActive(false);
    }
    public void AreYouSureToSellFund()
    {
        areYouSurePanel?.SetActive(true);
        tooltip?.SetActive(false);
    }

    public void CancelSellingFund()
    {
        areYouSurePanel?.SetActive(false);        
    }
}
