﻿using UnityEngine;

namespace RPG.Core
{
    public class RaycastMousePosition : MonoBehaviour
    {
        public static Camera cam = null;

        private void Awake()
        {
            if (!cam) cam = Camera.main;
        }

        public RaycastHit GetRaycastMousePoint()
        {
            // Raycast using mouse position
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            return hit;
        }

        private static Ray GetMouseRay()
        {
            return cam.ScreenPointToRay(Input.mousePosition);
        }
    }
}