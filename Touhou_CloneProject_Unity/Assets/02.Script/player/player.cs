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
    TouchingDirections touchingDirections;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        // �� �����Ӹ��� �����Ǻ���
        rb.velocity = new Vector2(moveVector2.x * moveSpeed, moveVector2.y * moveSpeed);
        Debug.DrawRay(transform.position, Vector2.left * touchingDirections.rayDistance, Color.red);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // ���Ͱ� �ӽ� ���� ����
        Vector2 tempVector = context.ReadValue<Vector2>();

        if(touchingDirections.IsTouchingLeft) {
            // ���ʿ� ���� ���� ���, �������� ������ ���� �ִٸ� 0���� ����
            if(tempVector.x < 0) tempVector.x = 0;
            Debug.Log("�����!");
        }
        else if (touchingDirections.IsTouchingRight)
        {
            // �����ʿ� ���� ���� ���, ���������� ������ ���� �ִٸ� 0���� ����
            if (tempVector.x > 0) tempVector.x = 0;
        }else if (touchingDirections.IsTouchingTop)
        {
            // ���ʿ� ���� ���� ���, �������� ������ ���� �ִٸ� 0���� ����
            if (tempVector.y > 0) tempVector.y = 0;
        }else if (touchingDirections.IsTouchingDown)
        {
            // �Ʒ��ʿ� ���� ���� ���, �Ʒ������� ������ ���� �ִٸ� 0���� ����
            if (tempVector.y < 0) tempVector.y = 0;
        }

        moveVector2 = tempVector;
        // �Է¹��� Ű ���� Vector2 ��ü�� �����Ͽ� ������(��ġ) ����
        //moveVector2 = context.ReadValue<Vector2>();
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // Border ��迡 �浹�� ���� ���߱�
    //    if (collision.gameObject.tag == "Border")
    //    {
    //        moveVector2 = Vector2.zero;
    //    }
    //}
}
