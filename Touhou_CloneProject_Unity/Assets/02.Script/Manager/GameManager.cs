using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //[SerializeField] public GameObject[] enemyObjects;
    [SerializeField] public string[] enemyObjects;
    [SerializeField] public Transform[] spawnPoints;
    [SerializeField] public float maxSpawnDelay;
    [SerializeField] public float curSpawnDelay;
    [SerializeField] public GameObject player;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public Image[] lifeImage;
    [SerializeField] public Image[] boomImage;
    [SerializeField] public GameObject gameOverSet;
    [SerializeField] public ObjectManager objectManager;

    private void Awake()
    {
        enemyObjects = new string[] { "enemyS", "enemyM", "enemyL" };
    }

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = UnityEngine.Random.Range(0.5f, 3f);
            curSpawnDelay = 0;
        }

        // UI Score Update
        player playerLogic = player.GetComponent<player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
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
        GameObject enemy = objectManager.MakeObj(enemyObjects[ranEnemy]);
        enemy.transform.position = spawnPoints[ranPoint].position;
        enemy.transform.rotation = Quaternion.identity;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

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

    public void UpdateLifeIcon(int life)
    {
        // UI Life Disable
        for(int i=life; i < lifeImage.Length; i++)
        {
            lifeImage[i].color = new Color(1,1,1,0);
        }
    }

    public void UpdateBoomIcon(int life)
    {
        // All Boom Icon Enable
        for (int i = 0; i < boomImage.Length; i++)
        {
            boomImage[i].color = new Color(1, 1, 1, 1);
        }

        // Use Boom Icon Disable
        for (int i = life; i < boomImage.Length; i++)
        {
            boomImage[i].color = new Color(1, 1, 1, 0);
        }
    }

    public void gameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
