using System;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    GameObject player = null;
    StateMachine stateMachine = null;
    NavMeshAgent navMeshAgent = null;
    Animator animator = null;

    public float chaseDistance = 5f;
    public float weaponRange = 1f;
    public float movementSpeed = 4f;
    public float distanceToPlayer = 0f;

    public float timeSinceLastSawPlayer = Mathf.Infinity;
    public float timeSinceAggravated = Mathf.Infinity;
    public float aggroCooldownTime = 3f;
    public float suspicionTime = 3f;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceAggravated += Time.deltaTime;
    }

    public bool IsAggravated()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        bool aggravated = timeSinceLastSawPlayer <= aggroCooldownTime;

        if (distanceToPlayer <= chaseDistance) timeSinceLastSawPlayer = 0;

        return distanceToPlayer <= chaseDistance || aggravated;
    }
    public bool IsSuspicious()
    {
        return timeSinceAggravated < suspicionTime;
    }

    public bool WithinAttackRange()
    {
        print(distanceToPlayer <= weaponRange);
        return distanceToPlayer <= weaponRange;
    }

    public void AttackStart() { }
    public void AttackActivate() { }
    public void AttackEnd() { }
}
