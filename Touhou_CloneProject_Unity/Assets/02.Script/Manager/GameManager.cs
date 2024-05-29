using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

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
    [SerializeField] public GameObject gameWinSet;
    [SerializeField] public GameObject gameExit;
    [SerializeField] public ObjectManager objectManager;

    [SerializeField] public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
    public bool isPaused;

    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjects = new string[] { "enemyS", "enemyM", "enemyL", "enemyB" };
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        // �ʱ�ȭ
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // ���� �б�
        TextAsset textData = Resources.Load("Stage 0") as TextAsset; // ���� �����Ͱ� TextAsset�� �ƴϸ� ��ó����
        StringReader reader = new StringReader(textData.text);

        while (reader != null)
        { 
            // ���پ� �о line�� ����
            string line = reader.ReadLine();
            if (line == null) break;

            // ����Ʈ ������ ����
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // ���� �ݱ�
        reader.Close();
        // ù��° ���� ������ ����
        maxSpawnDelay = spawnList[0].delay;
    }

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
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
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
        
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjects[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;
        enemy.transform.rotation = Quaternion.identity;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        if(enemyPoint == 6 || enemyPoint == 8) // Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rb.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (enemyPoint == 5 || enemyPoint == 7) // Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rb.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else
        {
            rb.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        // ������ �ε��� ����
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        // ���� ������ ������ ����
        maxSpawnDelay = spawnList[spawnIndex].delay;
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

    public void gameWin()
    {
        gameWinSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void TogglePauseGameSetting()
    {
        isPaused = !isPaused;
        gameExit.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
