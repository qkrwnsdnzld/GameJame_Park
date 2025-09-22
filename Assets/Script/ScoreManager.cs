using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int Score { get; private set; }
    public int coinScore = 100; // ���� 1�� ����

    public int passScore = 10;                 // �� �߰�: ���� ��� ����
    public void AddPass(int amount = 0) => Set(Score + (amount > 0 ? amount : passScore));  // �� �߰�

    void Start()
    {
        Set(0);
    }

    public void AddCoin()
    {
        Set(Score + coinScore);
    }

    void Set(int v)
    {
        Score = v;
        if (scoreText) scoreText.text = $"Score: {Score}";
    }
}