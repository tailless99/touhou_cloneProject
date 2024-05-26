using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    Animator animator;
    SpriteRenderer spriteRenderer;

    public int patternIndex;    // 패턴번호
    public int curPattenCount;  // 현재 패턴 실행횟수
    public int[] maxPattenCount;// 최대 패턴 실행횟수

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyName == "B")
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (enemyName == "B") return;

        OnFire_A();
        Reload();
    }

    private void OnEnable()
    {
        health = maxHealth;

        if (enemyName == "B")
        {
            Invoke("Stop", 2);
        }
    }

    private void Stop()
    {
        if (!gameObject.activeSelf) return;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector3.zero;

        Invoke("Think", 2);
    }

    private void Think()
    {
        patternIndex = (patternIndex == 4) ? 0 : patternIndex + 1;
        curPattenCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;

            case 1:
                FireShot();
                break;

            case 2:
                FireArc();
                break;

            case 3:
                FireAround();
                break;
        }
    }

    private void FireFoward()
    {
        // 정방으로 4발 발사
        GameObject bulletR = objectManager.MakeObj("bulletBossA");
        GameObject bulletRR = objectManager.MakeObj("bulletBossA");
        GameObject bulletL = objectManager.MakeObj("bulletBossA");
        GameObject bulletLL = objectManager.MakeObj("bulletBossA");

        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        bulletRR.transform.position = transform.position + Vector3.right * 0.6f;
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.6f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        // 패턴 카운팅
        curPattenCount++;

        if (curPattenCount < maxPattenCount[patternIndex])
        {
            Invoke("FireFoward", 2);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    private void FireShot()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyB"); // 인스턴스로 생성
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            // 방향설정
            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-1.2f, 1.2f), Random.Range(0, 4f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }


        curPattenCount++;

        if (curPattenCount < maxPattenCount[patternIndex])
            Invoke("FireShot", 2);
        else
        {
            Invoke("Think", 3);
        }
    }

    private void FireArc()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA"); // 인스턴스로 생성
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            // 방향설정
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPattenCount/ maxPattenCount[patternIndex]), -1);
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        curPattenCount++;

        if (curPattenCount < maxPattenCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireAround()
    {
        // 한 패턴에 발사 총알 갯수
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPattenCount % 2 == 0 ? roundNumA : roundNumB;

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = objectManager.MakeObj("bulletBossB"); // 인스턴스로 생성
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            // 방향설정
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum),
                                        Mathf.Sin(Mathf.PI * 2 * i / roundNum));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            // Rotation
            // 진행 방향값에서 90도를 더해줘야 진행방향으로 돌릴 수 있다.
            Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        curPattenCount++;

        if (curPattenCount < maxPattenCount[patternIndex])
            Invoke("FireAround", 2);
        else
        {
            Invoke("Think", 3);
        }
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

        if (enemyName == "B")
        {
            animator.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("RetrunSprite", 0.1f); // 시간차 실행
        }
        // HP가 0이하가 되면 삭제
        if (health <= 0)
        {
            player playerLogic = player.GetComponent<player>();
            playerLogic.score += this.score;



            // 아이템 랜덤 드랍
            int ran = enemyName == "B" ? 0 : Random.Range(0, 10);
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
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
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
