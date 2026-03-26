using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayGame : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("CutscenesScene");
    }
}
