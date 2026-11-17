using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;    // Drag TimerText

    // Waktu bertahan (detik). Bisa dibaca script lain (Game Over), tapi cuma diubah di sini
    public float Elapsed { get; private set; }

    private void Update()
    {
        // Time.deltaTime = 0 saat game di-pause (timeScale 0), jadi timer otomatis berhenti
        Elapsed += Time.deltaTime;
        timerText.text = Format(Elapsed);           // Tampilkan dengan format menit:detik
    }

    // 'static' = bisa dipanggil tanpa objek: GameTimer.Format(75f) menghasilkan "01:15"
    public static string Format(float seconds)
    {
        int total = Mathf.FloorToInt(seconds);      // Buang desimal
        int minutes = total / 60;                   // Berapa menit penuh
        int secs = total % 60;                      // Sisa detiknya
        return $"{minutes:00}:{secs:00}";           // ":00" = selalu 2 digit (5 jadi 05)
    }
}