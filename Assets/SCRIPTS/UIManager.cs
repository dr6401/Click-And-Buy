using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> mainPanels;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchDisplayToPanel(GameObject panel)
    {
        foreach (GameObject pnl in mainPanels)
        {
            pnl.SetActive(pnl == panel);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnMainPanelChanged += SwitchDisplayToPanel;
    }

    private void OnDisable()
    {
        GameEvents.OnMainPanelChanged -= SwitchDisplayToPanel;
    }
}
