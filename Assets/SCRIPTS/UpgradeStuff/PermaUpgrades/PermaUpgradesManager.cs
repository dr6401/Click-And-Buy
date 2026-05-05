using UnityEngine;

public class PermaUpgradesManager : MonoBehaviour
{
    [Header("Permanent Upgrades Current Levels")]
    public int profitMultLvl;
    public int lossShieldLvl;
    public int riskyMovesLvl;

    public static PermaUpgradesManager Instance;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
}
