using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;       // Drag PausePanel
    [SerializeField] private GameObject levelUpPanel;     // Drag LevelUpPanel (biar ga pause saat menu level up)
    [SerializeField] private GameObject gameOverPanel;    // Drag GameOverPanel
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;

    private bool isPaused;                                // true kalau sedang di-pause

    private void Start()
    {
        pausePanel.SetActive(false);                      // Sembunyikan di awal
        resumeButton.onClick.AddListener(Resume);         // Tombol Resume
        restartButton.onClick.AddListener(Restart);       // Tombol Restart
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;    // Cuma peduli saat Esc ditekan

        // Jangan ganggu kalau menu level up atau game over sedang tampil
        if (levelUpPanel.activeSelf || gameOverPanel.activeSelf) return;

        if (isPaused) Resume();                           // Sedang pause? lanjutkan
        else Pause();                                     // Sedang jalan? pause
    }

    private void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;                              // Bekukan game
    }

    private void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;                              // Lanjutkan game
    }

    private void Restart()
    {
        Time.timeScale = 1f;                              // Normalkan dulu sebelum load
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}