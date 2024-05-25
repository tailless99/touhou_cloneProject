using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Test : MonoBehaviour
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
    [SerializeField] public Transform[] Goals;
    public GameObject player;
    public ObjectManager objectManager;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    int goalIndex;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        goalIndex = 0;
    }

    private void Update()
    {
        //OnFire_A();
        Reload();
        Move();
    }

    private void Move()
    {
        Vector2 goalPosition = Vector2.zero;

        // 목적지가 반드시 있어야 한다.
        if (Goals.Length > 0)
        {
            switch (goalIndex)
            {
                // 직선 구간
                case 0:
                    goalPosition = Goals[goalIndex].position - transform.position;
                    transform.Translate(goalPosition.normalized * speed * Time.deltaTime);
                    break;
                // 원을 그리는 운동
                case 1:
                    goalPosition = Goals[goalIndex].position - transform.position;
                    transform.Translate(goalPosition.normalized * speed * Time.deltaTime);
                    break;
            }
        }

        // 인덱스가 목적지의 마지막 인덱스를 가리키는게 아니면 증가
        if (goalIndex <= Goals.Length && 
            0.1f > Vector2.Distance(transform.position, goalPosition)) goalIndex++;
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
