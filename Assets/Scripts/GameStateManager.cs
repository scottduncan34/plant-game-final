using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("Points")]
    public int points = 0;
    public TextMeshProUGUI pointsText;

    [Header("Game State UI")]
    public GameObject winPanel;
    public GameObject losePanel;

    private bool gameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdatePointsUI();

        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
    }

    public void EnableUIInput()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    //track player points
    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsUI();
    }

    public bool SpendPoints(int amount)
    {
        if (points < amount)
            return false;

        points -= amount;
        UpdatePointsUI();
        return true;
    }

    void UpdatePointsUI()
    {
        if (pointsText)
            pointsText.text = "Points: " + points;
    }

    
    //game end conditions
    public void WinGame()
    {
        if (gameOver) return;
        gameOver = true;

        if (winPanel)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("YOU WIN!");
    }

    public void LoseGame()
    {
        if (gameOver) return;
        gameOver = true;

        if (losePanel)
            losePanel.SetActive(true);

        Time.timeScale = 0.01f;
        Debug.Log("YOU LOSE!");
    }

    //game end button functions
    public void RestartGame()
    {

        Time.timeScale = 1f;
        Debug.Log("RESTART CLICKED");
        Cursor.lockState = CursorLockMode.Locked;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
