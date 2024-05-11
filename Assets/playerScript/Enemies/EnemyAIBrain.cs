using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIBrain : MonoBehaviour, IAgentInput
{
    [field: SerializeField]
    public GameObject Target { get; set; }

    [field: SerializeField]
    public AIState CurrentState { get; private set; }


    [field: SerializeField]
    public UnityEvent OnFireButtonPressed { get; set; }
    [field: SerializeField]
    public UnityEvent OnFireButtonReleased { get; set; }
    [field: SerializeField]
    public UnityEvent<Vector2> OnMovementKeyPressed { get; set; }
    [field: SerializeField]
    public UnityEvent<Vector2> OnPointerPositionChange { get; set; }


    private void Awake()
    {
        try
        {
            Target = FindObjectOfType<Player>().gameObject;
        }
        catch{}
    }
    private void Update()
    {
        if (Target == null)
        {
            OnMovementKeyPressed?.Invoke(Vector2.zero);
        }
        
        else
        {
            CurrentState.UpdateState();
        }
    }

    public void Attack()
    {
        // Check if the Target GameObject is null or has been destroyed
        if (Target == null)
        {
            Debug.LogWarning("Target GameObject is null or has been destroyed.");
            return;
        }

        // Invoke the OnFireButtonPressed event
        OnFireButtonPressed?.Invoke();
    }

    public void Move(Vector2 movementDirection, Vector2 targetPosition)
    {
        OnMovementKeyPressed?.Invoke(movementDirection);
        OnPointerPositionChange?.Invoke(targetPosition);
    }

    internal void ChangeToState(AIState state)
    {
        CurrentState = state;
    }
}