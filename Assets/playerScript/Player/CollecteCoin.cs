using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CollecteCoin : MonoBehaviour
{
    public float collectionDistance = 5.0f; // Distance within which the player can collect coins
    public float moveSpeed = 5.0f; // Speed at which the player moves towards the coin
    public int maxCoinsToCollect = 10; // Maximum number of coins to collect in the optimized path

    private Transform target; // The target the player is currently moving towards (coin or MiniBoss)
    private bool isCollecting = false; // Flag to track if the player is collecting coins
    private List<Transform> collectedCoins = new List<Transform>(); // List to keep track of collected coins

    private Pathfinding pathfinding;
    private PathRequestManager requestManager;
    private Grid grid;

    public static KeyCode key = KeyCode.C; 

    

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();

        
    }

    void Update()
    {
        if (Input.GetKeyDown(key) )
        {
            isCollecting = true; // Toggle collecting state
            if (isCollecting)
            {
                FindOptimalCoinPath();
            }
        }

        if (isCollecting)
        {
            if (target == null)
            {
                if (collectedCoins.Count > 0)
                {
                    target = collectedCoins[0];
                    collectedCoins.RemoveAt(0);
                    if (pathfinding != null)
                    {
                        StartCoroutine(pathfinding.astar(transform.position, target.position));
                    }

                }
                else
                {
                    isCollecting = false; // Stop collecting if no more coins are available
                }
            }
            else
            {
                MoveTowardsTarget();
            }
        }
    }

    

    void FindOptimalCoinPath()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("coin");
        if (coins == null || coins.Length == 0)
        {
            Debug.LogWarning("No coins found in the scene!");
            return;
        }

        List<GameObject> coinList = new List<GameObject>(coins);
        collectedCoins.Clear();

        Vector3 currentPosition = transform.position;

        for (int i = 0; i < maxCoinsToCollect; i++)
        {
            GameObject nearestCoin = null;
            float nearestDistance = Mathf.Infinity;

            foreach (var coin in coinList)
            {
                if (coin == null) continue;

                float distance = Vector3.Distance(currentPosition, coin.transform.position);
                if (distance < nearestDistance && distance <= collectionDistance)
                {
                    nearestDistance = distance;
                    nearestCoin = coin;
                }
            }

            if (nearestCoin != null)
            {
                collectedCoins.Add(nearestCoin.transform);
                coinList.Remove(nearestCoin);
                currentPosition = nearestCoin.transform.position;
            }
            else
            {
                break; // No more reachable coins within collection distance
            }
        }

        if (collectedCoins.Count > 0)
        {
            target = collectedCoins[0];
            collectedCoins.RemoveAt(0);
            if (pathfinding != null)
            {
                StartCoroutine(pathfinding.astar(transform.position, target.position));
            }

        }
    }

    void MoveTowardsTarget()
    {
        if (target == null) return;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        else isCollecting = false;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target.CompareTag("coin"))
            {
                CollectCoin(target.gameObject);
            }
           
        }
    }

    void CollectCoin(GameObject coin)
    {
        // You can add your own logic here, such as increasing the score, playing a sound, etc.
        Debug.Log("Collected a coin!");

        // Destroy the coin or use your existing collision-based destruction
        Destroy(coin);

        // Reset the target so the player can find the next one
        target = null;
    }
}
