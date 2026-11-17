using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private PlayerXP playerXP;       // Drag Player ke sini
    [SerializeField] private Slider slider;           // Drag XPBar
    [SerializeField] private TMP_Text levelText;      // Drag LevelText

    private void Start()
    {
        playerXP.OnXPChanged += UpdateBar;            // Berlangganan perubahan XP
        UpdateBar(playerXP.CurrentXP, playerXP.XPToNextLevel);   // Tampilan awal
    }

    private void OnDestroy()
    {
        if (playerXP != null) playerXP.OnXPChanged -= UpdateBar; // Berhenti berlangganan
    }

    private void UpdateBar(int currentXP, int xpNeeded)
    {
        slider.value = (float)currentXP / xpNeeded;   // Persentase menuju level berikutnya
        levelText.text = $"Lv {playerXP.Level}";      // Update teks level
    }
}