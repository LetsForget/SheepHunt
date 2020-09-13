using UnityEngine;

namespace GameProcess
{
    public class Raycaster : MonoBehaviour
    {
        public static bool RaycastMousePosFor<T>(out T result) where T : MonoBehaviour
        {
            Ray ray = _instance._mainCamera.ScreenPointToRay(Input.mousePosition);
            bool catched = Physics.Raycast(ray, out RaycastHit hit);

            if (catched)
            {
                if (hit.collider.TryGetComponent(out result))
                {
                    return true;
                }
                else
                {
                    result = hit.collider.GetComponentInParent<T>();
                    
                    if (result == null)
                    {
                        result = hit.collider.GetComponentInChildren<T>();
                    }

                    return result != null;
                }
            }

            result = null;
            return false;
        }

        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("More than one raycaster!");
                Destroy(this);
                return;
            }

            _instance = this;
        }

        private static Raycaster _instance;
        [SerializeField] private Camera _mainCamera;
    }
}