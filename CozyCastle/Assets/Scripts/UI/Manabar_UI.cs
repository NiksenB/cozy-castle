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
            playerStats.onManaChanged += UpdateBar; // Subscribe to event
            UpdateBar(playerStats.GetCurrentMana(), playerStats.GetMaxMana());
        }
    }

    public void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.onManaChanged -= UpdateBar; // Unsubscribe from event, avoids memory leaks
            playerStats = null; 
        }
    }

    public void Refresh()
    {
        if (playerStats != null)
        {
            UpdateBar(playerStats.GetCurrentMana(), playerStats.GetMaxMana());
        }
    }

    public void UpdateBar(float currentMana, float maxMana)
    {
        manaFillImage.fillAmount = currentMana / maxMana;
    }
}
