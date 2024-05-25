using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Follower : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] public float maxShotDelay;
    [SerializeField] public float curShotDelay;
    [SerializeField] public float bulletPower;
    [SerializeField] public ObjectManager objectManager;
    [SerializeField] Vector3 followPos;
    [SerializeField] Transform parent;

    bool FireBtnPress;    
    public Queue<Vector3> parentPos;
    public int followDelay;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }

    private void FixedUpdate()
    {
        // 움직이는 로직 캡슐화
        Watch();
        Follow();

        // 일반 공격
        if (FireBtnPress)
        {
            OnFire_A();
            Reload();
        }
    }

    private void Watch()
    {
        // input position
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        // output position
        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnFire_A()
    {
        if (curShotDelay < maxShotDelay) return;

        GameObject bullet = objectManager.MakeObj("bulletFollower"); // 인스턴스로 생성
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    // 연속공격을 위해 공격키가 눌렸는지를 체크함
    public void OnFire_TypeA(bool shotAndStop)
    {
        if (shotAndStop) FireBtnPress = true;
        else FireBtnPress = false;
    }

    // 벽에 막혔는가 등을 체크하여, 움직일 수 있다면 움직인다.
    public void Follow()
    {
        transform.position = followPos;
    }
}
