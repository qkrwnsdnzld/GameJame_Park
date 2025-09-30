using System.Collections.Generic;
using UnityEngine;

public class InfinityMapController : MonoBehaviour
{
    [Header("Prefabs (4개 이상 가능)")]
    public List<MapSegment> segmentPrefabs;

    [Header("초기 생성 개수 (카메라 시야 + 여유분)")]
    public int initialSegments = 6;

    [Header("스크롤 속도 (m/s)")]
    public float scrollSpeed = 8f;

    [Header("이 선(Z) 뒤로 완전히 넘어간 세그먼트는 재활용")]
    public float recycleLineX = -50f;

    // 내부 상태
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
            Debug.LogError("segmentPrefabs가 비어 있음!");
            enabled = false;
            return;
        }

        // 시작 시 앞쪽(+Z)으로 초기 배치
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
        // Track(부모)을 뒤로 이동 → 세계가 흐르는 효과
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
        // first의 '끝'이 재활용선 뒤로 넘어가면 재배치
        float endX = first.transform.position.x + first.Length;

        if (endX < recycleLineX)
        {
            // 맨 뒤 세그먼트 찾기
            MapSegment last = null;
            foreach (var s in _active) last = s; // Queue의 마지막

            // Dequeue → 맨 뒤로 보냄
            _active.Dequeue();
            float newX = last.transform.position.x + last.Length;

            // 필요 시 프리팹 다양화: 기존 오브젝트 그대로 재사용(초간단)
            first.transform.position = new Vector3(newX, first.transform.position.y, 5f);

            // (선택) 장식/오브젝트 리셋용 훅이 있으면 여기서 호출
            // var refreshable = first.GetComponent<ISegmentRefresh>();
            // refreshable?.Refresh(_rng);

            _active.Enqueue(first);
        }
    }
}