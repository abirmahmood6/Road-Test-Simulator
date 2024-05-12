using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class AICarSpawner : MonoBehaviour
{
    // Array of AI car prefabs
    public GameObject[] aiCarPrefabs;

    // Array of spawn points for each AI car
    public Transform[] spawnPoints;

    // Interval between spawns for all AI cars
    public float spawnInterval = 5f;

    // Timer to control spawning
    private float spawnTimer = 0f;

    void Update()
    {
        // Update the spawn timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn
        if (spawnTimer >= spawnInterval)
        {
            // Reset the spawn timer
            spawnTimer = 0f;

            // Spawn AI cars
            for (int i = 0; i < aiCarPrefabs.Length; i++)
            {
                SpawnAICar(aiCarPrefabs[i], spawnPoints[i]);
            }
        }
    }

    void SpawnAICar(GameObject aiCarPrefab, Transform spawnPoint)
    {
        // Instantiate the AI car at the chosen spawn point
        GameObject aiCar = Instantiate(aiCarPrefab, spawnPoint.position, spawnPoint.rotation);

        // Set the spawned AI car's parent to this spawner
        aiCar.transform.SetParent(transform);

        Destroy(aiCar, 20f);
    }
}
