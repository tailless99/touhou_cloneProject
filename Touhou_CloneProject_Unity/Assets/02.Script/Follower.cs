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
        // �����̴� ���� ĸ��ȭ
        Watch();
        Follow();

        // �Ϲ� ����
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

        GameObject bullet = objectManager.MakeObj("bulletFollower"); // �ν��Ͻ��� ����
        bullet.transform.position = transform.position;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShotDelay = 0;
    }

    // ���Ӱ����� ���� ����Ű�� ���ȴ����� üũ��
    public void OnFire_TypeA(bool shotAndStop)
    {
        if (shotAndStop) FireBtnPress = true;
        else FireBtnPress = false;
    }

    // ���� �����°� ���� üũ�Ͽ�, ������ �� �ִٸ� �����δ�.
    public void Follow()
    {
        transform.position = followPos;
    }
}
