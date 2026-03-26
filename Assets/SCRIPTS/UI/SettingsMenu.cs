using UnityEngine;
using UnityEngine.Serialization;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuPanelBackground;
    [SerializeField] private GameObject settingsPanel;

    public void OpenSettingsMenu()
    {
        pauseMenu.SetActive(false);
        pauseMenuPanelBackground.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanelBackground.SetActive(true);
        pauseMenu.SetActive(true);
    }
}
