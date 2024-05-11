using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;



public class Player : MonoBehaviour, IAgent, IHittable
{

    [SerializeField]
    private int maxHealth = 2;

    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            uiHealth.UpdateUI(health);
        }
    } // Initial health value
    private int currentHealth; // Current health value

    [SerializeField]
    private Vector3 startPosition; // Start position for the player

    [SerializeField]
    private AbstractDungeonGenerator dungeonGenerator;



    private bool dead = false;
    private PlayerWeapon playeWeapon;
    [field: SerializeField]
    public UIHealth uiHealth { get; set; }


    [field: SerializeField]
    public UnityEvent OnDie { get; set; }
    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }


    private void Awake()
    {
        playeWeapon = GetComponentInChildren<PlayerWeapon>();
    }
    private void Start()
    {
        currentHealth = maxHealth; // Set current health to initial health
        Health = maxHealth;
        uiHealth.Initialize(Health);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Resource"))
        {
            var resource = collision.gameObject.GetComponent<Resource>();
            if (resource != null)
            {
                switch (resource.ResourceData.ResourceType)
                {
                    case ResourceTypeEnum.Health:
                        if (Health >= maxHealth)
                        {
                            return;
                        }
                        Health += resource.ResourceData.GetAmount();
                        resource.PickUpResource();
                        break;
                    case ResourceTypeEnum.Ammo:
                        if (playeWeapon.AmmoFull)
                        {
                            return;
                        }
                        playeWeapon.AddAmmo(resource.ResourceData.GetAmount());
                        resource.PickUpResource();
                        break;
                    default:
                        break;
                }
            }

        }
    }
    public void GetHit(int damage, GameObject damageDealer)
    {
        if (!dead)
        {
            Health -= damage; // Reduce health by damage amount
            OnGetHit?.Invoke(); // Invoke OnGetHit event

            if (Health <= 0)
            {
                Die(); // Player dies
            }
        }
    }
    private void Die()
    {
        dead = true; // Player is dead
        OnDie?.Invoke(); // Invoke OnDie event
        //Respawn(); // Respawn the player

        // Pause Manager 
        gameObject.SetActive(false);
    }

    /*
    private void Respawn()
    {
        transform.position = startPosition; // Respawn at start position
        currentHealth = maxHealth; // Reset health to initial value
        //Health = maxHealth;
        dead = false; // Player is no longer dead


        



        
        if (dungeonGenerator != null)
        {
            dungeonGenerator.GenerateDunguon(); // Regenerate the dungeon
        }
        
    }

    */
    


}
