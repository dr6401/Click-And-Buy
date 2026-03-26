using System;
using UnityEngine;

public class GeneralProgressManager : MonoBehaviour
{
    public int totalAmountOfAnimalsConsumed;
    public int totalAmountOfMutatedAnimalsConsumed;
    public int numberOfTimesPlayerPausedGame;
    public int numberOfTimesPlayerSpunEndlessSlots;
    public int numberOfTimesPlayerTriggeredMomentumFeast;
    public float amountOfTimeExtendedFromEndlessAppetite;
    public bool hasPlayerCompletedCampaign;
    public bool hasPlayerSeenMidFencesFall;
    public bool hasPlayerSeenOuterFencesFall;
    private SaveData data;
    
    public static GeneralProgressManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        LoadSettings();
    }
    
    public void SaveSettings()
    {
        data = SaveSystem.Load();
        data.generalData.totalAmountOfAnimalsConsumed = totalAmountOfAnimalsConsumed;
        data.generalData.totalAmountOfMutatedAnimalsConsumed = totalAmountOfMutatedAnimalsConsumed;
        data.generalData.numberOfTimesPlayerPausedGame = numberOfTimesPlayerPausedGame;
        data.generalData.numberOfTimesPlayerSpunEndlessSlots = numberOfTimesPlayerSpunEndlessSlots;
        data.generalData.numberOfTimesPlayerTriggeredMomentumFeast = numberOfTimesPlayerTriggeredMomentumFeast;
        data.generalData.amountOfTimeExtendedFromEndlessAppetite = amountOfTimeExtendedFromEndlessAppetite;
        data.generalData.hasPlayerCompletedCampaign = hasPlayerCompletedCampaign;
        data.generalData.hasPlayerSeenMidFencesFall = hasPlayerSeenMidFencesFall;
        data.generalData.hasPlayerSeenOuterFencesFall = hasPlayerSeenOuterFencesFall;
        Debug.Log($"Saving general data");
        SaveSystem.Save(data);
    }

    private void LoadSettings()
    {
        data = SaveSystem.Load();
        totalAmountOfAnimalsConsumed = data.generalData.totalAmountOfAnimalsConsumed;
        totalAmountOfMutatedAnimalsConsumed = data.generalData.totalAmountOfMutatedAnimalsConsumed;
        numberOfTimesPlayerPausedGame = data.generalData.numberOfTimesPlayerPausedGame;
        numberOfTimesPlayerSpunEndlessSlots = data.generalData.numberOfTimesPlayerSpunEndlessSlots;
        numberOfTimesPlayerTriggeredMomentumFeast = data.generalData.numberOfTimesPlayerTriggeredMomentumFeast;
        amountOfTimeExtendedFromEndlessAppetite = data.generalData.amountOfTimeExtendedFromEndlessAppetite;
        hasPlayerCompletedCampaign = data.generalData.hasPlayerCompletedCampaign;
        hasPlayerSeenMidFencesFall = data.generalData.hasPlayerSeenMidFencesFall;
        hasPlayerSeenOuterFencesFall = data.generalData.hasPlayerSeenOuterFencesFall;
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }


    private void OnEnable()
    {

    }
    
    private void OnDisable()
    {

    }
}
