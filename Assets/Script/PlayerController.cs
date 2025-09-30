using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // �� Input System ��� ��
#endif

/// <summary>
/// W/S�� 3���� �̵�, ���� ������ ���� +100, ��ֹ� �浹 �� ���ӿ��� �г� ǥ�� + �Ͻ�����
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs (�ν����Ϳ��� ����)")]
    [SerializeField] private ScoreManager score; // ScoreSystem ������Ʈ�� ScoreManager �巡��
    [SerializeField] private UIManager ui;       // Canvas ������Ʈ(UIManager) �巡��

    [Header("Lane Settings")]
    public float playerX = -10f;                       // �÷��̾� X ����
    public float[] lanesZ = new float[] { -4f, 0f, 4f }; // 3�� ���� Z ��ǥ
    public float laneLerpSpeed = 12f;                  // ���� �̵� ���� �ӵ�

    private int laneIndex = 1; // �߾� ����(0:-4, 1:0, 2:+4)
    private float targetZ;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ű�׸�ƽ RB (Ʈ���� �浹��)
    }

    void Start()
    {
        // ���� ��ġ ����
        laneIndex = Mathf.Clamp(laneIndex, 0, lanesZ.Length - 1);
        targetZ = lanesZ[laneIndex];
        var p = transform.position;
        transform.position = new Vector3(playerX, p.y, targetZ);
    }

    [Header("Speed Up")]
    public float speedUpPerSec = 2f; // �ʴ� �ӵ� ������
    public float maxLaneLerpSpeed = 30f; // �ִ� �ӵ� ����

    void Update()
    {
        // �Է� ó�� (Both �����̸� �� �� �����ϵ�)
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.wKey.wasPressedThisFrame) MoveLane(+1);
        if (Keyboard.current.sKey.wasPressedThisFrame) MoveLane(-1);
#else
        if (Input.GetKeyDown(KeyCode.W)) MoveLane(+1);
        if (Input.GetKeyDown(KeyCode.S)) MoveLane(-1);
#endif
        // �ε巯�� ���� �̵�
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

    // �浹 ó��: Coin = ���� + ����, Obstacle = ���ӿ���
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (score) score.AddCoin();       // ���� +100
            Destroy(other.gameObject);        // ���� ����
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            Time.timeScale = 0f;              // �Ͻ�����
            if (ui) ui.ShowGameOver(true);    // ���ӿ��� �г� ǥ��
        }
    }
}