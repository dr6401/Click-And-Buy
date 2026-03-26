using System;
using UnityEngine;

/*public class AnimalScoreManager : MonoBehaviour
{
    public static AnimalScoreManager Instance;
    public AnimalScoreTrackerSO animalScoreTrackerSO;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddAnimalScore(AnimalType.TypeOfAnimal type)
    {
        switch (type)
        {
            case AnimalType.TypeOfAnimal.Chicken:
                animalScoreTrackerSO.chickensConsumed++;
                break;
            case AnimalType.TypeOfAnimal.Pig:
                animalScoreTrackerSO.pigsConsumed++;
                break;
            case AnimalType.TypeOfAnimal.Sheep:
                animalScoreTrackerSO.sheepConsumed++;
                break;
            case AnimalType.TypeOfAnimal.Cow:
                animalScoreTrackerSO.cowsConsumed++;
                break;
            case AnimalType.TypeOfAnimal.Chick:
                animalScoreTrackerSO.chicksConsumed++;
                break;
        }
    }

    public int GetAnimalScore(AnimalType.TypeOfAnimal type)
    {
        switch (type)
        {
            case AnimalType.TypeOfAnimal.Chicken:
                return animalScoreTrackerSO.chickensConsumed;
            case AnimalType.TypeOfAnimal.Pig:
                return animalScoreTrackerSO.pigsConsumed;
            case AnimalType.TypeOfAnimal.Sheep:
                return animalScoreTrackerSO.sheepConsumed;
            case AnimalType.TypeOfAnimal.Cow:
                return animalScoreTrackerSO.cowsConsumed;
            case AnimalType.TypeOfAnimal.Chick:
                return animalScoreTrackerSO.chicksConsumed;
            default:
                Debug.LogWarning($"Invalid type: {type}! Some animal prefab doesn't have the correct TypeOfAnimal set");
                return 0;
        }
    }

    public void ResetAllAnimalScores()
    {
        animalScoreTrackerSO.chickensConsumed = 0;
        animalScoreTrackerSO.pigsConsumed = 0;
        animalScoreTrackerSO.sheepConsumed = 0;
        animalScoreTrackerSO.cowsConsumed = 0;
        animalScoreTrackerSO.chicksConsumed = 0;
    }
}*/