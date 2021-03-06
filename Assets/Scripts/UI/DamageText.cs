﻿using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshPro textMesh = default;

        public void Initialize(int damage)
        {
            textMesh.SetText(damage.ToString());
        }
    }
}