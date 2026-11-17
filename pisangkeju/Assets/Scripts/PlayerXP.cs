using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int xpToNextLevel = 5;          // XP yang dibutuhin buat level 2
    [SerializeField] private float xpGrowth = 1.5f;          // Kebutuhan XP naik 1.5x tiap level

    public int Level { get; private set; } = 1;             // Level sekarang
    public int CurrentXP { get; private set; }              // XP sekarang di level ini
    public int XPToNextLevel => xpToNextLevel;              // Biar UI bisa baca

    public event Action<int, int> OnXPChanged;              // Kirim (XP sekarang, XP dibutuhin)
    public event Action<int> OnLevelUp;                     // Kirim level baru

    public void AddXP(int amount)
    {
        CurrentXP += amount;                                // Tambah XP

        // 'while' (bukan 'if') karena bisa naik beberapa level sekaligus kalau XP-nya banyak
        while (CurrentXP >= xpToNextLevel)
        {
            CurrentXP -= xpToNextLevel;                     // Sisa XP dibawa ke level berikutnya
            Level++;                                        // Naik level
            xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * xpGrowth);   // Kebutuhan XP naik
            Debug.Log($"LEVEL UP! Sekarang level {Level}");
            OnLevelUp?.Invoke(Level);                       // Umumin: menu level up yang akan merespons
        }

        OnXPChanged?.Invoke(CurrentXP, xpToNextLevel);      // Umumin buat bar XP
    }
}