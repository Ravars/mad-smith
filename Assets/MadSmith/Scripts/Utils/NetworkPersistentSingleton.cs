using MadSmith.Scripts.Utils;

namespace Utils
{
    public class NetworkPersistentSingleton<T> : NetworkSingleton<T> where T : NetworkSingleton<T>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
    }
}