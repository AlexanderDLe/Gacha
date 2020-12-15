using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable]
    public struct EffectPackage
    {
        [GUIColor(1f, 1f, .3f, 1f)]
        public LayerMask layerToAffect;
        public List<EffectItem> effectItems;
    }
}