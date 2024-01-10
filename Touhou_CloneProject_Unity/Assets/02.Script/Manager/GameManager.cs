using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyObjects;
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public float maxSpawnDelay;
    [SerializeField] public float curSpawnDelay;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = UnityEngine.Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }
    }

    private void SpawnEnemy()
    {
        int ranEnemy = UnityEngine.Random.Range(0, 3);
        int ranPoint = UnityEngine.Random.Range(0, 5);
        Instantiate(enemyObjects[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);
    }
}
