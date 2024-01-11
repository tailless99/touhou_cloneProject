using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public string enemyName;
    [SerializeField] public int health;
    [SerializeField] Sprite[] sprites;  // ��� : 0 / �ǰݽ� : 1
    [SerializeField] public GameObject bulletObjectA;
    [SerializeField] public GameObject bulletObjectB;
    [SerializeField] public float maxShotDelay;
    [SerializeField] public float curShotDelay;
    public GameObject player;

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

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnFire_A()
    {
        if (curShotDelay < maxShotDelay) return;

        if (enemyName == "S" || enemyName == "M")
        {
            GameObject bullet = Instantiate(bulletObjectA, transform.position, transform.rotation); // �ν��Ͻ��� ����
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        } 
        else if(enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.3f, transform.rotation); // �ν��Ͻ��� ����
            GameObject bulletL = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.3f, transform.rotation); // �ν��Ͻ��� ����

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
            
            rigidR.AddForce(dirVecR.normalized * 5, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 5, ForceMode2D.Impulse);
        }
        curShotDelay = 0;
    }

    private void OnHit(int damage)
    {
        health -= damage;

        spriteRenderer.sprite = sprites[1];
        Invoke("RetrunSprite", 0.1f); // �ð��� ����

        // HP�� 0���ϰ� �Ǹ� ����
        if(health <= 0) Destroy(this.gameObject);
    }

    private void RetrunSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            Destroy(collision.gameObject);
        }
    }
}
