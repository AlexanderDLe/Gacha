using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = "Effect/Create New Effect", order = 0)]
    public class Effect : ScriptableObject
    {
        [SerializeField] string effectName;
    }
}