using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

public class TestAbility : MonoBehaviour, IHasCooldown
{
    private int id = 1;
    private float cooldownDuration = 5f;
    [SerializeField] CooldownSystem cooldownSystem;

    public int Id => id;

    public float CooldownDuration => cooldownDuration;

    private void Update()
    {
        if (cooldownSystem.IsOnCooldown(id)) return;
        else
        {
            // Cast Skill
        }
    }
}