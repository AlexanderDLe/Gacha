using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Skill;
using RPG.Utility;
using UnityEngine;

namespace RPG.Combat
{
    public class AOEEffect : MonoBehaviour
    {
        public Transform aoeCenterPoint;

        GameObject parentObject;
        LayerMask layerToAffect;
        AOESkill script;
        AOECastType aoeCastType;

        Vector3 originPoint;
        List<EffectItem> effectChain;
        bool repeatChain;
        float repeatDelay = 3f;
        float repeatDelayMin = .1f;

        float radius;

        #region Initialization
        public void Initialize(AOESkill script, GameObject parentObject)
        {
            this.script = script;
            this.parentObject = parentObject;

            InitializeHitbox();
            InitializeCastType();
            InitializeEffectChain();
            InitializeEffectTime();
            InitializeDebug();
            StartCoroutine(ExecuteChain());
        }

        private void InitializeCastType()
        {
            if (script.aoeTargetEnum == AOETargetEnum.SphereCastToPoint)
            {
                aoeCastType = AOECastType.SphereCast;
            }
            else
            {
                aoeCastType = AOECastType.OverlapSphere;
            }
        }
        private void InitializeDebug()
        {
            if (script.debug) Debug();
        }
        private void InitializeHitbox()
        {
            switch (script.aoeTargetEnum)
            {
                case AOETargetEnum.Self:
                    originPoint = parentObject.transform.position;
                    break;
                case AOETargetEnum.SphereCastToPoint:
                    originPoint = parentObject.transform.position;
                    break;
                case AOETargetEnum.ProvidedByPrefab:
                    originPoint = GetProvidedAOEPoint();
                    break;
                case AOETargetEnum.MousePosition:
                    originPoint = GetRaycastMousePoint();
                    break;
                default:
                    break;
            }
        }
        private Vector3 GetProvidedAOEPoint()
        {
            if (!aoeCenterPoint) print("You must provide a Transform to the Prefab.");
            return aoeCenterPoint.position;
        }
        private Vector3 GetRaycastMousePoint()
        {
            RaycastMousePosition raycaster = parentObject.GetComponent<RaycastMousePosition>();
            RaycastHit hit = raycaster.GetRaycastMousePoint(LayerMask.GetMask("Terrain"));
            return hit.point;
        }
        private void InitializeEffectChain()
        {
            effectChain = script.effectChain;
            radius = script.radius;
        }
        private void InitializeEffectTime()
        {
            repeatDelay = Mathf.Max(script.repeatDelay, repeatDelayMin);
            repeatChain = script.repeatChain;
        }
        public void Debug()
        {
            ObjectPooler objectPooler = parentObject.GetComponent<ObjectPooler>();
            DebugObject debugObj = objectPooler.SpawnFromPool("DebugObject").GetComponent<DebugObject>();

            debugObj.Initialize(originPoint, radius, 1);
        }
        #endregion

        #region Execution
        bool shouldWait = false;
        float waitDuration = 0f;

        IEnumerator ExecuteChain()
        {
            foreach (EffectItem effect in effectChain)
            {
                if (shouldWait)
                {
                    yield return new WaitForSeconds(waitDuration);
                    shouldWait = false;
                }
                switch (effect.effectEnum)
                {
                    case EffectEnum.Wait:
                        Executor(new E_Wait(out shouldWait, out waitDuration, effect.duration));
                        break;
                    case EffectEnum.Damage:
                        Executor(new E_Damage(GetHits(effect.layerToAffect), effect.value));
                        break;
                    default:
                        break;
                }
            }
            if (repeatChain)
            {
                yield return new WaitForSeconds(repeatDelay);
                StartCoroutine(ExecuteChain());
            }
        }

        public Collider[] GetHits(LayerMask layer)
        {
            print("In Sphere Cast");
            if (aoeCastType == AOECastType.OverlapSphere)
            {
                return Physics.OverlapSphere(originPoint, radius, layer);
            }
            else if (aoeCastType == AOECastType.SphereCast)
            {
                // We convert from list to array because lists can dynamically add items.
                List<Collider> hitResults = new List<Collider>();

                RaycastHit[] rayHits = Physics.SphereCastAll(originPoint, radius, parentObject.transform.forward, script.distance, layer);

                foreach (RaycastHit rayHit in rayHits)
                {
                    hitResults.Add(rayHit.collider);
                }

                return hitResults.ToArray();
            }
            return null;
        }

        public void Executor(IEffect effect)
        {
            effect.ApplyEffect();
        }

        #endregion
    }
}