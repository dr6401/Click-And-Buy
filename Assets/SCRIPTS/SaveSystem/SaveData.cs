using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SaveData
{
    public CampaignData campaignData = new CampaignData();
    public RealEndlessData realEndlessData = new RealEndlessData();
    public ChallengerData challengerData = new ChallengerData();
    public IdlerData idlerData = new IdlerData();
    public GeneralData generalData = new GeneralData();
    public SettingsData settingsData = new SettingsData();

    [System.Serializable]
    public class SettingsData
    {
        public bool hasPlayerSeenIntro;
        public bool hasPlayerCompletedFTUE;
        public float masterVolume = 0.5f;
        public float musicVolume = 0.25f;
        public float sfxVolume = 0.5f;
        public int targetFPS = -1;
        public bool screenShakeEnabled = false;
        public float lastCameraZoom = 15f;
    }
    [System.Serializable]
    public class CampaignData
    {
        public int campaignProgressLevel;
        public int campaignChickensConsumed;
        public int campaignPigsConsumed;
        public int campaignSheepConsumed;
        public int campaignCowsConsumed;
        public int campaignChicksConsumed;
        public List<SerializableBoolEntry> campaignUnlockedUpgrades =
            new List<SerializableBoolEntry>();
        public int animalsConsumedLastRound;
        public int campaignCurrentMoney;
    }
    
    [System.Serializable]
    public class RealEndlessData
    {
        public int currentWave = 1;
        public int animalsToPopIncrease = 1;
        public int animalsToSpawn = 10;
        public int animalsToPop = 1;
        
        public float blackHolePullForce = 12f;
        public float blackHolePullRadius = 6.5f;
        public float blackHoleScale = 2.4f;
        public float blackHoleMoveSpeed = 7.5f;
    
        public int basePerLevelAnimalsToSpawn = 10;
        public float chickensSpawnMultiplier = 0f;
        
        public float momentumFeastSpeedMultiplier = 1f;
        public float momentumFeastDecayTime = 3f;
        public int momentumFeastAnimalsToConsumeAmount = 10000;

        public bool hasGravityPulseBeenUnlocked = false;
        public float gravityPulseInterval = 7f;
        public float gravityPulseForce = 1f;
        public float gravityPulseDuration = 0.2f;

        public float shepherdWanderStrengthReduction = 0f;
        public float shepherdWanderDurationReduction = 0f;

        public bool hasSingularityBeenUnlocked = false;
        public float singularitySpawnInterval = 5.25f;
        public float singularityDuration = 3f;
        public float singularitySize = 3f;

        public float levelDuration = 15f;

        public float moneyMultiplier = 1f;
        
        public float endlessAppetiteDurationExtensionChance = 0f;
        public float endlessAppetiteDurationExtensionTime = 0.1f;
        
        public float animalDigestionSpeed = 5f; 
        public float animalDecaySpeed = 1f;
        
        public float riskyMetabolismMoneyMultiplier = 1f;
        public float riskyMetabolismExtraDigestionTimeMultiplier = 1f;
        
        public float consumptionChainIncreaseRate = 0.01f;
        public float consumptionChainTriggerChance = 0f;
        public float currentRunMetabolismRateIncrease = 0f;
        
        public bool hasCosmicLaserBeenUnlocked = false;
        public float cosmicLaserSpawnInterval = 7.25f;
        public float cosmicLaserDuration = 2.5f;
        public float cosmicLaserSize = 1.33f;
        
        public bool hasOverchargeBeenUnlocked = false;
        public float overchargeSizeMultiplier = 3;
        public float overchargeDuration = 2.5f;
        
        public bool unlockedHorizonExpansion = false;
        public float horizonExpansionGrowthRate = 0.01f;
        public bool unlockedMutation = false;
        public float mutationChance = 0.0f;
        public float mutationSlowerConsumptionMultiplier = 0.2f;
    }
    
    [System.Serializable]
    public class ChallengerData
    {
        public bool isHardcore = false;
        
        // Normal Challenger Data
        public int currentWaveChallenger = 1;
        public int animalsToPopIncreaseChallenger = 1;
        public int animalsToSpawnChallenger = 10;
        public int animalsToPopChallenger = 1;
        
        public float blackHolePullForceChallenger = 12f;
        public float blackHolePullRadiusChallenger = 6.5f;
        public float blackHoleScaleChallenger = 2.4f;
        public float blackHoleMoveSpeedChallenger = 7.5f;
    
        public int basePerLevelAnimalsToSpawnChallenger = 10;
        public float chickensSpawnMultiplierChallenger = 0f;
        public float spawnIntervalChallenger = 0.05f;
        
        public float momentumFeastSpeedMultiplierChallenger = 1f;
        public float momentumFeastDecayTimeChallenger = 3f;
        public int momentumFeastAnimalsToConsumeAmountChallenger = 10000;

        public bool hasGravityPulseBeenUnlockedChallenger = false;
        public float gravityPulseIntervalChallenger = 7f;
        public float gravityPulseForceChallenger = 1f;
        public float gravityPulseDurationChallenger = 0.2f;

        public float shepherdWanderStrengthReductionChallenger = 0f;
        public float shepherdWanderDurationReductionChallenger = 0f;

        public bool hasSingularityBeenUnlockedChallenger = false;
        public float singularitySpawnIntervalChallenger = 5.25f;
        public float singularityDurationChallenger = 3f;
        public float singularitySizeChallenger = 3f;

        public float baseLevelDurationChallenger = 15f;
        public float additionalLevelDurationChallenger = 0f;

        public float moneyMultiplierChallenger = 1f;
        
        public float endlessAppetiteDurationExtensionChanceChallenger = 0f;
        public float endlessAppetiteDurationExtensionTimeChallenger = 0.1f;
        
        public float animalDigestionSpeedChallenger = 5f; 
        public float animalDecaySpeedChallenger = 1f;
        
        public float riskyMetabolismMoneyMultiplierChallenger = 1f;
        public float riskyMetabolismExtraDigestionTimeMultiplierChallenger = 1f;
        
        public float consumptionChainIncreaseRateChallenger = 0.01f;
        public float consumptionChainTriggerChanceChallenger = 0f;
        public float currentRunMetabolismRateIncreaseChallenger = 0f;
        
        public bool hasCosmicLaserBeenUnlockedChallenger = false;
        public float cosmicLaserSpawnIntervalChallenger = 7.25f;
        public float cosmicLaserDurationChallenger = 2.5f;
        public float cosmicLaserSizeChallenger = 1.33f;
        
        public bool hasOverchargeBeenUnlockedChallenger = false;
        public float overchargeSizeMultiplierChallenger = 2;
        public float overchargeDurationChallenger = 2.5f;
        
        public bool unlockedHorizonExpansionChallenger = false;
        public float horizonExpansionGrowthRateChallenger = 0.01f;
        public bool unlockedMutationChallenger = false;
        public float mutationChanceChallenger = 0.0f;
        public float mutationSlowerConsumptionMultiplierChallenger = 0.2f;
        
        // Hardcore Challenger Data
        public int currentWaveChallengerHardcore = 1;
        public int animalsToPopIncreaseChallengerHardcore = 1;
        public int animalsToSpawnChallengerHardcore = 10;
        public int animalsToPopChallengerHardcore = 1;

        public float blackHolePullForceChallengerHardcore = 12f;
        public float blackHolePullRadiusChallengerHardcore = 6.5f;
        public float blackHoleScaleChallengerHardcore = 2.4f;
        public float blackHoleMoveSpeedChallengerHardcore = 7.5f;

        public int basePerLevelAnimalsToSpawnChallengerHardcore = 10;
        public float chickensSpawnMultiplierChallengerHardcore = 0f;
        public float spawnIntervalChallengerHardcore = 0.05f;

        public float momentumFeastSpeedMultiplierChallengerHardcore = 1f;
        public float momentumFeastDecayTimeChallengerHardcore = 3f;
        public int momentumFeastAnimalsToConsumeAmountChallengerHardcore = 10000;

        public bool hasGravityPulseBeenUnlockedChallengerHardcore = false;
        public float gravityPulseIntervalChallengerHardcore = 7f;
        public float gravityPulseForceChallengerHardcore = 1f;
        public float gravityPulseDurationChallengerHardcore = 0.2f;

        public float shepherdWanderStrengthReductionChallengerHardcore = 0f;
        public float shepherdWanderDurationReductionChallengerHardcore = 0f;

        public bool hasSingularityBeenUnlockedChallengerHardcore = false;
        public float singularitySpawnIntervalChallengerHardcore = 5.25f;
        public float singularityDurationChallengerHardcore = 3f;
        public float singularitySizeChallengerHardcore = 3f;

        public float baseLevelDurationChallengerHardcore = 15f;
        public float additionalLevelDurationChallengerHardcore = 0f;

        public float moneyMultiplierChallengerHardcore = 1f;

        public float endlessAppetiteDurationExtensionChanceChallengerHardcore = 0f;
        public float endlessAppetiteDurationExtensionTimeChallengerHardcore = 0.1f;

        public float animalDigestionSpeedChallengerHardcore = 5f; 
        public float animalDecaySpeedChallengerHardcore = 1f;

        public float riskyMetabolismMoneyMultiplierChallengerHardcore = 1f;
        public float riskyMetabolismExtraDigestionTimeMultiplierChallengerHardcore = 1f;

        public float consumptionChainIncreaseRateChallengerHardcore = 0.01f;
        public float consumptionChainTriggerChanceChallengerHardcore = 0f;
        public float currentRunMetabolismRateIncreaseChallengerHardcore = 0f;

        public bool hasCosmicLaserBeenUnlockedChallengerHardcore = false;
        public float cosmicLaserSpawnIntervalChallengerHardcore = 7.25f;
        public float cosmicLaserDurationChallengerHardcore = 2.5f;
        public float cosmicLaserSizeChallengerHardcore = 1.33f;

        public bool hasOverchargeBeenUnlockedChallengerHardcore = false;
        public float overchargeSizeMultiplierChallengerHardcore = 2;
        public float overchargeDurationChallengerHardcore = 2.5f;

        public bool unlockedHorizonExpansionChallengerHardcore = false;
        public float horizonExpansionGrowthRateChallengerHardcore = 0.01f;
        public bool unlockedMutationChallengerHardcore = false;
        public float mutationChanceChallengerHardcore = 0.0f;
        public float mutationSlowerConsumptionMultiplierChallengerHardcore = 0.2f;
    }
    
    [System.Serializable]
    public class IdlerData
    {
        public int animalsPoppedInIdlerMode = 0;
        
        public float blackHolePullForce = 18f;
        public float blackHolePullRadius = 15f;
        public float blackHoleScale = 2.4f;
        public float blackHoleMoveSpeed = 7.5f;
        
        public float animalDigestionSpeed = 4f; 
        public float animalDecaySpeed = 1f;
    }
    
    [System.Serializable]
    public class GeneralData
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
    }

    [System.Serializable]
    public class SerializableBoolEntry
    {
        public string key;
        public bool value;

        public SerializableBoolEntry(string key, bool value)
        {
            this.key = key;
            this.value = value;
        }
    }

    public static List<SerializableBoolEntry> DictToList(Dictionary<string, bool> dict)
    {
        List<SerializableBoolEntry> list = new List<SerializableBoolEntry>();
        foreach (var kvp in dict)
        {
            list.Add(new SerializableBoolEntry(kvp.Key, kvp.Value));
        }
        return list;
    }
}
