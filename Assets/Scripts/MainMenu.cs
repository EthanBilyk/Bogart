using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPlayButton(){
        SceneManager.LoadScene(1);
        Debug.Log("Clicked Play Button.");
    }
    
    public void OnOptionsButton(){
        SceneManager.LoadScene(2);
    }
    
    public void OnQuitButton(){
        Application.Quit();
    }
}
