using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillsCounter : MonoBehaviour
{


    public GameObject Boss;
    public GameObject BossHealth; 


    // counter kills
    public TextMeshProUGUI counterKills;
    void Start()
    {
        counterKills.text = "0 Kills";
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.Kills != 0)
        {
            counterKills.text = Enemy.Kills.ToString() + " Kills";
        }

        if(Enemy.Kills >= 1 && Boss && BossHealth) 
        {
            Boss.SetActive(true);
            BossHealth.SetActive(true);
        }

    }
}
