using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;

    [Header("Spawn Positions")]
    public float spawnX = 20f;      // �����ʿ��� ����
    public float y = 0.5f;          // ������ ����
    public float[] lanesZ = new float[] { -4f, 0f, 4f }; // 3����

    [Header("What to spawn")]
    [Range(0f, 1f)] public float coinChance = 0.35f; // ���� ���� Ȯ��

    [Header("Timing")]
    public float startInterval = 1.0f;     // ���� ����(��)
    public float minInterval = 0.25f;      // �ּ� ���� �ٴ�
    public float intervalDecayPerSec = 0.0001f; // �ʴ� ���� ���ҷ�

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

        // ���� ����
        float curInterval = startInterval;

        if (Time.time >= nextTime)
        {
            SpawnOne();
            nextTime = Time.time + curInterval;
        }
    }

    int lastEmptyLane = -1; // ������ ����� ���� �ε��� ����

    void SpawnOne()
    {
        float doubleObstacleChance = 0.2f;

        if (Random.value < doubleObstacleChance)
        {
            int emptyLane;
            do
            {
                emptyLane = Random.Range(0, lanesZ.Length);
            } while (emptyLane == lastEmptyLane); // ������ ���� ���� ����

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
            } while (lane == lastEmptyLane); // ������ ���� ���� ����

            lastEmptyLane = lane;

            Vector3 pos = new Vector3(spawnX, y, lanesZ[lane]);
            GameObject prefab = (Random.value < coinChance) ? coinPrefab : obstaclePrefab;
            if (prefab == null)
            {
                Debug.LogWarning("Spawner: prefab�� ��� �־��.");
                return;
            }
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}