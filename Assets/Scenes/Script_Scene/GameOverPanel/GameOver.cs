using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{


    public GameObject player;
    public GameObject GameOverPanel; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        GameOverPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf)
        {
            GameOverPanel.SetActive(false);
        }
        else
        {
            GameOverPanel.SetActive(true);

        }
    }
}
