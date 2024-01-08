using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float grazeSpeed = 3f;
    bool verticalBorder;
    bool horizontalBorder;

    Vector2 moveVector2 = Vector2.zero;
    Rigidbody2D rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // �� �����Ӹ��� �����Ǻ���
        rb.velocity = new Vector2(moveVector2.x * moveSpeed, moveVector2.y * moveSpeed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // �Է¹��� Ű ���� Vector2 ��ü�� �����Ͽ� ������(��ġ) ����
        moveVector2 = context.ReadValue<Vector2>();
        DirectionCheck(moveVector2);
    }

    // �¿� �̵� �ִϸ��̼��� ���� bool�� ����
    private void DirectionCheck(Vector2 moveVector)
    {
        bool rigntCheck = moveVector.x > 0 ? true : false;
        animator.SetBool(AnimationStrings.IsRight, rigntCheck);

        bool leftCheck = moveVector.x < 0 ? true : false;
        animator.SetBool(AnimationStrings.IsLeft, leftCheck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Border ��迡 �浹�� ���� ���߱�
        if (collision.gameObject.tag == "Border")
        {
            moveVector2 = Vector2.zero;
        }
    }
}
