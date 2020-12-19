using RPG.Utility;
using UnityEngine;

namespace RPG.Core
{
    public class RaycastMousePosition : MonoBehaviour
    {
        LayerMask terrainLayer;
        ObjectPooler objectPooler = null;
        public static Camera cam = null;

        [Header("Debug")]
        [SerializeField] bool isDebugging = false;
        [SerializeField] GameObject debugObject = null;

        private float maxDistance = 1000f;
        static private Vector3 pointDepth = new Vector3(0, 0, 10f);

        private void Awake()
        {
            if (!cam) cam = Camera.main;
            terrainLayer = LayerMask.GetMask("Terrain");
            objectPooler = GameObject.FindWithTag("DebugPooler").GetComponent<ObjectPooler>();
        }
        public void Start()
        {
            objectPooler.AddToPool(debugObject, 10);
        }

        public void RotateObjectTowardsMousePosition(GameObject gameObject)
        {
            RaycastHit hit = GetRaycastMousePoint(terrainLayer);
            Vector3 destination = GetRotationWithoutY(gameObject, ref hit);
            gameObject.transform.rotation = Quaternion.LookRotation(destination);
            if (isDebugging) RunDebug(gameObject, hit.point);
        }

        public void RotateObjectTowardsMousePosition(GameObject gameObject, RaycastHit hit)
        {
            Vector3 destination = GetRotationWithoutY(gameObject, ref hit);
            gameObject.transform.rotation = Quaternion.LookRotation(destination);
            if (isDebugging) RunDebug(gameObject, hit.point);
        }

        private static Vector3 GetRotationWithoutY(GameObject gameObject, ref RaycastHit hit)
        {
            Vector3 hitpoint = new Vector3(hit.point.x, 0, hit.point.z);
            Vector3 objectPos = gameObject.transform.position;
            Vector3 pospoint = new Vector3(objectPos.x, 0, objectPos.z);
            Vector3 destination = (hitpoint - pospoint).normalized;
            return destination;
        }

        private void RunDebug(GameObject gameObject, Vector3 destination)
        {
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward, Color.red, 3f);
            GameObject obj = objectPooler.SpawnFromPool(debugObject.name);
            DebugObject debugObj = obj.GetComponent<DebugObject>();
            debugObj.Initialize(destination, gameObject.transform.rotation, .5f, 3);
        }

        public RaycastHit GetRaycastMousePoint()
        {
            // Raycast using mouse position
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, maxDistance);
            return hit;
        }
        public RaycastHit GetRaycastMousePoint(LayerMask layerMask)
        {
            // Raycast using mouse position with specified Layer Mask
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, maxDistance, layerMask);
            return hit;
        }

        private static Ray GetMouseRay()
        {
            return cam.ScreenPointToRay(Input.mousePosition + pointDepth);
        }
    }
}