using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_new : MonoBehaviour
{
    [SerializeField] public int score;
    [SerializeField] public float speed;
    [SerializeField] public string enemyName;
    [SerializeField] public int health;
    [SerializeField] public int maxHealth;
    [SerializeField] Sprite[] sprites;  // 평소 : 0 / 피격시 : 1
    [SerializeField] public GameObject bulletEnemyA;
    [SerializeField] public GameObject bulletEnemyB;
    [SerializeField] public GameObject ItemCoin;
    [SerializeField] public GameObject ItemPower;
    [SerializeField] public GameObject ItemBoom;
    [SerializeField] public float maxShotDelay;
    [SerializeField] public float curShotDelay;
    public GameObject player;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        OnFire_A();
        Reload();
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnFire_A()
    {
        if (curShotDelay < maxShotDelay) return;

        if (enemyName == "S" || enemyName == "M")
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA"); // 인스턴스로 생성
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("bulletEnemyA");
            GameObject bulletL = objectManager.MakeObj("bulletEnemyA");

            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 5, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
        }
        curShotDelay = 0;
    }

    public void OnHit(int damage)
    {
        if (health <= 0) return;

        health -= damage;

        spriteRenderer.sprite = sprites[1];
        Invoke("RetrunSprite", 0.1f); // 시간차 실행

        // HP가 0이하가 되면 삭제
        if (health <= 0)
        {
            player_new playerLogic = player.GetComponent<player_new>();
            playerLogic.score += this.score;

            // 아이템 랜덤 드랍
            int ran = Random.Range(0, 10);
            if (ran > 8)
            {
                // Boom
                GameObject itemBoom = objectManager.MakeObj("itemBoom");
                itemBoom.transform.position = transform.position;
            }
            else if (ran > 6)
            {
                // PowerBoom
                GameObject itemPower = objectManager.MakeObj("itemPower");
                itemPower.transform.position = transform.position;
            }
            else if (ran > 5)
            {
                // Coin;
                GameObject itemCoin = objectManager.MakeObj("itemCoin");
                itemCoin.transform.position = transform.position;
            }
            transform.rotation = Quaternion.identity;
            this.gameObject.SetActive(false);
        }
    }

    private void RetrunSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {   
            this.gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
        }
    }
}
