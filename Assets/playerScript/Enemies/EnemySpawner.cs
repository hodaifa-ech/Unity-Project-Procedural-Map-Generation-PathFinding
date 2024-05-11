using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;
    public float distance = 5;
    public GameObject player;

    [SerializeField] private int NbrOfEnemy = 10;

    private void Start()
    {
    }

    private void Update()
    {
        // Check distance and start/stop spawning accordingly
        

        if (Vector2.Distance(player.transform.position, transform.position) <= distance )
        {
            if (!canSpawn)
            {
                canSpawn = true;
                StartCoroutine(Spawner());
            }
        }
        else
        {
            if (canSpawn)
            {
                canSpawn = false;
                StopCoroutine(Spawner());
            }
        }
        
    }

    private int enemiesSpawned = 0;
    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);
        while (canSpawn && enemiesSpawned <= NbrOfEnemy && player.activeSelf)
        {
            yield return wait;
            if (enemyPrefabs.Length > 0)
            {
                int rand = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyToSpawn = enemyPrefabs[rand];
                if (enemyToSpawn != null) // Check if the prefab to spawn is not null
                {
                    Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
                    enemiesSpawned++;
                }
            }
            else
            {
                Debug.LogWarning("No enemy prefabs assigned in EnemySpawner!");
            }
        }
    }
}
