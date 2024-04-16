using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeGame();
    }

    public void OnQuitToMainMenuButtonClick()
    {
        SceneManager.LoadScene(0);
        ResumeGame();
    }

    public void OnQuitGameButtonClick()
    {
        Application.Quit();
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        FindObjectOfType<Player>().setAlive(true);
        FindObjectOfType<PlayerMenuObject>().setActive(false);
        FindObjectOfType<Player>().setPaused(false);
    }

    
}
