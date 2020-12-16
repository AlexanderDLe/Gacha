using RPG.Attributes;
using RPG.Characters;
using UnityEngine;

namespace RPG.Control
{
    public class InitializeManager : MonoBehaviour
    {
        CharacterManager character = null;

        public void CurrentCharacter(CharacterManager character)
        {
            this.character = character;
        }

        public void CharacterPrefab(out GameObject currentCharPrefab, GameObject[] charPrefabs, int charIndex)
        {
            currentCharPrefab = charPrefabs[charIndex];
            currentCharPrefab.SetActive(true);
        }

        public void CharacterStats(out Stats currentBaseStats, out string currCharName, out Sprite currCharImage)
        {
            currentBaseStats = character.stats;
            currCharName = character.name;
            currCharImage = character.image;
        }

        public void CharacterSkills(out SkillManager movementSkill, out SkillManager primarySkill, out SkillManager ultimateSkill)
        {
            movementSkill = character.movementSkill;
            primarySkill = character.primarySkill;
            ultimateSkill = character.ultimateSkill;
        }

        public void CharacterAnimation(Animator animator)
        {
            animator.avatar = character.avatar;
            if (character.animatorController != null)
            {
                animator.runtimeAnimatorController = character.animatorController;
            }
            animator.Rebind();
        }
    }
}