using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //start game button
    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    //quit game button
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}
