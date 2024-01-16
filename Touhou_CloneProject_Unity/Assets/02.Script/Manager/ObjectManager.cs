using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public GameObject enemyLFreb;
    [SerializeField] public GameObject enemyMFreb;
    [SerializeField] public GameObject enemySFreb;
    [SerializeField] public GameObject itemCoinFreb;
    [SerializeField] public GameObject itemPowerFreb;
    [SerializeField] public GameObject itemBoomFreb;
    [SerializeField] public GameObject bulletPlayerAFreb;
    [SerializeField] public GameObject bulletPlayerBFreb;
    [SerializeField] public GameObject bulletEnemyAFreb;
    [SerializeField] public GameObject bulletEnemyBFreb;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;
    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;

    GameObject[] targetPool;

    private void Awake()
    {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[1000];
        bulletPlayerB = new GameObject[1000];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];

        Generate();
    }

    private void Generate()
    {
        // Enemy
        for(int i=0; i < enemyL.Length; i++) {
            enemyL[i] = Instantiate(enemyLFreb);
            enemyL[i].SetActive(false);
        }
        for (int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemyMFreb);
            enemyM[i].SetActive(false);
        }
        for (int i = 0; i < enemyS.Length; i++)
        {
            enemyS[i] = Instantiate(enemySFreb);
            enemyS[i].SetActive(false);
        }

        // Item
        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoinFreb);
            itemCoin[i].SetActive(false);
        }
        for (int i = 0; i < itemPower.Length; i++)
        {
            itemPower[i] = Instantiate(itemPowerFreb);
            itemPower[i].SetActive(false);
        }
        for (int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemBoomFreb);
            itemBoom[i].SetActive(false);
        }

        // bullet
        for (int i = 0; i < bulletPlayerA.Length; i++)
        {
            bulletPlayerA[i] = Instantiate(bulletPlayerAFreb);
            bulletPlayerA[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayerB.Length; i++)
        {
            bulletPlayerB[i] = Instantiate(bulletPlayerBFreb);
            bulletPlayerB[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemyA.Length; i++)
        {
            bulletEnemyA[i] = Instantiate(bulletEnemyAFreb);
            bulletEnemyA[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemyB.Length; i++)
        {
            bulletEnemyB[i] = Instantiate(bulletEnemyBFreb);
            bulletEnemyB[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch(type)
        {
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }

    public GameObject[] getPool(string type)
    {
        switch (type)
        {
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
        }
        return targetPool;
    }
}