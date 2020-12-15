using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
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
        AOECastEnum aoeCastType;

        Vector3 originPoint;
        List<AOEPackageItem> aoePackageChain;

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
            StartCoroutine(ExecuteAOEChain());
        }
        private void InitializeCastType()
        {
            if (script.aoeTargetEnum == AOETargetEnum.SphereCastToPoint)
            {
                aoeCastType = AOECastEnum.SphereCast;
            }
            else
            {
                aoeCastType = AOECastEnum.OverlapSphere;
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
            aoePackageChain = script.aoePackageChain;
            radius = script.radius;
        }
        private void InitializeEffectTime()
        {
            repeatDelay = Mathf.Max(script.repeatDelay, repeatDelayMin);
            repeatChain = script.repeatChain;
        }
        public void Debug()
        {
            ObjectPooler objectPooler = parentObject.GetComponent<StateManager>().objectPooler;
            DebugObject debugObj = objectPooler.SpawnFromPool(objectPooler.debugObject.name).GetComponent<DebugObject>();

            debugObj.Initialize(originPoint, parentObject.transform.rotation, radius, 2);

            if (aoeCastType == AOECastEnum.SphereCast)
            {
                debugObj.MoveDistance(script.distance);
            }
        }
        #endregion

        #region Execution

        IEnumerator ExecuteAOEChain()
        {
            bool shouldWait = false;
            float waitDuration = 0f;

            foreach (AOEPackageItem effect in aoePackageChain)
            {
                if (shouldWait)
                {
                    yield return new WaitForSeconds(waitDuration);
                    shouldWait = false;
                }
                switch (effect.aoePackageEnum)
                {
                    case AOEPackageEnum.Wait:
                        Executor(new IAOE_Wait(out shouldWait, out waitDuration, effect.duration));
                        break;
                    case AOEPackageEnum.Executable:
                        Executor(new IAOE_Execute(GetHits(effect.effectPackage.layerToAffect), effect.effectPackage));
                        break;
                    default:
                        break;
                }
            }
            if (repeatChain)
            {
                yield return new WaitForSeconds(repeatDelay);
                StartCoroutine(ExecuteAOEChain());
            }
        }

        public Collider[] GetHits(LayerMask layer)
        {
            print("In Sphere Cast");
            if (aoeCastType == AOECastEnum.OverlapSphere)
            {
                return Physics.OverlapSphere(originPoint, radius, layer);
            }
            else if (aoeCastType == AOECastEnum.SphereCast)
            {
                return GetSphereCastAllHits(layer);
            }
            return null;
        }

        private Collider[] GetSphereCastAllHits(LayerMask layer)
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

        public void Executor(IAOE_Effect effect)
        {
            effect.ApplyEffect();
        }
        #endregion
    }
}