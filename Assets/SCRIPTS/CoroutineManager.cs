using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
