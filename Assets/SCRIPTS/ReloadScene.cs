using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private void Update()
    {
    }

    public void ReloadAScene()
    {
        Debug.Log("loading scene");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    
}
