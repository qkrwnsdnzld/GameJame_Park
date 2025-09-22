using UnityEngine;

public class Mover : MonoBehaviour
{
    public float baseSpeed = 8f;     // �̵� �ӵ�
    public float destroyX = -30f;    // �� x���� �����̸� ����

    [Header("Pass Score")]
    public float passLineX = -20f;   // �÷��̾� X�� �����ϰ�! (�⺻ -10)
    bool passScored = false;         // �� ���� ���� �ֱ�

    void Update()
    {
        transform.position += Vector3.left * baseSpeed * Time.deltaTime;

        // "��ֹ�"�̰�, ���� ���� �� ���, �÷��̾� X(=passLineX)�� �������� �Ѿ�� +10
        if (!passScored && CompareTag("Obstacle") && transform.position.x < passLineX)
        {
            passScored = true;
            var sm = FindObjectOfType<ScoreManager>();
            if (sm) sm.AddPass(); // �⺻ +10
        }

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }
}