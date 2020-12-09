using RPG.Attributes;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AnimationEventHandler : MonoBehaviour
    {
        public float probability = .5f;
        public abstract void Initialize(BaseStats baseStats, PlayableCharacter_SO charScript);
        public abstract void InitializeFX();
    }
}