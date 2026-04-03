using UnityEngine;

public class SingleToMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
    }
}
