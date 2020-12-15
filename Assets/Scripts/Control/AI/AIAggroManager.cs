using System;
using RPG.Characters;
using UnityEngine;

public class AIAggroManager : MonoBehaviour
{
    GameObject player = null;
    float weaponRange = 0f;

    private void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceAggravated += Time.deltaTime;
    }

    public void Initialize(GameObject player, EnemyCharacter_SO script)
    {
        this.player = player;
        this.weaponRange = script.weaponRange;
        this.chaseDistance = script.chaseDistance;
        this.aggroCooldownTime = script.aggroCooldownTime;
        this.suspicionTime = script.suspicionTime;
    }

    public float chaseDistance = 5f;
    public float distanceToPlayer = 0f;
    public float timeSinceLastSawPlayer = Mathf.Infinity;
    public float timeSinceAggravated = Mathf.Infinity;
    public float aggroCooldownTime = 3f;
    public float suspicionTime = 3f;
    public bool isAggressive = false;

    public void Aggravate()
    {
        timeSinceAggravated = 0;
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
        return distanceToPlayer <= weaponRange;
    }
    public event Action OnStartAggression;
    public void StartAggression()
    {
        isAggressive = true;
        OnStartAggression();
    }
    public void EndAggression()
    {
        isAggressive = false;
    }
}
