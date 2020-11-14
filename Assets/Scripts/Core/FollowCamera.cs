using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] GameObject target = null;
        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = target.transform.position;
        }
    }
}