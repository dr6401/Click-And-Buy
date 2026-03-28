using System.Collections;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private MMFeedbacks loadMainMenuFeedback;
    public void QuitApp()
    {
        StartCoroutine(QuitGameAndDisableAnalytics());
    }

    private IEnumerator QuitGameAndDisableAnalytics()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        loadMainMenuFeedback?.PlayFeedbacks();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
