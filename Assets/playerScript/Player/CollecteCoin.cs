using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollecteCoin : MonoBehaviour
{
    public float collectionDistance = 10f; 
    public float moveSpeed = 5f; 
    public int maxCoinsToCollect = 5; 
    private List<GameObject> coins; 
    private bool isCollecting = false;
    private Dictionary<string, float> memoizationTable = new Dictionary<string, float>();
    private List<Transform> collectedCoins = new List<Transform>();
    private Transform target;
    private Pathfinding pathfinding;
    private Dictionary<Vector3, Dictionary<Vector3, float>> distanceCache;
    private bool useFirstScript = true; 

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
        distanceCache = new Dictionary<Vector3, Dictionary<Vector3, float>>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            useFirstScript = true;
            if (!isCollecting)
            {
                FindCoinsWithinDistance();
                if (coins.Count > 0)
                {
                    isCollecting = true;
                    StartCoroutine(CollectCoinsFirstScript());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            useFirstScript = false;
            isCollecting = !isCollecting;
            if (isCollecting)
            {
                FindOptimalCoinPath();
            }
        }

        if (isCollecting && !useFirstScript)
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
                    isCollecting = false;
                }
            }
            else
            {
                MoveTowardsTarget();
            }
        }
    }
    // Methods for the daynamique script
    void FindCoinsWithinDistance()
    {
        coins = new List<GameObject>();
        GameObject[] allCoins = GameObject.FindGameObjectsWithTag("coin");

        foreach (GameObject coin in allCoins)
        {
            if (Vector3.Distance(transform.position, coin.transform.position) <= collectionDistance)
            {
                coins.Add(coin);
            }
        }
    }

    IEnumerator CollectCoinsFirstScript()
    {
        int coinsCollected = 0;
        Vector3 currentPosition = transform.position;
        List<GameObject> coinPath = new List<GameObject>();

        FindOptimalPathRecursive(currentPosition, coins, new List<GameObject>(), ref coinPath);

        foreach (GameObject coin in coinPath)
        {
            if (coinsCollected >= maxCoinsToCollect) break;

            while (coin != null && Vector3.Distance(transform.position, coin.transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, coin.transform.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            coins.Remove(coin);
            coinsCollected++;
        }

        isCollecting = false;
    }

    float FindOptimalPathRecursive(Vector3 currentPosition, List<GameObject> remainingCoins, List<GameObject> currentPath, ref List<GameObject> bestPath)
    {
        if (remainingCoins.Count == 0 || currentPath.Count >= maxCoinsToCollect)
        {
            if (currentPath.Count > bestPath.Count)
            {
                bestPath = new List<GameObject>(currentPath);
            }
            return 0;
        }

        string memoKey = currentPosition.ToString() + string.Join("", remainingCoins);
        if (memoizationTable.ContainsKey(memoKey))
        {
            return memoizationTable[memoKey];
        }

        float minDistance = Mathf.Infinity;

        foreach (GameObject coin in remainingCoins)
        {
            float distance = Vector3.Distance(currentPosition, coin.transform.position);
            if (distance <= collectionDistance)
            {
                List<GameObject> newRemainingCoins = new List<GameObject>(remainingCoins);
                newRemainingCoins.Remove(coin);

                List<GameObject> newPath = new List<GameObject>(currentPath) { coin };
                float totalDistance = distance + FindOptimalPathRecursive(coin.transform.position, newRemainingCoins, newPath, ref bestPath);

                if (totalDistance < minDistance)
                {
                    minDistance = totalDistance;
                }
            }
        }

        memoizationTable[memoKey] = minDistance;
        return minDistance;
    }
    // Methods for the greedy script
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

        Vector3 start = transform.position;
        List<Vector3> coinPositions = new List<Vector3>();
        foreach (var coin in coinList)
        {
            coinPositions.Add(coin.transform.position);
        }

        List<Vector3> optimalPath = GetOptimalPath(start, coinPositions);
        foreach (var pos in optimalPath)
        {
            foreach (var coin in coinList)
            {
                if (coin.transform.position == pos)
                {
                    collectedCoins.Add(coin.transform);
                    coinList.Remove(coin);
                    break;
                }
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

    List<Vector3> GetOptimalPath(Vector3 start, List<Vector3> positions)
    {
        List<Vector3> path = new List<Vector3>();
        Dictionary<Vector3, bool> visited = new Dictionary<Vector3, bool>();
        foreach (var pos in positions)
        {
            visited[pos] = false;
        }

        Vector3 currentPosition = start;

        while (positions.Count > 0 && path.Count < maxCoinsToCollect)
        {
            Vector3 nearest = FindNearest(currentPosition, positions, visited);
            if (nearest != Vector3.zero)
            {
                path.Add(nearest);
                visited[nearest] = true;
                positions.Remove(nearest);
                currentPosition = nearest;
            }
            else
            {
                break;
            }
        }

        return path;
    }

    Vector3 FindNearest(Vector3 currentPosition, List<Vector3> positions, Dictionary<Vector3, bool> visited)
    {
        Vector3 nearest = Vector3.zero;
        float nearestDistance = Mathf.Infinity;

        foreach (var pos in positions)
        {
            if (!visited[pos])
            {
                float distance = GetCachedDistance(currentPosition, pos);
                if (distance < nearestDistance && distance <= collectionDistance)
                {
                    nearestDistance = distance;
                    nearest = pos;
                }
            }
        }

        return nearest;
    }

    float GetCachedDistance(Vector3 from, Vector3 to)
    {
        if (!distanceCache.ContainsKey(from))
        {
            distanceCache[from] = new Dictionary<Vector3, float>();
        }

        if (!distanceCache[from].ContainsKey(to))
        {
            float distance = Vector3.Distance(from, to);
            distanceCache[from][to] = distance;
        }

        return distanceCache[from][to];
    }

    void MoveTowardsTarget()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

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
        Debug.Log("Collected a coin!");
        Destroy(coin);
        target = null;
    }
}
