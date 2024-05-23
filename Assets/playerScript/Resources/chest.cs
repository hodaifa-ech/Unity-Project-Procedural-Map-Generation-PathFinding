using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class chest : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] private GameObject player;
    [SerializeField] private List<ItemSpawnData> itemsToDrop = new List<ItemSpawnData>();
    float[] itemWeights;
    [SerializeField][Range(0, 1)] private float dropChance = 0.5f;
    private bool isOpened = false;

    private void Start()
    {
        itemWeights = itemsToDrop.ConvertAll(item => item.rate).ToArray();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isOpened)
            {

                Destroy(gameObject);
                DisplayItems();
            }
            // Add else conditions for other actions related to pressing 'B' outside collision.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isOpened = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isOpened = false;
        }
    }
    private void DisplayItems()
    {
        float dropVariable = Random.value;
        if (dropVariable < dropChance)
        {

            int index = GetRandomWeightedIndex(itemWeights);
            Instantiate(itemsToDrop[index].itemPrefab, transform.position, Quaternion.identity);
        }
    }

    private int GetRandomWeightedIndex(float[] itemWeights)
    {
        float sum = 0f;
        for (int i = 0; i < itemWeights.Length; i++)
        {
            sum += itemWeights[i];
        }
        float randomValue = Random.Range(0, sum);
        float tempSum = 0;

        for (int i = 0; i < itemWeights.Length; i++)
        {
            if (randomValue >= tempSum && randomValue < tempSum + itemWeights[i])
            {
                return i;
            }
            tempSum += itemWeights[i];
        }
        return 0;
    }
}


