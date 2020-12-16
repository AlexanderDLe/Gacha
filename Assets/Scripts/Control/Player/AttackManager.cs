using RPG.Attributes;
using RPG.Characters;
using RPG.Combat;
using RPG.Core;
using RPG.Utility;
using UnityEngine;

namespace RPG.Control
{
    public class AttackManager : MonoBehaviour
    {
        Animator animator;
        Stats stats;
        AudioManager audioPlayer;
        ObjectPooler objectPooler;
        RaycastMousePosition raycaster;
        AutoAttack_SO attackScript;
        EffectPackage[] effectPackages;

        private void Start()
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }

        public void LinkReferences(AudioManager audioPlayer, RaycastMousePosition raycaster, Animator animator, ObjectPooler objectPooler)
        {
            this.audioPlayer = audioPlayer;
            this.raycaster = raycaster;
            this.animator = animator;
            this.objectPooler = objectPooler;
        }

        public void Initialize(CharacterManager character, Stats stats)
        {
            PlayableCharacter_SO script = character.script;

            this.stats = stats;
            this.weapon = character.weapon;
            this.fightingType = character.attackType;
            this.hitboxPoint = weapon.hitboxPoint;
            this.attackScript = character.attackScript;

            this.numberOfAutoAttackHits = character.numberOfAutoAttackHits;
            this.autoAttackArray = character.autoAttackArray;
            this.effectPackages = character.effectPackages;

            if (this.fightingType == AttackTypeEnum.Projectile)
            {
                this.projectile_SO = character.projectile_SO;
            }
            if (this.fightingType == AttackTypeEnum.Melee)
            {
                this.autoAttackHitRadiuses = character.autoAttackHitRadiuses;
            }

            InitializeAutoAttackFX(attackScript);
        }

        private void InitializeAutoAttackFX(AutoAttack_SO attackScript)
        {
            this.autoAttackVFX = attackScript.autoAttackVFX;
            this.weakAttackAudio = attackScript.weakAttackAudio;
            this.mediumAttackAudio = attackScript.mediumAttackAudio;
        }

        GameObject[] autoAttackVFX;
        AudioClip[] weakAttackAudio;
        AudioClip[] mediumAttackAudio;
        AttackTypeEnum fightingType;
        Weapon weapon;
        Projectile_SO projectile_SO;
        LayerMask enemyLayer;
        Transform hitboxPoint;

        // Auto Attack Mechanics
        public bool isInAutoAttackState = false;
        public bool canTriggerNextAutoAttack = true;
        public int comboNum = 0;
        public int numberOfAutoAttackHits;
        public string[] autoAttackArray;

        public float[] autoAttackHitRadiuses;
        public float[] autoAttackDamageFractions;

        public void SetIsInAutoAttackState(bool value) => isInAutoAttackState = value;
        public bool GetCanTriggerNextAutoAttack() => canTriggerNextAutoAttack;
        public void SetCanTriggerNextAutoAttack(bool value) => canTriggerNextAutoAttack = value;
        public string[] GetAutoAttackArray() => autoAttackArray;
        public void SetComboNum(int i) => comboNum = i;
        public int GetComboNum() => comboNum;

        public bool IsInAutoAttackAnimation()
        {
            for (int i = 0; i < numberOfAutoAttackHits; i++)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(autoAttackArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        // Animation Event Triggered Functions
        public void Attack1()
        {
            audioPlayer.PlayAudio(AudioEnum.Character, weakAttackAudio);
            int comboIndex = 0;

            SpawnAttackVFXObj(comboIndex);
            AutoAttack(comboIndex);
        }
        public void Attack2()
        {
            audioPlayer.PlayAudio(AudioEnum.Character, mediumAttackAudio);
            int comboIndex = 1;

            SpawnAttackVFXObj(comboIndex);
            AutoAttack(comboIndex);
        }

        private void SpawnAttackVFXObj(int index)
        {
            EffectObject fxObj = objectPooler.SpawnFromPool(autoAttackVFX[index].name).GetComponent<EffectObject>();
            fxObj.Initialize(transform.position, transform.rotation, 1);
        }

        public void AutoAttack(int comboIndex)
        {
            if (fightingType == AttackTypeEnum.Melee) InflictMeleeDamage(comboIndex);
            if (fightingType == AttackTypeEnum.Projectile) ShootProjectile(comboIndex);
        }

        private void ShootProjectile(int comboIndex)
        {
            LayerMask terrainLayer = LayerMask.GetMask("Terrain");
            RaycastHit ray = raycaster.GetRaycastMousePoint(terrainLayer);

            string prefabName = projectile_SO.prefab.name;
            Vector3 projOrigin = transform.position;
            Vector3 projDestination = ray.point;

            float speed = projectile_SO.speed;
            float lifetime = projectile_SO.maxLifeTime;
            LayerMask layerToHarm = LayerMask.GetMask("Enemy");
            EffectPackage package = effectPackages[comboIndex];

            Projectile proj = objectPooler.SpawnFromPool(prefabName).GetComponent<Projectile>();
            proj.Initialize(projOrigin, projDestination, speed, package, lifetime, layerToHarm);
        }

        public void InflictMeleeDamage(int comboIndex)
        {
            float radius = autoAttackHitRadiuses[comboIndex];
            EffectPackage package = effectPackages[comboIndex];

            IAOE_Effect deliverEffects = new IAOE_Execute(hitboxPoint.position, radius, enemyLayer, package);
            deliverEffects.ApplyEffect();
        }

        public void AttackStart() { }
        public void AttackEnd()
        {
            SetCanTriggerNextAutoAttack(true);
            SetIsInAutoAttackState(false);
        }
    }
}