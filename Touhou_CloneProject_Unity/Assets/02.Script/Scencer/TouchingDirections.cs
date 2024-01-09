using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    // 감지 거리
    [SerializeField] public float rayDistance = 0.2f;

    private bool _IsTouchingTop;
    private bool _IsTouchingDown;
    private bool _IsTouchingRight;
    private bool _IsTouchingLeft;
    public LayerMask targetLayer;

    public bool IsTouchingTop
    {  get { return _IsTouchingTop; }
       set { _IsTouchingTop = value; } 
    }

    public bool IsTouchingDown
    {
        get { return _IsTouchingDown; }
        set { _IsTouchingDown = value; }
    }

    public bool IsTouchingRight
    {
        get { return _IsTouchingRight; }
        set { _IsTouchingRight = value; }
    }

    public bool IsTouchingLeft
    {
        get { return _IsTouchingLeft; }
        set { _IsTouchingLeft = value; }
    }

    BoxCollider2D boxCollider;
    public ContactFilter2D castFilter;
    RaycastHit2D[] topHits = new RaycastHit2D[3];
    RaycastHit2D[] downHits = new RaycastHit2D[3];
    RaycastHit2D[] rightHits = new RaycastHit2D[3];
    RaycastHit2D[] leftHits = new RaycastHit2D[3];

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        
        // targetLayer와 충돌하도록 필터링 설정
        castFilter = new ContactFilter2D();
        castFilter.useLayerMask = true;
        castFilter.SetLayerMask(targetLayer);
    }

    private void FixedUpdate()
    {
        IsTouchingTop = boxCollider.Cast(Vector2.up, castFilter, topHits, rayDistance) > 0 ? true : false;
        IsTouchingDown = boxCollider.Cast(Vector2.down, castFilter, downHits, rayDistance) > 0 ? true : false;
        IsTouchingRight = boxCollider.Cast(Vector2.right, castFilter, rightHits, rayDistance) > 0 ? true : false;
        IsTouchingLeft = boxCollider.Cast(Vector2.left, castFilter, leftHits, rayDistance) > 0 ? true : false;
    }
}
