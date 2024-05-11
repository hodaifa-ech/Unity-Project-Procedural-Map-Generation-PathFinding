using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MiniBoss : MonoBehaviour
{

    public Enemy MiniBossObject;
    private bool trans = false;
    private GameObject player;

    public int healthIncreaseRate = 1;
    // Time interval in seconds before health increases
    public float healthIncreaseInterval = 1.0f;
    private float timeSinceLastIncrease = 0.0f;

    private int healthTmp = 0;
    private Rigidbody2D rb;
    





    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        GameObject obj = GameObject.FindWithTag("MiniBoss");
        MiniBossObject = obj.GetComponent<Enemy>();
        healthTmp = MiniBossObject.Health;


        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (MiniBossObject.Health <= healthTmp-5)
        {
            healthTmp = MiniBossObject.Health;
            transform.position = player.transform.position;
        }
        if(MiniBossObject.Health <= 5)
        {
            rb.velocity = rb.velocity * 2f; 
        }

        timeSinceLastIncrease += Time.deltaTime;

        // Check if it's time to increase health
        if (timeSinceLastIncrease >= healthIncreaseInterval && MiniBossObject.Health != MiniBossObject.EnemyData.MaxHealth)
        {
            // Increase health
            MiniBossObject.Health += healthIncreaseRate;

            // Reset the timer
            timeSinceLastIncrease = 0.0f;

        }



    }




}
