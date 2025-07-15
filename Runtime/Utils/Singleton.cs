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

                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var go = new GameObject($"[Singleton] {typeof(T)}");
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }
        }
    }
}
