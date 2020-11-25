using RPG.Characters;
using RPG.Combat;
using UnityEngine;

namespace RPG.Control
{
    public class CharacterBuilder : MonoBehaviour
    {
        Animator animator = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
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

            char_GO = BuildCharacterController(char_SO);
            charManager = BuildCharacterManager(char_GO, char_SO);
            char_PF = BuildAndEquipCharacter(char_SO);

            return charManager;
        }

        public GameObject BuildCharacterController(PlayableCharacter_SO char_SO)
        {
            GameObject char_GO = new GameObject();
            char_GO.transform.SetParent(gameObject.transform);
            char_GO.name = char_SO.name + " Controller";
            return char_GO;
        }

        public CharacterManager BuildCharacterManager(GameObject char_GO, PlayableCharacter_SO char_SO)
        {
            CharacterManager charManager = char_GO.AddComponent<CharacterManager>();
            charManager.Initialize(gameObject, char_GO, animator, char_SO);
            return charManager;
        }

        public GameObject BuildAndEquipCharacter(PlayableCharacter_SO char_SO)
        {
            /* The character prefab given to the Character Scriptable Object must have a Weapon Holder script component with a reference to a Hold Weapon Game Object that will be used to spawn the weapon. */

            GameObject char_PF = Instantiate(char_SO.prefab, transform);
            WeaponHolder weaponHolder = char_PF.GetComponent<WeaponHolder>();
            GameObject holdWeapon_GO = weaponHolder.holdWeapon_GO;
            Weapon char_Weapon = Instantiate(char_SO.weapon, holdWeapon_GO.transform);
            char_PF.SetActive(false);
            return char_PF;
        }
    }
}