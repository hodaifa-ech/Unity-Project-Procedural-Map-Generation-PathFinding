using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    public GameObject PausePanel;




    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }
        
    }

    public void back()
    {
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        GameObject varGameObject = GameObject.FindWithTag("Player");

        varGameObject.GetComponent<AgentInput>().enabled = false;

    }

    public void Continue() 
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        GameObject varGameObject = GameObject.FindWithTag("Player");

        varGameObject.GetComponent<AgentInput>().enabled = true;
    }
}

