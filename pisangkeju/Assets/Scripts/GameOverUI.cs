using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;   // Buat load ulang scene
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Referensi Gameplay")]
    [SerializeField] private PlayerHealth playerHealth;   // Drag Player
    [SerializeField] private PlayerXP playerXP;           // Drag Player (buat ambil level akhir)
    [SerializeField] private GameTimer gameTimer;         // Drag UIManager (yang punya GameTimer)

    [Header("Referensi UI")]
    [SerializeField] private GameObject panel;            // Drag GameOverPanel
    [SerializeField] private TMP_Text summaryText;        // Drag SummaryText
    [SerializeField] private Button restartButton;        // Drag RestartButton

    private void Start()
    {
        Time.timeScale = 1f;                              // Pastikan game jalan normal (penting setelah restart)
        panel.SetActive(false);                           // Sembunyikan di awal

        restartButton.onClick.AddListener(Restart);       // Tombol Restart memanggil fungsi Restart
        playerHealth.OnDeath += ShowGameOver;             // Berlangganan event mati
    }

    private void OnDestroy()
    {
        if (playerHealth != null) playerHealth.OnDeath -= ShowGameOver;
    }

    private void ShowGameOver()
    {
        // Ringkasan permainan. "\n" = baris baru
        summaryText.text = $"Bertahan: {GameTimer.Format(gameTimer.Elapsed)}\nLevel: {playerXP.Level}";

        panel.SetActive(true);                            // Tampilkan panel Game Over
        Time.timeScale = 0f;                              // Bekukan game
    }

    private void Restart()
    {
        Time.timeScale = 1f;                              // WAJIB dinormalkan dulu, kalau ga game baru malah freeze
        // Muat ulang scene yang sedang aktif (semua kembali ke kondisi awal)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}