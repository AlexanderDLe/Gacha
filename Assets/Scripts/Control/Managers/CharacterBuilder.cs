using System;
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
        ProjectileLauncher projectileLauncher;
        MeleeAttacker meleeAttacker;
        AOECaster aoeCaster;

        public void LinkReferences(Animator animator, ObjectPooler objectPooler, AudioManager audioManager, RaycastMousePosition raycaster, MeleeAttacker meleeAttacker, ProjectileLauncher projectileLauncher, AOECaster aoeCaster)
        {
            this.animator = animator;
            this.objectPooler = objectPooler;
            this.audioManager = audioManager;
            this.raycaster = raycaster;

            this.meleeAttacker = meleeAttacker;
            this.projectileLauncher = projectileLauncher;
            this.aoeCaster = aoeCaster;
        }

        public CharacterManager BuildCharacter(GameObject char_GO,
            PlayableCharacter_SO char_SO, out GameObject char_PF)
        {
            if (char_SO == null)
            {
                char_PF = null;
                return null;
            }
            CharacterManager charManager;
            Weapon weapon;

            char_GO = BuildCharacterController(char_SO);
            char_PF = BuildAndEquipCharacter(char_SO, out weapon);
            charManager = BuildCharacterManager(char_GO, char_SO, weapon);

            return charManager;
        }

        public GameObject BuildCharacterController(PlayableCharacter_SO char_SO)
        {
            /* The Player will control up to 3 characters. The PLAYER GAME OBJECT will contain up to 3 CHARACTER CONTROLLER GAME OBJECTS which will contain the data for each individual character. The controller game objects will contain the Character Manager, the Skill Managers, and the Base Stats for each character. */

            // 1. Create new Controller Game Object
            GameObject char_GO = new GameObject();

            // 2. Child the Controller_GO to the Player_GO
            char_GO.transform.SetParent(gameObject.transform);
            char_GO.name = char_SO.name + " Controller";
            return char_GO;
        }

        public GameObject BuildAndEquipCharacter(PlayableCharacter_SO char_SO, out Weapon weapon)
        {
            /* The character prefab given to the Character Scriptable Object must have a Weapon Holder script component with a reference to a Hold Weapon Game Object that will be used to spawn the weapon. */

            // 1. Spawn Character
            GameObject char_PF = Instantiate(char_SO.prefab, transform);

            // 2. Get Weapon Holder component
            WeaponHolder weaponHolder = char_PF.GetComponent<WeaponHolder>();

            // 3. Get Hold Weapon Game Object Transform
            Transform holdWeaponTransform = weaponHolder.holdWeapon_GO.transform;

            // 4. Spawn weapon at the Hold Weapon Transform
            weapon = Instantiate(char_SO.weapon, holdWeaponTransform);

            // 5. If Projectile fighter, then add to Object Pool
            AddProjectilesToPool(char_SO);

            char_PF.SetActive(false);
            return char_PF;
        }

        public CharacterManager BuildCharacterManager(GameObject char_GO, PlayableCharacter_SO char_SO, Weapon weapon)
        {
            /* The Character Manager is a script component that will manage and contain data for each of the individual characters. */

            // 1. Add a CharacterManager script to the Controller Game Object
            CharacterManager charManager = char_GO.AddComponent<CharacterManager>();

            // 2. Set up the Character Skill Script
            SkillEventHandler animEventHandler = AddCharEventHandler(char_SO);
            animEventHandler.LinkReferences(audioManager, objectPooler, raycaster, animator, meleeAttacker, projectileLauncher, aoeCaster);

            // 3. Initialize the CharacterManager with the necessary data
            charManager.Initialize(gameObject, char_GO, animator, char_SO, weapon, animEventHandler);
            return charManager;
        }

        private void AddProjectilesToPool(PlayableCharacter_SO char_SO)
        {
            if (char_SO.fightingType == FightTypeEnum.Projectile)
            {
                GameObject projectile = char_SO.projectile.prefab;
                objectPooler.AddToPool(projectile, 10);
            }
        }

        private Characters.SkillEventHandler AddCharEventHandler(PlayableCharacter_SO char_SO)
        {
            PlayableCharEnum charEnum = char_SO.characterEnum;

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