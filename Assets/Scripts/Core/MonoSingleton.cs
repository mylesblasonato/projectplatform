using UnityEngine;

public class MonoSingleton : MonoBehaviour
{
    private static MonoSingleton _instance;
    public static MonoSingleton Instance => _instance;

    protected void MakeSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
