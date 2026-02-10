using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Outside");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
