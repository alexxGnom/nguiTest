//************************************************************************************************
// http://wiki.unity3d.com/index.php?title=Singleton#Generic_Based_Singleton_for_MonoBehaviours
//************************************************************************************************  

using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T _instance = null;
    private static object lockObject = new object();

    public static UnityAction OnCreateInstance;

    public static T Instance
    {
        get
        {
            // Instance requiered for the first time, we look for it
            if (_instance == null)
            {
                lock (lockObject)
                {
                    //Search for existing objects
                    UnityEngine.Object[] result = FindObjectsOfType(typeof(T));

                    if (result.Length > 0)
                    {
                        _instance = result[0] as T;
                        if (result.Length > 1)
                            Debug.LogError(String.Format("[MonoSingleton] Something went really wrong - there should never be more than 1 {0}! Reopening the scene might fix it. ", typeof(T).ToString()));
                    }
                    // Object not found, inform that we are trying to aquire not created instance
                    else
                    {
#if UNITY_EDITOR
                        Debug.LogWarning("[MonoSingleton] You are trying to get not created instance of " + typeof(T).ToString());
#endif
                    }
                }
            }
            return _instance;
        }
    }

    public static bool HasInstance
    {
        get { return _instance != null; }
    }

    public static T CreateInstance(bool dontDestroyOnLoad = false, GameObject fromPrefab = null)
    {
        if (_instance == null)
        {
            lock (lockObject)
            {
                //Search for existing objects
                UnityEngine.Object[] result = FindObjectsOfType(typeof(T));


                if (result.Length > 0)
                {
                    _instance = result[0] as T;
                    Debug.LogWarning(String.Format("[MonoSingleton]: You trying to create instance of {0}, but it is already existing!", typeof(T)), _instance.gameObject);

                    if (result.Length > 1)
                        Debug.LogError(String.Format("[MonoSingleton]: Something went really wrong - there should never be more than 1 {0}! Reopening the scene might fix it. ", typeof(T)));
                }
                else if(fromPrefab == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                else
                {
                    _instance = Instantiate(fromPrefab).GetComponent<T>();
                }

                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(_instance);
            }
        }

        return _instance;
    }


    // This function is called when the instance is used the first time
    // Put all the initializations you need here, as you would do in Awake
    protected virtual void Init()
    {
    }


    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_instance != null)
            {
                _instance.Init();
                Debug.Log("<color=blue>[Initialization]:</color> " + typeof(T).Name + " created");
            }
        }
        else if (_instance == this as T)
        {
            _instance.Init();
            print("<color=blue>[Initialization]:</color> " + typeof(T).Name + " created");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Make sure the instance isn't referenced anymore when the object is destroyed, just in case.
    protected virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            _instance = null;
        }
    }

    // Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
        _instance = null;
    }
}
