using UnityEngine;

public class SingleToScriptable<T> : ScriptableObject where T : Object
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<T>("ScriptableObject/" + typeof(T).Name);
            return instance;
        }
    }
}
