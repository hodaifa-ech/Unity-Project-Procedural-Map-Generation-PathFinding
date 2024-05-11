using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class AgentMovement : MonoBehaviour
{


    private float activeMoveSpeed = 1f;
    public float dashSpeed = 1f;

    private float dashLength = .5f, dashCooldown = 1f;

    public float dashCounter = 0f;
    private float dashCoolCounter = 0f;

    private GameObject player;


    public float SmallEnemySpeed = 0.3f;

    /**/
    protected Rigidbody2D rigidbody2d;

    [field: SerializeField]
    public MovementDataSO MovementData { get; set; }

    [SerializeField]
    protected float currentVelocity = 3;
    protected Vector2 movementDirection;

    [field: SerializeField]
    public UnityEvent<float> OnVelocityChange { get; set; }

    protected bool isKnockedBack = false;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void MoveAgent(Vector2 movementInput)
    {
        movementDirection = movementInput;
        currentVelocity = CalculateSpeed(movementInput);
        
    }

    private float CalculateSpeed(Vector2 movementInput)
    {
        if(movementInput.magnitude > 0)
        {
            currentVelocity += MovementData.acceleration * Time.deltaTime;
        }
        else
        {
            currentVelocity -= MovementData.deacceleration * Time.deltaTime;
        }
        return Mathf.Clamp(currentVelocity, 0, MovementData.maxSpeed);
    }

    private void FixedUpdate()
    {
        OnVelocityChange?.Invoke(currentVelocity);
        if (isKnockedBack == false)
            rigidbody2d.velocity = currentVelocity * movementDirection.normalized;
        /**/
        if(gameObject.tag == "SmallEnemy")
        {
            rigidbody2d.velocity = (currentVelocity + SmallEnemySpeed) * movementDirection.normalized;
        }


        if (gameObject.tag == "Boss")
        {
            rigidbody2d.velocity = (currentVelocity + activeMoveSpeed) * movementDirection.normalized;

            if (Vector3.Distance(transform.position, player.transform.position) > 4f)
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;

                if (dashCounter <= 0)
                {
                    activeMoveSpeed = 0;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }


        }





        /**/
    }
    public void KnockBack(Vector2 direction, float power, float duration)
    {
        if (isKnockedBack == false && gameObject.tag != "Boss")
        {
            isKnockedBack = true;
            StartCoroutine(KnockBackCoroutine(direction, power, duration));
        }
    }

    public void ResetKnockBack()
    {
        StopAllCoroutines();
        ResetKnockBackParameters();
    }

    IEnumerator KnockBackCoroutine(Vector2 direction, float power, float duration)
    {
        rigidbody2d.AddForce(direction.normalized * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        ResetKnockBackParameters();
    }

    private void ResetKnockBackParameters()
    {
        currentVelocity = 0;
        rigidbody2d.velocity = Vector2.zero;
        isKnockedBack = false;
    }
}
