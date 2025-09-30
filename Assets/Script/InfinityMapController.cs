using System.Collections.Generic;
using UnityEngine;

public class InfinityMapController : MonoBehaviour
{
    [Header("Prefabs (4�� �̻� ����)")]
    public List<MapSegment> segmentPrefabs;

    [Header("�ʱ� ���� ���� (ī�޶� �þ� + ������)")]
    public int initialSegments = 6;

    [Header("��ũ�� �ӵ� (m/s)")]
    public float scrollSpeed = 8f;

    [Header("�� ��(Z) �ڷ� ������ �Ѿ ���׸�Ʈ�� ��Ȱ��")]
    public float recycleLineX = -50f;

    // ���� ����
    private readonly Queue<MapSegment> _active = new Queue<MapSegment>();
    private System.Random _rng;

    void Awake()
    {
        _rng = new System.Random();
    }

    void Start()
    {
        if (segmentPrefabs == null || segmentPrefabs.Count == 0)
        {
            Debug.LogError("segmentPrefabs�� ��� ����!");
            enabled = false;
            return;
        }

        // ���� �� ����(+Z)���� �ʱ� ��ġ
        float spawnX = 0f;
        for (int i = 0; i < initialSegments; i++)
        {
            var seg = Spawn(RandomPrefab(), spawnX);
            _active.Enqueue(seg);
            spawnX += seg.Length;
        }
    }

    void Update()
    {
        // Track(�θ�)�� �ڷ� �̵� �� ���谡 �帣�� ȿ��
        transform.Translate(-scrollSpeed * Time.deltaTime, 0f, 0f, Space.World);

        TryRecycle();
    }

    private MapSegment Spawn(MapSegment prefab, float x)
    {
        var seg = Instantiate(prefab, new Vector3(x -20, -0.4f, 5f), Quaternion.identity, transform);
        seg.name = $"{prefab.name}_{_active.Count}";
        return seg;
    }

    private MapSegment RandomPrefab()
    {
        int i = _rng.Next(segmentPrefabs.Count);
        return segmentPrefabs[i];
    }

    private void TryRecycle()
    {
        if (_active.Count == 0) return;

        var first = _active.Peek();
        // first�� '��'�� ��Ȱ�뼱 �ڷ� �Ѿ�� ���ġ
        float endX = first.transform.position.x + first.Length;

        if (endX < recycleLineX)
        {
            // �� �� ���׸�Ʈ ã��
            MapSegment last = null;
            foreach (var s in _active) last = s; // Queue�� ������

            // Dequeue �� �� �ڷ� ����
            _active.Dequeue();
            float newX = last.transform.position.x + last.Length;

            // �ʿ� �� ������ �پ�ȭ: ���� ������Ʈ �״�� ����(�ʰ���)
            first.transform.position = new Vector3(newX, first.transform.position.y, 5f);

            // (����) ���/������Ʈ ���¿� ���� ������ ���⼭ ȣ��
            // var refreshable = first.GetComponent<ISegmentRefresh>();
            // refreshable?.Refresh(_rng);

            _active.Enqueue(first);
        }
    }
}