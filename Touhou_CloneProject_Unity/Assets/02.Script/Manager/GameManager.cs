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
    [SerializeField] public GameObject player;

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

    public void ReSpawnPlayer()
    {
        Invoke("ReSpawnPlayerExe", 2f);
    }

    void ReSpawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);
    }

    private void SpawnEnemy()
    {
        int ranEnemy = UnityEngine.Random.Range(0, 3);
        int ranPoint = UnityEngine.Random.Range(0, 9);
        GameObject enemy = Instantiate(enemyObjects[ranEnemy],
                                    spawnPoints[ranPoint].position,
                                    spawnPoints[ranPoint].rotation);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;

        if(ranPoint == 6 || ranPoint == 8) // Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rb.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (ranPoint == 5 || ranPoint == 7) // Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rb.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else
        {
            rb.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }
    }
}
