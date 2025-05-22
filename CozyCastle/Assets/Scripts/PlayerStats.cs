using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxMana = 200f;  // Set in Inspector
    private float currentMana;

    public delegate void OnManaChanged(float currentMana, float maxMana);
    public event OnManaChanged onManaChanged; // Event for UI update

    private void Start()
    {
        InvokeRepeating(nameof(RegenerateManaGradually), 1f, 0.5f); // 🔹 Calls every 0.5 second
    }

    private void Awake()
    {
        currentMana = maxMana;
    }

    public bool TryUseMana(int amount)
    {
        if (amount > 0 && amount <= currentMana)
        {
            UseMana(amount);
            return true;
        }
        Debug.Log("Not enough mana.");
        return false;

    }
    public void UseMana(int amount)
    {
        currentMana -= amount;
        Debug.Log("Mana level: " + currentMana);
        onManaChanged?.Invoke(currentMana, maxMana); // Notify UI
    }

    public void RestoreMana(float amount)
    {
        if (amount > 0 && currentMana + amount <= maxMana)
        {
            currentMana += amount;
            onManaChanged?.Invoke(currentMana, maxMana); // Notify UI
        }
        else
        {
            currentMana = maxMana;
        }
    }

    private void RegenerateManaGradually()
    {
        RestoreMana(0.5f);
    }

    public float GetCurrentMana() => currentMana;
    public float GetMaxMana() => maxMana;
}