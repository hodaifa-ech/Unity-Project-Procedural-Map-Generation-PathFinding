using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHealthSlider : MonoBehaviour
{
    public Slider progressSlider ;
    public Enemy Boss;

    private float BossHealth;


    private void Start()
    {
        BossHealth = Boss.Health; 
        progressSlider.value = 1;
    }

    
    
    private void Update()
    {

        float progress = Boss.Health;
        if (Boss.dead != true)
        {
            
            progress = progress / BossHealth;
            progressSlider.value = progress;
        }
        else
            progressSlider.value = progress;

    }

    // Update is called once per frame

}
