using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AOECaster : MonoBehaviour
    {
        ObjectPooler objectPooler = null;

        private void Awake()
        {
            objectPooler = GameObject.FindWithTag("ObjectPooler").GetComponent<ObjectPooler>();
        }

        public void Cast()
        {

        }
    }
}