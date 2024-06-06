using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiniBoss : MonoBehaviour
{
    [SerializeField]
    private GameObject MiniBoss;

    [SerializeField]
    private GameObject MiniBossHealth;

    [SerializeField]
    private int nbrKills = 2; 


    // Update is called once per frame
    void Update()
    {
        if(Enemy.Kills == nbrKills)
        {
            MiniBoss.SetActive(true);
            MiniBossHealth.SetActive(true);
        }
    }
}
