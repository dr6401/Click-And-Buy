using System;
using System.Collections;
using UnityEngine;

public class SonarPulseSpawner : MonoBehaviour
{
    public GameObject sonarPulsePrefab;
    public int amountSpawned = 10;
    public float durationBetweenSpawns = 0.5f;
    public float maxDurationOfSonarPulseLifetime;
    public bool isPredictedPriceHigherThanCurrentPrice;

    private void Start()
    {
        PredictionData predictionData = gameObject.GetComponent<PredictionData>();
        maxDurationOfSonarPulseLifetime = predictionData.secondsIntoFuturePredicted;
        isPredictedPriceHigherThanCurrentPrice = predictionData.isPredictedPriceHigherThanCurrentPrice;
        SpawnSonarPulses(amountSpawned, transform);
    }

    public void SpawnSonarPulses(int amount, Transform parent)
    {
        StartCoroutine(SpawnSonarPulses(amount, durationBetweenSpawns, parent));
    }

    private IEnumerator SpawnSonarPulses(int amount, float delay, Transform parent)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject spawnedSonarPulse = Instantiate(sonarPulsePrefab, parent);
            SonarPulse sonarPulseScript = spawnedSonarPulse.GetComponent<SonarPulse>();
            sonarPulseScript.lifeTime = maxDurationOfSonarPulseLifetime;
            sonarPulseScript.isPredictedPriceHigherThanCurrent = isPredictedPriceHigherThanCurrentPrice;
            yield return new WaitForSeconds(delay);
            maxDurationOfSonarPulseLifetime -= delay;
        }
    } 
}
