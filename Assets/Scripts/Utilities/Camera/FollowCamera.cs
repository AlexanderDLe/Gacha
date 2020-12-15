using UnityEngine;

namespace RPG.Utility
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target = null;

        void Update()
        {
            transform.position = target.transform.position;
        }
    }
}