using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Enemy : MonoBehaviour, IHittable, IAgent, IKnockBack
{

    public static int Kills = 0;


    [field: SerializeField]
    public EnemyDataSO EnemyData { get; set; }

    [field: SerializeField]
    public int Health { get; set; } = 2;

    [field: SerializeField]
    public EnemyAttack enemyAttack { get; set; }

    public bool dead = false;

    private AgentMovement agentMovemenet;


    [field: SerializeField]
    public UnityEvent OnGetHit { get; set; }

    [field: SerializeField]
    public UnityEvent OnDie { get; set; }
    [SerializeField]
    private UILevel uilevel;

    public Player PlayerCoin; // player pour collecter les coins 
    public Button level;

    private bool upgradeActivated = false;
    private void Awake()
    {
        PlayerCoin = FindObjectOfType<Player>();
        if (enemyAttack == null)
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }
        agentMovemenet = GetComponent<AgentMovement>();
    }

    private void Start()
    {
        level.onClick.AddListener(UpgradeButtonClick);

    }
    private void UpgradeButtonClick()
    {
        if (PlayerCoin.coin >= 250)
        {
            upgradeActivated = true;
            uilevel.Udpatelevel(2);

        }
        else if (PlayerCoin.coin >= 1000)
        {
            upgradeActivated = true;
            uilevel.Udpatelevel(4);

        }

    }
    public void GetHit(int damage, GameObject damageDealer)
    {
        if (dead == false)
        {
            if (PlayerCoin.coin >= 250 && damage < 2 && upgradeActivated)
            {
                damage = damage * 2;
            }

            else if (PlayerCoin.coin >= 1000 && damage < 4 && upgradeActivated)
            {
                damage = damage * 2;
            }
            Health -= damage;
            OnGetHit?.Invoke();
            if (Health <= 0)
            {
                dead = true;
                OnDie?.Invoke();

            }
        }
    }
    public void Die()
    {


        if (MiniBoss.isDead == false || GameObject.FindWithTag("Boss") == false)
        {
            Kills++;
        }
        Destroy(gameObject);

    }
    public void PerformAttack()
    {
        if (dead == false)
        {
            enemyAttack.Attack(EnemyData.Damage);
        }
    }
    public void KnockBack(Vector2 direction, float power, float duration)
    {
        agentMovemenet.KnockBack(direction, power, duration);
    }
}