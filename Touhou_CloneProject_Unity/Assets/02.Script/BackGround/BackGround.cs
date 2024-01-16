using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public int startIndex;
    [SerializeField] public int endIndex;
    [SerializeField] public Transform[] sprites;

    float viewHeight;

    private void Awake()
    {   // Camera.main.orthographicSize 는 중심점부터 시작되는 거리, 즉 반지름이다
        viewHeight = Camera.main.orthographicSize * 2;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.y < viewHeight*(-1))
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;

            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave-1) == -1 ? sprites.Length-1 : startIndexSave-1;
        }
    }
}
