using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable]
    public class EffectPackage
    {
        public LayerMask layerToAffect;
        public List<EffectItem> effectItems;
    }
}