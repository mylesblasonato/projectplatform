using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(this); //Or GameObject as appropriate
            return;
        }
        Instance = this.gameObject.GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}
