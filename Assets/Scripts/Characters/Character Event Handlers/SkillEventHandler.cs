using System;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    public abstract class SkillEventHandler : MonoBehaviour
    {
        public AudioManager audioManager;
        public Stats stats;
        public PlayableCharacter_SO script;
        public ObjectPooler charObjectPooler;
        public RaycastMousePosition raycaster;
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        public ProjectileLauncher projectileLauncher;
        public float probability = .5f;

        public SkillManager movSkill;
        public SkillManager priSkill;
        public SkillManager ultSkill;

        public abstract void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator, NavMeshAgent navMeshAgent, ProjectileLauncher projectileLauncher);

        public abstract void Initialize(Stats stats, PlayableCharacter_SO charScript);
        public abstract void InitializeSkillManagers(SkillManager movementSkill, SkillManager primarySkill, SkillManager ultimateSkill);

        public abstract void InitializeMovementSkill();
        public abstract void InitializePrimarySkill();
        public abstract void InitializeUltimateSkill();

        public Dictionary<string, Action> enterSkillDict;
        public Dictionary<string, Action> executeSkillDict;
        public Dictionary<string, Action> exitSkillDict;

        public abstract void EnterMovementSkill();
        public abstract void EnterPrimarySkill();
        public abstract void EnterUltimateSkill();

        public virtual void ExecuteMovementSkill() { }
        public virtual void ExecutePrimarySkill() { }
        public virtual void ExecuteUltimateSkill() { }

        public virtual void ExitMovementSkill() { }
        public virtual void ExitPrimarySkill() { }
        public virtual void ExitUltimateSkill() { }

    }
}