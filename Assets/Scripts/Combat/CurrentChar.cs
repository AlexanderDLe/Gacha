using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CurrentChar : MonoBehaviour
    {
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        private void Start()
        {
            weapon = Instantiate(weapon, handTransform);
        }
    }
}