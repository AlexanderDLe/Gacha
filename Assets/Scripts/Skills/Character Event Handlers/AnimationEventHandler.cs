using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AnimationEventHandler : MonoBehaviour
    {
        public float probability = .5f;
        public abstract void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler);
        public abstract void Initialize(BaseStats baseStats, PlayableCharacter_SO charScript);
        public abstract void InitializeSkills();
    }
}