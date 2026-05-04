using UnityEngine;

public class SwitchDisplayToPanel : MonoBehaviour
{
    public GameObject panelToDisplay;

    public void OpenPanel()
    {
        GameEvents.OnMainPanelChanged(panelToDisplay);
    }
}
