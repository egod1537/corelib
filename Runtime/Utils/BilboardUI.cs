using UnityEngine;

namespace Corelib.Utils
{
    public class BillboardUI : MonoBehaviour
    {
        private Camera _cameraToLookAt;

        void Start()
        {
            _cameraToLookAt = Camera.main;
        }

        void LateUpdate()
        {
            if (_cameraToLookAt == null)
            {
                _cameraToLookAt = Camera.main;
                if (_cameraToLookAt == null) return;
            }

            transform.rotation = _cameraToLookAt.transform.rotation;
        }
    }
}
