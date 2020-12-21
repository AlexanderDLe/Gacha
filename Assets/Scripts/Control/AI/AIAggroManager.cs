using System;
using RPG.Characters;
using UnityEngine;

public class AIAggroManager : MonoBehaviour
{
    GameObject player = null;
    LayerMask playerLayer;
    public GameObject target = null;
    float weaponRange = 0f;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
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

    public void Initialize(GameObject player, EnemyCharacter_SO script)
    {
        this.player = player;
        this.weaponRange = script.weaponRange;
        this.chaseDistance = script.chaseDistance;
        this.aggroCooldownTime = script.aggroCooldownTime;
        this.suspicionTime = script.suspicionTime;
    }

    public float chaseDistance = 5f;
    public float distanceToTarget = 0f;
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
        ScanForPlayerTarget();
        if (!target) return false;

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        bool aggravated = timeSinceLastSawPlayer <= aggroCooldownTime;

        if (distanceToTarget <= chaseDistance) timeSinceLastSawPlayer = 0;

        return distanceToTarget <= chaseDistance || aggravated;
    }

    private void ScanForPlayerTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, chaseDistance, playerLayer);

        float targetDistance = Mathf.Infinity;

        // If player detected within range, then set playerTarget
        foreach (Collider hit in hits)
        {
            float hitDistance = Vector3.Distance(transform.position, hit.transform.position);

            if (hitDistance < targetDistance)
            {
                target = hit.gameObject;
                targetDistance = hitDistance;
            }
        }
    }

    public bool IsSuspicious()
    {
        return timeSinceAggravated < suspicionTime;
    }

    public bool WithinAttackRange()
    {
        return distanceToTarget <= weaponRange;
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
        target = null;
    }
}
