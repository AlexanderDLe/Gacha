using UnityEngine;

namespace RPG.Utility
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target = null;

        void LateUpdate()
        {
            transform.position = target.transform.position;
        }
    }
}