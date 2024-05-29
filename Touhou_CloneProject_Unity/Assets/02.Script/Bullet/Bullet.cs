using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    [SerializeField] public bool isRotate;

    private void Update()
    {
        if (isRotate) transform.Rotate(Vector3.forward * 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet") {
            this.gameObject.SetActive(false);
        }        
    }
}
