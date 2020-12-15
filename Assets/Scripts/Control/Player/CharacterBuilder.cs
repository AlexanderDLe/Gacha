using RPG.Characters;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    public class CharacterBuilder : MonoBehaviour
    {
        Animator animator = null;
        ObjectPooler objectPooler = null;
        AudioManager audioManager = null;
        RaycastMousePosition raycaster = null;

        public void LinkReferences(Animator animator, ObjectPooler objectPooler, AudioManager audioManager, RaycastMousePosition raycaster)
        {
            this.animator = animator;
            this.objectPooler = objectPooler;
            this.audioManager = audioManager;
            this.raycaster = raycaster;
        }

        public CharacterManager BuildCharacter(GameObject charController,
            PlayableCharacter_SO charScript, out GameObject charPrefab)
        {
            if (charScript == null)
            {
                charPrefab = null;
                return null;
            }
            CharacterManager charManager;
            Weapon weapon;

            charController = BuildCharacterController(charScript);
            charPrefab = BuildAndEquipCharacter(charScript, out weapon);
            charManager = BuildCharacterManager(charController, charScript, weapon);

            return charManager;
        }

        public GameObject BuildCharacterController(PlayableCharacter_SO charScript)
        {
            /* The Player will control up to 3 characters. The PLAYER GAME OBJECT will contain up to 3 CHARACTER CONTROLLER GAME OBJECTS which will contain the data for each individual character. The controller game objects will contain the Character Manager, the Skill Managers, and the Base Stats for each character. */

            // 1. Create new Controller Game Object
            GameObject charController = new GameObject();

            // 2. Child the Controller_GO to the Player_GO
            charController.transform.SetParent(gameObject.transform);
            charController.name = charScript.name + " Controller";
            return charController;
        }

        public GameObject BuildAndEquipCharacter(PlayableCharacter_SO charScript, out Weapon weapon)
        {
            /* The character prefab given to the Character Scriptable Object must have a Weapon Holder script component with a reference to a Hold Weapon Game Object that will be used to spawn the weapon. */

            // 1. Spawn Character
            GameObject char_PF = Instantiate(charScript.prefab, transform);

            // 2. Get Weapon Holder component
            WeaponHolder weaponHolder = char_PF.GetComponent<WeaponHolder>();

            // 3. Get Hold Weapon Game Object Transform
            Transform holdWeaponTransform = weaponHolder.holdWeapon_GO.transform;

            // 4. Spawn weapon at the Hold Weapon Transform
            weapon = Instantiate(charScript.weapon, holdWeaponTransform);

            // 5. If Projectile fighter, then add to Object Pool
            AddObjectsToPool(charScript);

            char_PF.SetActive(false);
            return char_PF;
        }

        public CharacterManager BuildCharacterManager(GameObject charController, PlayableCharacter_SO charScript, Weapon weapon)
        {
            /* The Character Manager is a script component that will manage and contain data for each of the individual characters. */

            // 1. Add a CharacterManager script to the Controller Game Object
            CharacterManager charManager = charController.AddComponent<CharacterManager>();

            // 2. Set up the Character Skill Script
            SkillEventHandler animEventHandler = AddCharEventHandler(charScript);
            animEventHandler.LinkReferences(audioManager, objectPooler, raycaster, animator);

            // 3. Initialize the CharacterManager with the necessary data
            charManager.Initialize(gameObject, charController, animator, charScript, weapon, animEventHandler);
            return charManager;
        }

        private void AddObjectsToPool(PlayableCharacter_SO charScript)
        {
            // Add attack VFX to pool
            GameObject[] attackVFXArr = charScript.autoAttack_SO.autoAttackVFX;
            foreach (GameObject attackVFX in attackVFXArr)
            {
                objectPooler.AddToPool(attackVFX, 3);
            }

            // If projectile user, then add projectiles to pool
            if (charScript.attackType == AttackTypeEnum.Projectile)
            {
                ProjectileAttack_SO proj_SO = charScript.autoAttack_SO as ProjectileAttack_SO;

                GameObject projectile = proj_SO.projectile.prefab;
                objectPooler.AddToPool(projectile, 10);
            }
        }

        private Characters.SkillEventHandler AddCharEventHandler(PlayableCharacter_SO charScript)
        {
            PlayableCharEnum charEnum = charScript.characterEnum;

            switch (charEnum)
            {
                case (PlayableCharEnum.MC):
                    return gameObject.AddComponent<MCEventHandler>();
                case (PlayableCharEnum.Howl):
                    return gameObject.AddComponent<HowlEventHandler>();
                default:
                    return null;
            }
        }
    }
}