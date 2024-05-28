using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameTitle : MonoBehaviour
{
    [SerializeField] TMP_Text Title;
    [SerializeField] TMP_Text StartText;
    [SerializeField] TMP_Text CreateText;
    [SerializeField] float TitleSpeed;

    Rigidbody2D TitleRigi;
    float TitleYPos;    // 목표 높이

    private void Awake()
    {
        TitleRigi = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        // 목표 위치
        TitleYPos = Title.transform.position.y;

        // 타이틀을 안보이도록 초기 위치 설정
        Vector2 tempPos = new Vector2(Title.transform.position.x, 0);
        Title.transform.position = tempPos;
    }

    private void Update()
    {
        // 타이틀이 목표한 위치까지 올라오지 못한 경우
        if (Title.transform.position.y < TitleYPos ? true : false)
        {   // 타이틀 올리기
            TitleRigi.velocity = Vector2.up * TitleSpeed;
        }
        // 타이틀이 목표한 위치까지 올라왔을 경우
        else
        {
            TitleRigi.velocity = Vector2.zero;

            // 어떤 키든 입력받을 시, 다음 씬으로 넘어가기
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
