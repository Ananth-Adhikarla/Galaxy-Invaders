using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 0.25f;

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {

        SceneManager.LoadScene(1);

        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession != null) { gameSession.ResetGame(); }

        Player player = FindObjectOfType<Player>();
        if (player != null) { player.ResetGame(); }

        Manager manager = FindObjectOfType<Manager>();
        if (manager != null) { manager.RecreateObjects(); }

    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
        Player player = FindObjectOfType<Player>();
        if (player != null) { player.ResetGame(); }
    }

    public void LoadNextLevel(int sceneNumber)
    {
        StartCoroutine(WaitAndLoadLevel(sceneNumber));
    }

    IEnumerator WaitAndLoadLevel(int sceneNumber)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(sceneNumber);
    }

    public void RestartGame(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);

        Player player = FindObjectOfType<Player>();
        Manager manager = FindObjectOfType<Manager>();
        if (player != null) { player.ResetGame(); }
        if (manager != null) { manager.RecreateObjects(); }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
