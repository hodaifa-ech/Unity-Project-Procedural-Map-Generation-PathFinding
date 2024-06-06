using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniBoss : MonoBehaviour
{


    // Panel de Transition au Grand Boss 
    public GameObject VictoryPanel;
    public static bool isDead = false; 

    public Enemy MiniBossObject;
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
        VictoryPanel.SetActive(false);

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

        timeSinceLastIncrease += Time.deltaTime;

        // Check if it's time to increase health
        if (timeSinceLastIncrease >= healthIncreaseInterval && MiniBossObject.Health != MiniBossObject.EnemyData.MaxHealth)
        {
            // Increase health
            MiniBossObject.Health += healthIncreaseRate;

            // Reset the timer
            timeSinceLastIncrease = 0.0f;

        }
        //if (!GameObject.FindGameObjectWithTag("MiniBoss") && !isDead) // Corrected the condition here
        //    Dead();

        


    }

    private void Dead()
    {
        isDead = true; 
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] spawner = GameObject.FindGameObjectsWithTag("SpawnerEnemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Enemy e = enemy.GetComponent<Enemy>();
                e.dead = true;
                e.OnDie?.Invoke();

            }
        }

        foreach (GameObject spawn in spawner)
        {
            if (spawn != null)
            {
                DestroyImmediate(spawn);
            }
        }


        // Freeze all Rigidbody components in the scene
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Pause the player Inputs 
        GameObject varGameObject = GameObject.FindWithTag("Player");
        varGameObject.GetComponent<AgentInput>().enabled = false;

        // Display the panel of the Victory 
        VictoryPanel.SetActive(true); 
    }


   /*
    private void OnDestroy()
    {
        try
        {
            Dead();
        }
        catch { Debug.Log("error De Exception ! "); }
    }
   */



}
