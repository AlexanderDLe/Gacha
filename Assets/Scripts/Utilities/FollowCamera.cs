using UnityEngine;

namespace RPG.Core
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