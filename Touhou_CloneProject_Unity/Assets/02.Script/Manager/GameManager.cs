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
        // 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 파일 읽기
        TextAsset textData = Resources.Load("Stage 0") as TextAsset; // 읽은 데이터가 TextAsset이 아니면 널처리됨
        StringReader reader = new StringReader(textData.text);

        while (reader != null)
        { 
            // 한줄씩 읽어서 line에 대입
            string line = reader.ReadLine();
            if (line == null) break;

            // 리스트 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // 파일 닫기
        reader.Close();
        // 첫번째 스폰 딜레이 적용
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

        // 리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        // 다음 리스폰 딜레이 갱신
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
