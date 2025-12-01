using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantObjective : MonoBehaviour
{
    [Header("Plant Health")]
    public Health plantHealth;

    [Header("Objective Water")]
    public float waterRequired = 100f;
    public float currentWater = 0f;

    [Header("UI")]
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;

    private bool hasWon = false;

    void Start()
    {
        if (!plantHealth)
            plantHealth = GetComponent<Health>();

        UpdateUI();
        UpdateHealthUI();
    }

    void Update()
    {
        UpdateHealthUI();
    }

    //add water to objective
    public void AddWater(float amount)
    {
        if (hasWon)
            return;

        currentWater += amount;
        currentWater = Mathf.Clamp(currentWater, 0, waterRequired);

        UpdateUI();

        if (currentWater >= waterRequired)
            WinGame();
    }

    void UpdateUI()
    {
        if (waterText)
            waterText.text = "Water: " +
                Mathf.RoundToInt(currentWater) +
                " / " +
                Mathf.RoundToInt(waterRequired);
    }

    void UpdateHealthUI()
    {
        if (!plantHealth)
            return;

        if (healthText)
            healthText.text = "Plant HP: " +
                Mathf.RoundToInt(plantHealth.currentHealth);

        if (healthSlider)
            healthSlider.value =
                plantHealth.currentHealth / plantHealth.maxHealth;
    }

    //win if objective is complete
    void WinGame()
    {
        hasWon = true;

        Debug.Log("YOU WIN – OBJECTIVE COMPLETE!");

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.EnableUIInput();
            GameStateManager.Instance.WinGame();
    }
}
