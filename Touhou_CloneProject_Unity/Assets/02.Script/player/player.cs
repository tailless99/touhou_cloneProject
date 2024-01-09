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
        // 매 프레임마다 포지션변경
        rb.velocity = new Vector2(moveVector2.x * moveSpeed, moveVector2.y * moveSpeed);
        Debug.DrawRay(transform.position, Vector2.left * touchingDirections.rayDistance, Color.red);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 백터값 임시 저장 변수
        Vector2 tempVector = context.ReadValue<Vector2>();

        if(touchingDirections.IsTouchingLeft) {
            // 왼쪽에 벽이 있을 경우, 왼쪽으로 가려는 값이 있다면 0으로 변경
            if(tempVector.x < 0) tempVector.x = 0;
            Debug.Log("닿았음!");
        }
        else if (touchingDirections.IsTouchingRight)
        {
            // 오른쪽에 벽이 있을 경우, 오른쪽으로 가려는 값이 있다면 0으로 변경
            if (tempVector.x > 0) tempVector.x = 0;
        }else if (touchingDirections.IsTouchingTop)
        {
            // 위쪽에 벽이 있을 경우, 위쪽으로 가려는 값이 있다면 0으로 변경
            if (tempVector.y > 0) tempVector.y = 0;
        }else if (touchingDirections.IsTouchingDown)
        {
            // 아래쪽에 벽이 있을 경우, 아래쪽으로 가려는 값이 있다면 0으로 변경
            if (tempVector.y < 0) tempVector.y = 0;
        }

        moveVector2 = tempVector;
        // 입력받은 키 값을 Vector2 객체에 대입하여 포지션(위치) 변경
        //moveVector2 = context.ReadValue<Vector2>();
        DirectionCheck(moveVector2);
    }

    // 좌우 이동 애니메이션을 위해 bool값 설정
    private void DirectionCheck(Vector2 moveVector)
    {
        bool rigntCheck = moveVector.x > 0 ? true : false;
        animator.SetBool(AnimationStrings.IsRight, rigntCheck);

        bool leftCheck = moveVector.x < 0 ? true : false;
        animator.SetBool(AnimationStrings.IsLeft, leftCheck);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // Border 경계에 충돌할 때는 멈추기
    //    if (collision.gameObject.tag == "Border")
    //    {
    //        moveVector2 = Vector2.zero;
    //    }
    //}
}
