using UnityEngine;

namespace Corelib.Utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var found = FindObjectsOfType<T>();

                if (found.Length == 0)
                {
                    return null;
                }

                if (found.Length > 1)
                {
                    Debug.LogError($"[Singleton] Multiple instances of {typeof(T)} found in scene.");
                }

                _instance = found[0];
                return _instance;
            }
        }
    }
}
