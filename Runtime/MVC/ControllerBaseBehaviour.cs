using UnityEngine;

namespace Corelib.Utils
{
    public class ControllerBaseBehaviour : MonoBehaviour
    {
        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }
        protected virtual void OnDestroy() { }
    }
}