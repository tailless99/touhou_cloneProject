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
    float TitleYPos;    // ��ǥ ����

    private void Awake()
    {
        TitleRigi = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start()
    {
        // ��ǥ ��ġ
        TitleYPos = Title.transform.position.y;

        // Ÿ��Ʋ�� �Ⱥ��̵��� �ʱ� ��ġ ����
        Vector2 tempPos = new Vector2(Title.transform.position.x, 0);
        Title.transform.position = tempPos;
    }

    private void Update()
    {
        // Ÿ��Ʋ�� ��ǥ�� ��ġ���� �ö���� ���� ���
        if (Title.transform.position.y < TitleYPos ? true : false)
        {   // Ÿ��Ʋ �ø���
            TitleRigi.velocity = Vector2.up * TitleSpeed;
        }
        // Ÿ��Ʋ�� ��ǥ�� ��ġ���� �ö���� ���
        else
        {
            TitleRigi.velocity = Vector2.zero;

            // � Ű�� �Է¹��� ��, ���� ������ �Ѿ��
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
