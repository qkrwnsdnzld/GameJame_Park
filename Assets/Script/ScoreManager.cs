using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int Score { get; private set; }
    public int coinScore = 100; // 코인 1개 점수

    public int passScore = 10;                 // ← 추가: 무사 통과 점수
    public void AddPass(int amount = 0) => Set(Score + (amount > 0 ? amount : passScore));  // ← 추가

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