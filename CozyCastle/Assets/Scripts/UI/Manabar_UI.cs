using UnityEngine;
using UnityEngine.UI;

public class Manabar_UI : MonoBehaviour
{
    [SerializeField] private Image manaFillImage;
    private PlayerStats playerStats;

    private void OnEnable()
    {
        PlayerInventoryManager.OnInventoriesReady += TryBind;
        TryBind();
    }

    private void OnDisable()
    {
        PlayerInventoryManager.OnInventoriesReady -= TryBind;
        Unbind();
    }

    private void TryBind()
    {
        var stats = FindFirstObjectByType<PlayerStats>();
        if (stats == null) return;

        Unbind();
        playerStats = stats;
        playerStats.onManaChanged += UpdateBar;
        UpdateBar(playerStats.GetCurrentMana(), playerStats.GetMaxMana());
    }

    private void Unbind()
    {
        if (playerStats == null) return;
        playerStats.onManaChanged -= UpdateBar;
        playerStats = null;
    }

    public void Refresh()
    {
        if (playerStats != null)
            UpdateBar(playerStats.GetCurrentMana(), playerStats.GetMaxMana());
    }

    public void UpdateBar(float currentMana, float maxMana)
    {
        manaFillImage.fillAmount = currentMana / maxMana;
    }
}