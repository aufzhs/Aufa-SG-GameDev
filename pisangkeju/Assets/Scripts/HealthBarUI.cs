using TMPro;               // Buat TMP_Text (TextMeshPro)
using UnityEngine;
using UnityEngine.UI;      // Buat Slider

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;   // Drag Player ke sini (yang punya PlayerHealth)
    [SerializeField] private Slider slider;               // Drag HealthBar ke sini
    [SerializeField] private TMP_Text hpText;             // Opsional: teks angka HP, boleh dikosongin

    private void Start()
    {
        // Berlangganan: tiap HP berubah, fungsi UpdateBar dipanggil otomatis
        playerHealth.OnHealthChanged += UpdateBar;

        // Tampilkan kondisi awal (HP penuh)
        UpdateBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void OnDestroy()
    {
        // Berhenti berlangganan biar ga ada error pas object dihapus (misal saat restart)
        if (playerHealth != null) playerHealth.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        slider.value = (float)current / max;              // Slider 0 sampai 1, jadi dibagi HP maksimal (di-cast float biar ga dibulatkan)

        if (hpText != null)                               // Cek dulu, teks ini opsional
            hpText.text = $"{current}/{max}";             // Contoh: "80/100"
    }
}