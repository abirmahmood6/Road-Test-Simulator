using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PedestrianPrefabData
{
    public GameObject pedestrianPrefab;
    public Transform[] spawnPoints;
}

public class Spawner : MonoBehaviour
{
    // Array of pedestrian prefabs and their associated spawn points
    public PedestrianPrefabData[] pedestrianPrefabData;

    // Spawn interval
    public float spawnInterval = 3f;

    // Timer for spawning pedestrians
    private float spawnTimer = 0f;

    void Update()
    {
        // Increment spawn timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a pedestrian
        if (spawnTimer >= spawnInterval)
        {
            // Reset spawn timer
            spawnTimer = 0f;

            // Spawn a pedestrian at a specific spawn point
            SpawnPedestrian();
        }
    }

    void SpawnPedestrian()
    {
        // Randomly select a pedestrian prefab
        int randomIndex = Random.Range(0, pedestrianPrefabData.Length);
        PedestrianPrefabData selectedPrefabData = pedestrianPrefabData[randomIndex];

        // Randomly select a spawn point for the selected pedestrian prefab
        int randomSpawnIndex = Random.Range(0, selectedPrefabData.spawnPoints.Length);
        Transform spawnPoint = selectedPrefabData.spawnPoints[randomSpawnIndex];

        // Instantiate the selected pedestrian prefab at the randomly selected spawn point
        GameObject newPedestrian = Instantiate(selectedPrefabData.pedestrianPrefab, spawnPoint.position, Quaternion.identity);

        // Add cleanup logic to destroy the pedestrian after a certain amount of time
        Destroy(newPedestrian, 10f); // Adjust the time as needed
    }
}