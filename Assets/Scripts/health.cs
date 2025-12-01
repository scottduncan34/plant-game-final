using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public int pointsOnDeath = 1;
    public bool isPlayer = false;
    public bool isPlant = false;

    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;

        if (isPlayer)
            UpdateHealthUI();
    }

    //update health on hit
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (isPlayer)
            UpdateHealthUI();

        if (currentHealth <= 0f)
            Die();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + Mathf.RoundToInt(currentHealth);
    }

    void Die()
    {
        //lose game if player dies
        if (isPlayer)
        {
            Debug.Log("Player died");

            if (GameStateManager.Instance != null)
                GameStateManager.Instance.EnableUIInput();
                GameStateManager.Instance.LoseGame();

            return;
        }

        //lose game if plant dies
        if (isPlant)
        {
            Debug.Log("Plant destroyed");

            if (GameStateManager.Instance != null)
                GameStateManager.Instance.EnableUIInput();
                GameStateManager.Instance.LoseGame();

            Destroy(gameObject);
            return;
        }

        //give points if zombie dies
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.AddPoints(pointsOnDeath);

        Destroy(gameObject);
    }
}
