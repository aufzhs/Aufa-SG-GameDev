using System.Collections.Generic;   // Buat List
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{
    [Header("Referensi Gameplay")]
    [SerializeField] private GameObject player;          // Drag Player (punya PlayerXP, Weapon, Controller, Health)

    [Header("Referensi UI")]
    [SerializeField] private GameObject panel;           // Drag LevelUpPanel
    [SerializeField] private Button[] cardButtons;       // 3 tombol kartu (Card1, Card2, Card3)
    [SerializeField] private TMP_Text[] titleTexts;      // 3 judul kartu (urutannya sama dengan tombol)
    [SerializeField] private TMP_Text[] descTexts;       // 3 deskripsi kartu

    // Daftar jenis upgrade yang ada
    private enum UpgradeType { Damage, FireRate, MoveSpeed, MaxHealth }

    // Data 1 upgrade: jenis, judul, dan deskripsi yang tampil di kartu
    private struct Upgrade
    {
        public UpgradeType type;
        public string title;
        public string description;
    }

    // Semua upgrade yang mungkin muncul (nanti diambil 3 secara acak)
    private readonly List<Upgrade> allUpgrades = new List<Upgrade>
    {
        new Upgrade { type = UpgradeType.Damage,    title = "Damage +1",     description = "Peluru lebih sakit" },
        new Upgrade { type = UpgradeType.FireRate,  title = "Fire Rate +20%", description = "Nembak lebih cepat" },
        new Upgrade { type = UpgradeType.MoveSpeed, title = "Speed +10%",    description = "Jalan lebih cepat" },
        new Upgrade { type = UpgradeType.MaxHealth, title = "Max HP +20",    description = "HP maksimal naik dan langsung terisi" },
    };

    private PlayerXP playerXP;
    private PlayerHealth playerHealth;
    private PlayerWeapon weapon;
    private PlayerController controller;

    private int pendingLevelUps;    // Berapa kali level up yang belum dipilih upgrade-nya
    private bool isOpen;            // true kalau menu sedang tampil

    private void Start()
    {
        Time.timeScale = 1f;                                  // Jaga-jaga: pastikan game jalan normal saat mulai

        playerXP = player.GetComponent<PlayerXP>();           // Ambil semua script player yang dibutuhin
        playerHealth = player.GetComponent<PlayerHealth>();
        weapon = player.GetComponent<PlayerWeapon>();
        controller = player.GetComponent<PlayerController>();

        panel.SetActive(false);                               // Menu tersembunyi di awal
        playerXP.OnLevelUp += OnLevelUp;                      // Berlangganan event level up
    }

    private void OnDestroy()
    {
        if (playerXP != null) playerXP.OnLevelUp -= OnLevelUp;   // Berhenti berlangganan
    }

    // Dipanggil otomatis tiap player naik level
    private void OnLevelUp(int newLevel)
    {
        if (playerHealth.IsDead) return;      // Player sudah mati, ga perlu menu

        pendingLevelUps++;                    // Catat: ada 1 pilihan upgrade yang menunggu
        if (!isOpen) ShowMenu();              // Kalau menu belum tampil, tampilkan sekarang
    }

    private void ShowMenu()
    {
        isOpen = true;
        Time.timeScale = 0f;                  // PAUSE game (gerak, tembak, spawn semuanya berhenti)
        panel.SetActive(true);                // Tampilkan panel

        List<Upgrade> choices = PickRandomUpgrades(cardButtons.Length);   // Ambil 3 upgrade acak

        for (int i = 0; i < cardButtons.Length; i++)
        {
            Upgrade up = choices[i];                          // Upgrade untuk kartu ke-i
            titleTexts[i].text = up.title;                    // Isi judul kartu
            descTexts[i].text = up.description;               // Isi deskripsi kartu

            cardButtons[i].onClick.RemoveAllListeners();      // Hapus aksi klik lama (biar ga numpuk)
            cardButtons[i].onClick.AddListener(() => OnCardChosen(up));   // Aksi baru: pilih upgrade ini
        }
    }

    // Ambil 'count' upgrade acak yang berbeda-beda
    private List<Upgrade> PickRandomUpgrades(int count)
    {
        List<Upgrade> pool = new List<Upgrade>(allUpgrades);  // Salinan daftar (biar daftar asli ga rusak)

        // Acak urutan (Fisher-Yates shuffle): tukar tiap elemen dengan elemen acak sebelum/di posisinya
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);                   // Indeks acak 0 sampai i
            Upgrade temp = pool[i];
            pool[i] = pool[j];
            pool[j] = temp;
        }

        return pool.GetRange(0, count);                       // Ambil 'count' pertama dari daftar yang sudah diacak
    }

    private void OnCardChosen(Upgrade up)
    {
        ApplyUpgrade(up);                     // Terapkan efek upgrade
        pendingLevelUps--;                    // Satu pilihan sudah selesai

        if (pendingLevelUps > 0)
        {
            ShowMenu();                       // Masih ada level up tertunda, tampilkan pilihan lagi
        }
        else
        {
            panel.SetActive(false);           // Tutup menu
            isOpen = false;
            Time.timeScale = 1f;              // LANJUTKAN game
        }
    }

    private void ApplyUpgrade(Upgrade up)
    {
        switch (up.type)                      // Pilih aksi sesuai jenis upgrade
        {
            case UpgradeType.Damage:
                weapon.UpgradeDamage(1);              // Damage +1
                break;
            case UpgradeType.FireRate:
                weapon.UpgradeFireRate(0.2f);         // Fire rate +20%
                break;
            case UpgradeType.MoveSpeed:
                controller.UpgradeMoveSpeed(0.1f);    // Speed +10%
                break;
            case UpgradeType.MaxHealth:
                playerHealth.IncreaseMaxHealth(20);   // Max HP +20
                break;
        }
    }
}