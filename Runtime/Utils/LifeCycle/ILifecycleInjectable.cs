namespace Corelib.Utils
{
    public interface ILifecycleInjectable
    {
        public void OnEnable() { }
        public void Start() { }
        public void OnDisable() { }
    }
}