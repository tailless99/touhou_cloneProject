using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float grazeSpeed = 3f;
    [SerializeField] public GameObject bulletObjectA;
    [SerializeField] public GameObject bulletObjectB;
    [SerializeField] public float maxShotDelay;
    [SerializeField] public float curShotDelay;
    [SerializeField] public float bulletPower;

    Vector2 moveVector2 = Vector2.zero;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    bool FireBtnPress;
    bool isOverLappingCollider;


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
        }        
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnFire_A()
    {
        if (curShotDelay < maxShotDelay) return;

        switch (bulletPower)
        {
            case 1:
                GameObject bullet = Instantiate(bulletObjectA, transform.position, transform.rotation); // �ν��Ͻ��� ����
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletL = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.1f, transform.rotation); // �ν��Ͻ��� ����
                GameObject bulletR = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.1f, transform.rotation);
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletLL = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.25f, transform.rotation); // �ν��Ͻ��� ����
                GameObject bulletCC = Instantiate(bulletObjectB, transform.position, transform.rotation);
                GameObject bulletRR = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.25f, transform.rotation);
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

    public void OnFire_TypeA(InputAction.CallbackContext context)
    {
        if (context.started) FireBtnPress = true;
        if (context.canceled) FireBtnPress = false;
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
        {
            // ���ʿ� ���� ���� ���, �������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.x < 0) moveDirection.x = 0;
        }
        if (touchingDirections.IsTouchingRight)
        {
            // �����ʿ� ���� ���� ���, ���������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.x > 0) moveDirection.x = 0;
        }
        if (touchingDirections.IsTouchingTop)
        {
            // ���ʿ� ���� ���� ���, �������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.y > 0) moveDirection.y = 0;
        }
        if (touchingDirections.IsTouchingDown)
        {
            // �Ʒ��ʿ� ���� ���� ���, �Ʒ������� ������ ���� �ִٸ� 0���� ����
            if (moveDirection.y < 0) moveDirection.y = 0;
        }

        // �� �����Ӹ��� �����Ǻ���
        rb.velocity = moveDirection * moveSpeed;
    }
}
