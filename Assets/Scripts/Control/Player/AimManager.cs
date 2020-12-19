using RPG.Characters;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Control
{
    public class AimManager : MonoBehaviour
    {
        public Canvas skillshotCanvas = null;
        public Canvas reticleCanvas = null;
        public Image skillshotImage = null;
        public Image rangeImage = null;
        public Image reticleImage = null;

        public RaycastMousePosition raycaster = null;
        public CharacterManager currentCharacter = null;
        private Vector3 mousePosition = Vector3.zero;
        public bool skillshotAimingActive = false;
        public bool rangeshotAimingActive = false;
        public float maxRangeShotDistance = 5f;

        public string SKILLSHOT = "SKILLSHOT";
        public string RANGESHOT = "RANGESHOT";

        public void LinkReferences(RaycastMousePosition raycaster)
        {
            this.raycaster = raycaster;
        }

        private void Update()
        {
            if (skillshotAimingActive) AimWithSkillshot();
            if (rangeshotAimingActive) AimWithRangeshot();
        }

        public void Initialize(CharacterManager character)
        {
            this.currentCharacter = character;
            SetAimImagesEnabled(false);
        }

        public bool IsAimingActive()
        {
            return skillshotAimingActive || rangeshotAimingActive;
        }
        public void CancelAiming()
        {
            currentCharacter.CancelSkillAiming();

            skillshotAimingActive = false;
            rangeshotAimingActive = false;
            SetAimImagesEnabled(false);
        }
        public void SetAimImagesEnabled(bool value)
        {
            skillshotImage.enabled = value;
            rangeImage.enabled = value;
            reticleImage.enabled = value;
        }
        public void InitializeAimImage(SkillManager skill)
        {
            if (skill.requiresSkillShot)
            {
                skillshotImage.sprite = skill.skillShotImage;
            }
            if (skill.requiresRangeShot)
            {
                rangeImage.sprite = skill.rangeImage;
                reticleImage.sprite = skill.reticleImage;
            }
        }
        public void SetAimingEnabled(string aimType, bool value)
        {
            if (aimType == "SKILLSHOT")
            {
                skillshotImage.enabled = value;
                skillshotAimingActive = value;
            }
            if (aimType == "RANGESHOT")
            {
                rangeImage.enabled = value;
                reticleImage.enabled = value;
                rangeshotAimingActive = value;
            }
        }
        public void AimWithSkillshot()
        {
            mousePosition = raycaster.GetRaycastMousePoint().point;
            Quaternion transRot = Quaternion.LookRotation(mousePosition - transform.position);
            transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
            skillshotCanvas.transform.rotation = Quaternion.Lerp(transRot, skillshotCanvas.transform.rotation, 0f);
        }
        public void AimWithRangeshot()
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();

            var hitPosDir = (hit.point - transform.position).normalized;
            float distance = Vector3.Distance(hit.point, transform.position);
            distance = Mathf.Min(distance, maxRangeShotDistance);

            var newHitPos = transform.position + hitPosDir * distance;
            reticleCanvas.transform.position = newHitPos;
        }
    }
}