using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] Sprite[] sprites;  // 평소 : 0 / 피격식 : 1

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down  * speed;
    }

    private void OnHit(int damage)
    {
        health -= damage;

        spriteRenderer.sprite = sprites[1];
        Invoke("RetrunSprite", 0.1f); // 시간차 실행

        // HP가 0이하가 되면 삭제
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
