using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
#if UNITY_DEITOR
using static UnityEditor.Timeline.TimelinePlaybackControls;
#endif

public class player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] public GameObject bulletPlayerA;
    [SerializeField] public GameObject bulletPlayerB;
    [SerializeField] public GameObject boomEffect;
    [SerializeField] public GameObject[] followers;
    [SerializeField] public float maxShotDelay;
    [SerializeField] public float curShotDelay;
    [SerializeField] public float bulletPower;
    [SerializeField] public float maxBulletPower;
    [SerializeField] public int boom;
    [SerializeField] public int maxBoom;
    [SerializeField] public GameManager gameManager;
    [SerializeField] public ObjectManager objectManager;
    [SerializeField] public int life;
    [SerializeField] public int score;
    

    Vector2 moveVector2 = Vector2.zero;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    public bool FireBtnPress;
    bool isBoomTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        // �����̴� ���� ĸ��ȭ
        MoveCheck();

        // �Ϲ� ����
        if (FireBtnPress)
        {
            OnFire_A();
            Reload();

            // �����߿� �Ծ �ȷο��� �����ϵ��� ������Ʈ���� �ٽ� üũ
            foreach (var Actor in followers)
            {
                if (Actor.activeSelf)
                    Actor.GetComponent<Follower>().OnFire_TypeA(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            // �ߺ� ���� ����
            if (gameObject.activeSelf)
            {
                // ��� ���̴� ����
                life--;
                gameManager.UpdateLifeIcon(life);

                // ������ or ���ӿ��� �Լ� ����
                if (life == 0) gameManager.gameOver();
                else gameManager.ReSpawnPlayer();

                // �÷��̾� ��Ȱ��ȭ
                gameObject.SetActive(false);
                collision.gameObject.SetActive(false);
            }
        }
        // �������� ���� ���
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    bulletPower += 1;
                    AddFollower();
                    if (bulletPower >= maxBulletPower)
                    {   // ������ �Ŀ��� �ִ�ġ�� �Ѵ´ٸ� �ִ�ġ������ �ʱ�ȭ
                        bulletPower = maxBulletPower;
                        score += 500;
                    }
                    break;
                case "Boom":
                    boom += 1;
                    if (boom >= maxBoom)
                    {   // ������ ��ź�� �ִ�ġ�� �Ѵ´ٸ� �ִ�ġ������ �ʱ�ȭ
                        boom = maxBoom;
                        score += 500;
                    }
                    gameManager.UpdateBoomIcon(boom);
                    break;
            }
            collision.gameObject.SetActive(false);  
        }
    }

    public void AddFollower()
    {
        if (bulletPower == 4)
            followers[0].SetActive(true);
        else if (bulletPower == 5)
            followers[1].SetActive(true);
        else if (bulletPower == 6)
            followers[2].SetActive(true);
    }

    private void offBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnGameQuitActive(InputAction.CallbackContext context)
    {
        if (context.started) gameManager.TogglePauseGameSetting();
    }

    public void OnFire_A()
    {
        if (curShotDelay < maxShotDelay) return;

        switch (bulletPower)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("bulletPlayerA"); // �ν��Ͻ��� ����
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletL = objectManager.MakeObj("bulletPlayerA");
                GameObject bulletR = objectManager.MakeObj("bulletPlayerA");

                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default :
                GameObject bulletLL = objectManager.MakeObj("bulletPlayerA");
                GameObject bulletCC = objectManager.MakeObj("bulletPlayerB");
                GameObject bulletRR = objectManager.MakeObj("bulletPlayerA");

                bulletLL.transform.position = transform.position + Vector3.left * 0.1f;
                bulletCC.transform.position = transform.position;
                bulletRR.transform.position = transform.position + Vector3.right * 0.1f;
                
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }


        curShotDelay = 0;
    }

    // ���Ӱ����� ���� ����Ű�� ���ȴ����� üũ��
    public void OnFire_TypeA(InputAction.CallbackContext context)
    {
        if (context.started) { 
            FireBtnPress = true;

            foreach (var Actor in followers)
            {
                if (Actor.activeSelf)
                    Actor.GetComponent<Follower>().OnFire_TypeA(true);
            }
        }

        if (context.canceled)
        {
            FireBtnPress = false;
            foreach (var Actor in followers)
            {
                if (Actor.activeSelf)
                    Actor.GetComponent<Follower>().OnFire_TypeA(false);
            }
        }
    }

    public void OnFirePress(InputAction.CallbackContext context) {
        if (context.started)
        {
            foreach (var Actor in followers)
            {
                if (Actor.activeSelf)
                    Actor.GetComponent<Follower>().OnFire_TypeA(true);
            }
        }
    }

    public void OnBoom(InputAction.CallbackContext context) {
        // ����üũ
        if (isBoomTime) return; // ��ź�� �߻����ΰ�
        if (boom == 0) return;   // ��Ÿ�� ������ 0�ΰ�

        boom--;
        isBoomTime = true;

        gameManager.UpdateBoomIcon(boom);
        boomEffect.SetActive(true);
        Invoke("offBoomEffect", 2.5f);

        // �������� ���ʹ̵鿡�� ����ó��
        GameObject[] enemiesL = objectManager.getPool("enemyL");
        GameObject[] enemiesM = objectManager.getPool("enemyM");
        GameObject[] enemiesS = objectManager.getPool("enemyS");
        GameObject[] enemiesB = objectManager.getPool("enemyB");
        
        foreach (GameObject enemy in enemiesL)
        {
            if (enemy.activeSelf) {
                Enemy enemyLogic = enemy.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        foreach (GameObject enemy in enemiesM)
        {
            if (enemy.activeSelf)
            {
                Enemy enemyLogic = enemy.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        foreach (GameObject enemy in enemiesS)
        {
            if (enemy.activeSelf)
            {
                Enemy enemyLogic = enemy.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
        foreach (GameObject enemy in enemiesB)
        {
            if (enemy.activeSelf)
            {
                Enemy enemyLogic = enemy.GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        // �������� �Ҹ��� ����ó��
        GameObject[] bulletEnemyA = objectManager.getPool("bulletEnemyA");
        GameObject[] bulletEnemyB = objectManager.getPool("bulletEnemyB");
        GameObject[] bulletBossA = objectManager.getPool("bulletBossA");
        GameObject[] bulletBossB = objectManager.getPool("bulletBossB");
            
        
        foreach (GameObject bullet in bulletEnemyA)
        {
            if(bullet.activeSelf) bullet.SetActive(false);
        }
        foreach (GameObject bullet in bulletEnemyB)
        {
            if (bullet.activeSelf) bullet.SetActive(false);
        }
        foreach (GameObject bullet in bulletBossA)
        {
            if (bullet.activeSelf) bullet.SetActive(false);
        }
        foreach (GameObject bullet in bulletBossB)
        {
            if (bullet.activeSelf) bullet.SetActive(false);
        }

    }

    public void OnGrazeMove(InputAction.CallbackContext context)
    {
        if (context.started) moveSpeed /= 2;
        if (context.canceled) moveSpeed *= 2;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // �Է¹��� Ű ���� Vector2 ��ü�� �����Ͽ� ������(��ġ) ����
        moveVector2 = context.ReadValue<Vector2>();
    }

    // ���� �����°� ���� üũ�Ͽ�, ������ �� �ִٸ� �����δ�.
    public void MoveCheck()
    {
        Vector2 moveDirection = moveVector2.normalized;

        // �¿� �̵� �ִϸ��̼��� ���� bool�� ����
        bool rightCheck = moveDirection.x > 0;
        bool leftCheck = moveDirection.x < 0;
        animator.SetBool(AnimationStrings.IsRight, rightCheck);
        animator.SetBool(AnimationStrings.IsLeft, leftCheck);

        if (touchingDirections.IsTouchingLeft)
        {   // ���ʿ� ���� ���� ���, �������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.x < 0) moveDirection.x = 0;
        }
        if (touchingDirections.IsTouchingRight)
        {   // �����ʿ� ���� ���� ���, ���������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.x > 0) moveDirection.x = 0;
        }
        if (touchingDirections.IsTouchingTop)
        {   // ���ʿ� ���� ���� ���, �������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.y > 0) moveDirection.y = 0;
        }
        if (touchingDirections.IsTouchingDown)
        {   // �Ʒ��ʿ� ���� ���� ���, �Ʒ������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.y < 0) moveDirection.y = 0;
        }

        // �� �����Ӹ��� �����Ǻ���
        rb.velocity = moveDirection * moveSpeed;
    }
}
