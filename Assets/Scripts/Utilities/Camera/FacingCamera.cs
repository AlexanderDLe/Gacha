using UnityEngine;

namespace RPG.Utility
{
    public class FacingCamera : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}