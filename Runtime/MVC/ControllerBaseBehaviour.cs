using UnityEngine;

namespace Corelib.Utils
{
    public class ControllerBaseBehaviour : MonoBehaviour
    {
        [LifecycleInject]
        protected AutoEventSubscriber autoEventSubscriber;
        [LifecycleInject]
        public AutoModelEventSubscriber autoModelEventSubscriber;

        protected virtual void Awake()
        {
            LifecycleInjectionUtil.ConstructLifecycleObjects(this);
        }
        protected virtual void OnEnable()
        {
            LifecycleInjectionUtil.OnEnable(this);
        }
        protected virtual void OnDisable()
        {
            LifecycleInjectionUtil.OnDisable(this);

        }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }
        protected virtual void OnDestroy() { }
    }
}