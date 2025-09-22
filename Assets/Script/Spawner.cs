using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    [Header("Spawn Positions")]
    public float spawnX = 20f;      // 오른쪽에서 등장
    public float y = 0.5f;          // 프리팹 높이
    public float[] lanesZ = new float[] { -4f, 0f, 4f }; // 3레인

    [Header("What to spawn")]
    [Range(0f, 1f)] public float coinChance = 0.35f; // 코인 나올 확률

    [Header("Timing")]
    public float startInterval = 1.0f;     // 시작 간격(초)
    public float minInterval = 0.25f;      // 최소 간격 바닥
    public float intervalDecayPerSec = 0.02f; // 초당 간격 감소량

    float timer;
    float nextTime;

    void Start()
    {
        timer = 0f;
        nextTime = Time.time + startInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 시간이 지날수록 간격 줄이기
        float curInterval = Mathf.Max(minInterval, startInterval - intervalDecayPerSec * timer);

        if (Time.time >= nextTime)
        {
            SpawnOne();
            nextTime = Time.time + curInterval;
        }
    }

    void SpawnOne()
    {
        // 레인 랜덤
        float z = lanesZ[Random.Range(0, lanesZ.Length)];
        Vector3 pos = new Vector3(spawnX, y, z);

        // 코인/장애물 선택
        GameObject prefab = (Random.value < coinChance) ? coinPrefab : obstaclePrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Spawner: prefab이 비어 있어요.");
            return;
        }

        // 생성 (현재 단계는 Instantiate만 사용)
        Instantiate(prefab, pos, Quaternion.identity);
    }
}