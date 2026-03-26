using System;
using UnityEngine;


public static class GameEvents
{
    public static Action<int> OnSpawnAnimals;
    public static Action OnLevelTimeRanOut;
    public static Action OnDisableInput;
    public static Action<int> OnReportAliveAnimalsCount;
    public static Action<int> OnReportPoppedAnimalsCount;
    public static Action OnReportEndlessLevelCompleted;
    public static Action OnReportEndlessLevelFailed;
    public static Action OnReportRealEndlessLevelCompleted;
    public static Action OnReportRealEndlessLevelFailed;
    public static Action OnEndlessModeCompleted;
    public static Action OnEndlessHardcoreModeCompleted;
    public static Action OnLevelStart;
    public static Action OnMutatedAnimalPopped;
    public static Action OnAnimalLevelCompleted;
    public static Action OnEndlessLevelCompleted;
    public static Action<int> OnCampaignStateReported;
    public static Action<bool> OnGamePaused;
    public static Action OnStartSpinningUpgradeSlots;
    public static Action OnUpgradeSlotsSpun;
    public static Action OnEndlessAppetiteTriggered;
    public static Action OnMomentumFeastTriggered;
    public static Action<Vector3, float, float> OnGravityPulseTriggered;
    public static Action OnEndlessUpgradeChosen;
    public static Action OnFTUETriggered;
    public static Action OnFTUEEnded;
    public static Action<float> OnLastCameraZoomReported;
    public static Action<int> OnIdlerMaxConcurrentAnimalsIncreased;
    public static Action OnIdlerScoreThresholdReached;
    public static Action OnResetEndlessStats;
    
    // UPGRADES

    public static Action OnUpgradeUpgraded;
}