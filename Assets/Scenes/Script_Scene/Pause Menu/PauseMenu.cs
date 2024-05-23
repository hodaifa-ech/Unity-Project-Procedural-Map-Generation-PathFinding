using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    public GameObject PausePanel;

    private bool isPaused = false;


    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && !isPaused)
        {
            isPaused = true ; 
            Pause();
        }
        if(Input.GetKey(KeyCode.Escape) && isPaused)
        {
            isPaused = false ;
            Continue();
        }

    }

    

    public void Pause()
    {
        isPaused = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        GameObject varGameObject = GameObject.FindWithTag("Player");

        varGameObject.GetComponent<AgentInput>().enabled = false;

    }

    public void Continue() 
    {
        isPaused = false ;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        GameObject varGameObject = GameObject.FindWithTag("Player");

        varGameObject.GetComponent<AgentInput>().enabled = true;
    }

    public void Quit()
    {
        // quitter le jeu 
    }
}

