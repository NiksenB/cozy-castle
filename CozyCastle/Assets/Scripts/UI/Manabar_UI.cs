using UnityEngine;
using UnityEngine.UI;

public class Manabar_UI : MonoBehaviour
{
    [SerializeField]
    private Image manaFillImage;
    private PlayerStats playerStats;

    public void Start()
    {
        manaFillImage.fillAmount = 1;

        playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.onManaChanged += UpdateManaBar; // Subscribe to event
            UpdateManaBar(playerStats.GetCurrentMana(), playerStats.GetMaxMana());
        }
    }

    public void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.onManaChanged -= UpdateManaBar; // Unsubscribe from event, avoids memory leaks
            playerStats = null; 
        }
    }

    public void Refresh()
    {
        if (playerStats != null)
        {
            UpdateManaBar(playerStats.GetCurrentMana(), playerStats.GetMaxMana());
        }
    }

    public void UpdateManaBar(float currentMana, float maxMana)
    {
        manaFillImage.fillAmount = currentMana / maxMana;
    }
}
