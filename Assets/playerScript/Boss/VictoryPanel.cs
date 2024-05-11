using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanel : MonoBehaviour
{
    public GameObject VictoryPanelGameObject;
    private bool Paused = false;
    private List<Enemy> enemy = new List<Enemy>(); // Initialize the list here

    private void Update()
    {
        if (!GameObject.FindWithTag("Boss") && !Paused) // Corrected the condition here
            Pause();
    }

    private void Pause()
    {
        // Pause 

        Paused = true;

        // 

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("SmallEnemy");
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

        foreach (GameObject enemy in enemies2)
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
                spawn.SetActive(false);
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
        VictoryPanelGameObject.SetActive(true);
    }

}
