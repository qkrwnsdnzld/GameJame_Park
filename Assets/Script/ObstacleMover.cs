using UnityEngine;

public class Mover : MonoBehaviour
{
    public float baseSpeed = 8f;     // 이동 속도
    public float destroyX = -30f;    // 이 x보다 왼쪽이면 제거

    [Header("Pass Score")]
    public float passLineX = -20f;   // 플레이어 X와 동일하게! (기본 -10)
    bool passScored = false;         // 한 번만 점수 주기

    void Update()
    {
        transform.position += Vector3.left * baseSpeed * Time.deltaTime;

        // "장애물"이고, 아직 점수 안 줬고, 플레이어 X(=passLineX)를 왼쪽으로 넘어가면 +10
        if (!passScored && CompareTag("Obstacle") && transform.position.x < passLineX)
        {
            passScored = true;
            var sm = FindObjectOfType<ScoreManager>();
            if (sm) sm.AddPass(); // 기본 +10
        }

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}