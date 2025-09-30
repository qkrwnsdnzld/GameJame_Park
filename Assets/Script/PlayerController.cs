using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // 새 Input System 사용 시
#endif

/// <summary>
/// W/S로 3레인 이동, 코인 먹으면 점수 +100, 장애물 충돌 시 게임오버 패널 표시 + 일시정지
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs (인스펙터에서 연결)")]
    [SerializeField] private ScoreManager score; // ScoreSystem 오브젝트의 ScoreManager 드래그
    [SerializeField] private UIManager ui;       // Canvas 오브젝트(UIManager) 드래그

    [Header("Lane Settings")]
    public float playerX = -10f;                       // 플레이어 X 고정
    public float[] lanesZ = new float[] { -4f, 0f, 4f }; // 3개 레인 Z 좌표
    public float laneLerpSpeed = 12f;                  // 레인 이동 보간 속도

    private int laneIndex = 1; // 중앙 레인(0:-4, 1:0, 2:+4)
    private float targetZ;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 키네마틱 RB (트리거 충돌용)
    }

    void Start()
    {
        // 시작 위치 정렬
        laneIndex = Mathf.Clamp(laneIndex, 0, lanesZ.Length - 1);
        targetZ = lanesZ[laneIndex];
        var p = transform.position;
        transform.position = new Vector3(playerX, p.y, targetZ);
    }

    [Header("Speed Up")]
    public float speedUpPerSec = 2f; // 초당 속도 증가량
    public float maxLaneLerpSpeed = 30f; // 최대 속도 제한

    void Update()
    {
        // 입력 처리 (Both 설정이면 둘 다 컴파일됨)
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.wKey.wasPressedThisFrame) MoveLane(+1);
        if (Keyboard.current.sKey.wasPressedThisFrame) MoveLane(-1);
#else
        if (Input.GetKeyDown(KeyCode.W)) MoveLane(+1);
        if (Input.GetKeyDown(KeyCode.S)) MoveLane(-1);
#endif
        // 부드러운 레인 이동
        var p = transform.position;
        float z = Mathf.Lerp(p.z, targetZ, Time.deltaTime * laneLerpSpeed);
        transform.position = new Vector3(playerX, p.y, z);

        laneLerpSpeed = Mathf.Min(laneLerpSpeed + speedUpPerSec * Time.deltaTime, maxLaneLerpSpeed);
    }

    private void MoveLane(int delta)
    {
        laneIndex = Mathf.Clamp(laneIndex + delta, 0, lanesZ.Length - 1);
        targetZ = lanesZ[laneIndex];
    }

    public void ResetToCenterLane()
    {
        laneIndex = 1;
        targetZ = lanesZ[laneIndex];
        var p = transform.position;
        transform.position = new Vector3(playerX, p.y, targetZ);
    }

    // 충돌 처리: Coin = 점수 + 제거, Obstacle = 게임오버
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (score) score.AddCoin();       // 점수 +100
            Destroy(other.gameObject);        // 코인 제거
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            Time.timeScale = 0f;              // 일시정지
            if (ui) ui.ShowGameOver(true);    // 게임오버 패널 표시
        }
    }
}