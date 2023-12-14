using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkierSpawner : MonoBehaviour
{


    public GameObject slowSkierPrefab;

    public float slowSkierSpawnChance = 0.3f;
    public GameObject mediumSkier;
    public float mediumSkierSpawnChance = 0.7f;
    public GameObject fastSkier;
    public float fastSkierSpawnChance = 0.7f;
    public float spawnCooldown=5f;
    public float timeSinceSpawn=0f;
    public int minSkiersSpawnAmount=1;
    public int maxSkiersSpawnAmount=3;

    public bool spawn = true;

    public float percentageDeviation=0.3f;





    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn+=Time.deltaTime;
        if(timeSinceSpawn>spawnCooldown && spawn){
            
            timeSinceSpawn=0f;
            int spawnUnits = Random.Range(minSkiersSpawnAmount,maxSkiersSpawnAmount);
            for(int i = 0; i<spawnUnits;i++) {
                float number = Random.Range(0.0f, 1.0f);
                GameObject skierPrefab;
                if(number<=slowSkierSpawnChance){
                    skierPrefab = slowSkierPrefab;
                }
                else if(number>=slowSkierSpawnChance && number <= 1-fastSkierSpawnChance){
                    skierPrefab = mediumSkier;
                }
                else{
                    skierPrefab = fastSkier;
                }

                Vector3 spawnPosition = new Vector3(transform.position.x+Random.Range(-5,5), transform.position.y,transform.position.z);
                var skier = Instantiate(skierPrefab, spawnPosition, Quaternion.identity);
                TurnController turnScript = skier.GetComponent<TurnController>();
                SocialScript socialScript = skier.GetComponent<SocialScript>();
                

                turnScript.turnForce *= RandomGaussian(1-percentageDeviation,1+percentageDeviation);
                turnScript.turnSwitchDelay *= RandomGaussian(1-percentageDeviation,1+percentageDeviation);
                turnScript.minTurnSpeed *= RandomGaussian(1-percentageDeviation,1+percentageDeviation);
                turnScript.minTurnAllowedDistance *= RandomGaussian(1-percentageDeviation,1+percentageDeviation);
                turnScript.minSpeed *= Mathf.Min(RandomGaussian(1-percentageDeviation,1+percentageDeviation),0.8f*turnScript.minTurnSpeed);
                turnScript.nextTurnLeft = (Random.Range(0, 2) == 1);

                socialScript.speedTimesViewDistance *= RandomGaussian(1-percentageDeviation,1+percentageDeviation);
            }
        }

    }

    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
{
    float u, v, S;

    do
    {
        u = 2.0f * UnityEngine.Random.value - 1.0f;
        v = 2.0f * UnityEngine.Random.value - 1.0f;
        S = u * u + v * v;
    }
    while (S >= 1.0f);

    // Standard Normal Distribution
    float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

    // Normal Distribution centered between the min and max value
    // and clamped following the "three-sigma rule"
    float mean = (minValue + maxValue) / 2.0f;
    float sigma = (maxValue - mean) / 3.0f;
    return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
}
}
