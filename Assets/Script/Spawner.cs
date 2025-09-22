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
    public float intervalDecayPerSec = 0.02f; // �ʴ� ���� ���ҷ�

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

        // �ð��� �������� ���� ���̱�
        float curInterval = Mathf.Max(minInterval, startInterval - intervalDecayPerSec * timer);

        if (Time.time >= nextTime)
        {
            SpawnOne();
            nextTime = Time.time + curInterval;
        }
    }

    void SpawnOne()
    {
        // ���� ����
        float z = lanesZ[Random.Range(0, lanesZ.Length)];
        Vector3 pos = new Vector3(spawnX, y, z);

        // ����/��ֹ� ����
        GameObject prefab = (Random.value < coinChance) ? coinPrefab : obstaclePrefab;
        if (prefab == null)
        {
            Debug.LogWarning("Spawner: prefab�� ��� �־��.");
            return;
        }

        // ���� (���� �ܰ�� Instantiate�� ���)
        Instantiate(prefab, pos, Quaternion.identity);
    }
}