using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {
        yield return Bootstrap.Instance.TransitionToScene("Outside",2, "MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
