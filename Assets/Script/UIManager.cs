using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class UIManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public Button retryButton;

    [Header("Audio (optional)")]
    public AudioSource bgm;        // ← BGM 오브젝트의 AudioSource 드래그
    public float fadeTime = 0.4f;  // 페이드 시간

    bool waitingForRestart;

    void Awake()
    {
        if (retryButton) retryButton.onClick.AddListener(OnRetry);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (waitingForRestart &&
            ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
             (Mouse.current != null && (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)) ||
             (Gamepad.current != null && (Gamepad.current.startButton.wasPressedThisFrame || Gamepad.current.aButton.wasPressedThisFrame))))
        {
            OnRetry();
        }
#else
        if (waitingForRestart && Input.anyKeyDown) OnRetry();
#endif
    }

    public void ShowGameOver(bool on)
    {
        gameOverPanel?.SetActive(on);
        waitingForRestart = on;
        if (bgm) StopAllCoroutines();
        if (bgm && on) StartCoroutine(FadeVolume(bgm, 0f, fadeTime)); // 페이드아웃
    }

    System.Collections.IEnumerator FadeVolume(AudioSource src, float to, float dur)
    {
        float from = src.volume;
        float t = 0f;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;           // timeScale=0에서도 진행
            src.volume = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        src.volume = to;
    }

    void OnRetry()
    {
        Time.timeScale = 1f;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}