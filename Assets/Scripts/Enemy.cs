using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float flightHeight = 2f;

    [Header("Detection")]
    [SerializeField] private float playerDetectionRange = 8f;
    [SerializeField] private float cropDetectionRange = 10f;
    [SerializeField] private float attackRange = 1.6f;

    [Header("Combat")]
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int playerDamage = 1;

    [Header("Crop")]
    [SerializeField] private float eatDuration = 2f;

    [Header("Retreat")]
    [SerializeField] private float retreatDuration = 2.5f;
    [SerializeField] private float retreatDistance = 6f;

    private IEnemyState currentState;
    private float nextAttackTime;
    private Transform playerTarget;
    private Transform cropTarget;
    private Vector3 spawnPoint;

    private IdleState idleState;
    private AttackState attackState;
    private EatState eatState;
    private RetreatingState retreatingState;

    public event Action<EnemyMovement> OnStateChanged;

    public Transform PlayerTarget => playerTarget;
    public Transform CropTarget => cropTarget;
    public float MoveSpeed => moveSpeed;
    public float FlightHeight => flightHeight;
    public float PlayerDetectionRange => playerDetectionRange;
    public float CropDetectionRange => cropDetectionRange;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public int PlayerDamage => playerDamage;
    public float EatDuration => eatDuration;
    public float RetreatDuration => retreatDuration;
    public float RetreatDistance => retreatDistance;
    public Vector3 SpawnPoint => spawnPoint;

    private void Start()
    {
        spawnPoint = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }

        idleState = new IdleState();
        attackState = new AttackState();
        eatState = new EatState();
        retreatingState = new RetreatingState();

        ChangeState(idleState);
    }

    private void Update()
    {
        currentState?.Tick(this, Time.deltaTime);
    }

    public void ChangeState(IEnemyState newState)
    {
        if (newState == null)
        {
            return;
        }

        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
        OnStateChanged?.Invoke(this);
    }

    public void SetCropTarget(Transform newCropTarget)
    {
        cropTarget = newCropTarget;
    }

    public bool HasValidPlayerTarget()
    {
        return playerTarget != null;
    }

    public bool HasValidCropTarget()
    {
        return cropTarget != null;
    }

    public float DistanceToPlayer()
    {
        if (!HasValidPlayerTarget())
        {
            return float.MaxValue;
        }

        return Vector3.Distance(transform.position, playerTarget.position);
    }

    public float DistanceToCrop()
    {
        if (!HasValidCropTarget())
        {
            return float.MaxValue;
        }

        return Vector3.Distance(transform.position, cropTarget.position);
    }

    public bool CanAttackPlayer()
    {
        return Time.time >= nextAttackTime;
    }

    public void TryAttackPlayer()
    {
        if (!HasValidPlayerTarget() || !CanAttackPlayer())
        {
            return;
        }

        if (DistanceToPlayer() > attackRange)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        playerTarget.gameObject.SendMessage("TakeDamage", playerDamage, SendMessageOptions.DontRequireReceiver);
    }

    public bool FindNearestCropTarget()
    {
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");
        Transform nearest = null;
        float nearestDistance = float.MaxValue;

        for (int index = 0; index < crops.Length; index++)
        {
            GameObject crop = crops[index];
            if (crop == null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, crop.transform.position);
            if (distance < nearestDistance && distance <= cropDetectionRange)
            {
                nearestDistance = distance;
                nearest = crop.transform;
            }
        }

        cropTarget = nearest;
        return cropTarget != null;
    }

    public void MoveTowards(Vector3 targetPosition, float deltaTime)
    {
        Vector3 desired = targetPosition;
        desired.y = flightHeight;

        transform.position = Vector3.MoveTowards(
            transform.position,
            desired,
            moveSpeed * deltaTime);

        Vector3 lookDirection = desired - transform.position;
        lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                12f * deltaTime);
        }
    }

    public void ScareAway()
    {
        ChangeState(retreatingState);
    }

    public void OnHitByPlayer()
    {
        ScareAway();
    }

    public void TakeDamage(int amount)
    {
        ScareAway();
    }

    public void TakeDamage()
    {
        ScareAway();
    }



    public IEnemyState GetIdleState()
    {
        return idleState;
    }

    public IEnemyState GetAttackState()
    {
        return attackState;
    }

    public IEnemyState GetEatState()
    {
        return eatState;
    }

    public IEnemyState GetRetreatState()
    {
        return retreatingState;
    }
}
