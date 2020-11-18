using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class SkillShot : MonoBehaviour
    {
        RaycastMousePosition raycaster = null;
        Vector3 mousePosition;

        public bool isUsingSkillshot = false;
        public bool isUsingCircleAim = false;

        [Header("Skillshot")]
        public Canvas skillshotCanvas;
        public Image skillshot;

        [Header("Circle Aim")]
        public Image rangeCircle;
        public Canvas reticleCanvas;
        public Image reticleAim;
        private Vector3 posUp;
        public float maxAbility2Distance = 5f;

        void Awake()
        {
            raycaster = GetComponent<RaycastMousePosition>();
        }

        private void Start()
        {
            skillshot.enabled = false;
            rangeCircle.enabled = false;
            reticleAim.enabled = false;
        }

        private void Update()
        {
            if (isUsingSkillshot) AimWithSkillshot();
            if (isUsingCircleAim) AimWithCircleAim();
        }

        public void ActivateSkillShotAim()
        {
            skillshot.enabled = true;
            isUsingSkillshot = true;
        }

        public void ActivateCircleAim()
        {
            rangeCircle.enabled = true;
            reticleAim.enabled = true;
            isUsingCircleAim = true;
        }

        public void AimWithSkillshot()
        {
            mousePosition = raycaster.GetRaycastMousePoint().point;
            Quaternion transRot = Quaternion.LookRotation(mousePosition - transform.position);
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            skillshotCanvas.transform.rotation = Quaternion.Lerp(transRot, skillshotCanvas.transform.rotation, 0f);
        }
        public void AimWithCircleAim()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();

            if (hit.collider.gameObject != this.gameObject)
            {
                posUp = new Vector3(hit.point.x, 10f, hit.point.z);
                mousePosition = hit.point;
            }

            var hitPosDir = (hit.point - transform.position).normalized;
            float distance = Vector3.Distance(hit.point, transform.position);
            distance = Mathf.Min(distance, maxAbility2Distance);

            var newHitPos = transform.position + hitPosDir * distance;
            reticleCanvas.transform.position = newHitPos;
        }

        public void DeactivateAim()
        {
            if (isUsingSkillshot)
            {
                skillshot.enabled = false;
                isUsingSkillshot = false;
            }
            if (isUsingCircleAim)
            {
                rangeCircle.enabled = false;
                reticleAim.enabled = false;
                isUsingCircleAim = false;
            }
        }
    }
}