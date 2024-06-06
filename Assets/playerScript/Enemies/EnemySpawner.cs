using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private bool canSpawn = true;

    public List<GameObject> InstantiatedEnemies = new List<GameObject>();
    public List<GameObject> RealTimeEnemies = new List<GameObject>();
    public static int DestroyedCounter;

    [field: SerializeField]
    [field: Range(0.1f, 20)]
    public float Distance { get; set; } = 5;

    [field: SerializeField]
    [field: Range(0.1f, 20)]
    public int NumberOfEnemiesRealTime { get; set; } = 5;

    public GameObject player;
    [SerializeField] private int NbrOfEnemy = 10;

    private void Start()
    {
    }

    private void Update()
    {
        // Check distance and start/stop spawning accordingly
        if (Vector2.Distance(player.transform.position, transform.position) <= Distance)
        {
            if (!canSpawn)
            {
                canSpawn = true;
                DestroyedCounter = 0;
                RegenerateListEnemies();
                StartCoroutine(Spawner());
            }
        }
        else
        {
            if (canSpawn)
            {
                canSpawn = false;
                DestroyedCounter = 0;
                RegenerateListEnemies();
                StartCoroutine(Spawner());

            }
        }
        try
        {
            CountEnemiesDestroyed();
            EnemiesExist();
        }
        catch
        {
            Debug.Log("Error CountEnemiesDestroyed() / EnemiesExist() !!  ");
        }
        
    }

    public void CountEnemiesDestroyed()
    {
        if (InstantiatedEnemies.Count > 0)
        {
            List<GameObject> objectsToRemove = new List<GameObject>();

            foreach (GameObject obj in InstantiatedEnemies)
            {
                if (obj == null)
                {
                    DestroyedCounter++;
                    objectsToRemove.Add(obj);
                }
            }

            foreach (GameObject obj in objectsToRemove)
            {
                InstantiatedEnemies.Remove(obj);
            }
        }
    }

    public void EnemiesExist()
    {
        RealTimeEnemies.Clear();
        foreach (GameObject obj in InstantiatedEnemies)
        {
            if (obj != null)
            {
                RealTimeEnemies.Add(obj);
            }
        }
    }

    public void RegenerateListEnemies()
    {
        EnemiesExist();
        InstantiatedEnemies = new List<GameObject>(RealTimeEnemies);
    }

    private int enemiesSpawned = 0;
    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);
        while (canSpawn && enemiesSpawned < NbrOfEnemy && player.activeSelf && RealTimeEnemies.Count < NumberOfEnemiesRealTime)
        {
            yield return wait;
            if (enemyPrefabs.Length > 0)
            {
                int rand = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyToSpawn = enemyPrefabs[rand];
                if (enemyToSpawn != null)
                {
                    GameObject obj = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
                    InstantiatedEnemies.Add(obj);
                    enemiesSpawned++;
                }
            }
            else
            {
                Debug.LogWarning("No enemy prefabs assigned in EnemySpawner!");
            }
        }
    }
    /*
    protected void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Distance);
            Gizmos.color = Color.white;
        }
    }
    */
}
