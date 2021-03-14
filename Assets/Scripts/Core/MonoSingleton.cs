using UnityEngine;

public class MonoSingleton : MonoBehaviour
{
    protected static MonoSingleton _instance;
    public static MonoSingleton Instance => _instance;

    protected void MakeSingleton(MonoSingleton obj)
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = obj;
        }
    }
}
