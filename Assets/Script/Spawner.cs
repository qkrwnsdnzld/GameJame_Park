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
    public float intervalDecayPerSec = 0.0001f; // 초당 간격 감소량

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

        // 간격 고정
        float curInterval = startInterval;

        if (Time.time >= nextTime)
        {
            SpawnOne();
            nextTime = Time.time + curInterval;
        }
    }

    int lastEmptyLane = -1; // 이전에 비워둔 레인 인덱스 저장

    void SpawnOne()
    {
        float doubleObstacleChance = 0.2f;

        if (Random.value < doubleObstacleChance)
        {
            int emptyLane;
            do
            {
                emptyLane = Random.Range(0, lanesZ.Length);
            } while (emptyLane == lastEmptyLane); // 이전과 같은 패턴 방지

            lastEmptyLane = emptyLane;

            for (int i = 0; i < lanesZ.Length; i++)
            {
                if (i == emptyLane) continue;
                Vector3 pos = new Vector3(spawnX, y, lanesZ[i]);
                Instantiate(obstaclePrefab, pos, Quaternion.identity);
            }
        }
        else
        {
            int lane;
            do
            {
                lane = Random.Range(0, lanesZ.Length);
            } while (lane == lastEmptyLane); // 이전과 같은 패턴 방지

            lastEmptyLane = lane;

            Vector3 pos = new Vector3(spawnX, y, lanesZ[lane]);
            GameObject prefab = (Random.value < coinChance) ? coinPrefab : obstaclePrefab;
            if (prefab == null)
            {
                Debug.LogWarning("Spawner: prefab이 비어 있어요.");
                return;
            }
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}