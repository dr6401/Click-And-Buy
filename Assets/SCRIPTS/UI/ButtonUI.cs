using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public bool isUpgradeButton = false;
    public bool isEndlessUpgradeButton = false;
    private SoundManager soundManager;
    public MMFeedbacks hoverFeedback;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        if (isEndlessUpgradeButton)
        {
            button = GetComponent<Button>();
            button.interactable = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlayHoverButtonSFX();
        hoverFeedback?.PlayFeedbacks();   
    }
        
    public void OnPointerClick(PointerEventData eventData)  
    {
        if (isEndlessUpgradeButton)
        {
            if (button.IsInteractable() == false) return;
        }
        if (!isUpgradeButton)
        {
            soundManager.PlayClickedButtonSFX();   
        }
        else
        {
            hoverFeedback?.PlayFeedbacks();   
        }
    }
}
