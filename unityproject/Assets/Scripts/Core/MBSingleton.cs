/*
 AUTHOR: "SIDZ"
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: PUT IT IN YOUR OWN NAMESPACE !
//NOTE: TAKE CARE OF THAT MULTITHREADING LOCKS THING !!
//NOTE: DISPLAYS LOG ONLY IN EDITOR 

/// <summary>
/// Basic MonoBehavior Singleton Implementation.
/// <Doesnt do the Multi Threading>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MBSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    /// <summary>
    /// Returns Singleton Instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if(instance ==null)
            {
                instance = FindObjectOfType<T>();
                if(instance ==null)
                {
                    var temp_GameOBject = new GameObject();
                    temp_GameOBject.name = typeof(T).Name+"_Singleton";
                    instance = temp_GameOBject.AddComponent<T>();
                    DontDestroyOnLoad(temp_GameOBject);
                }
            }
            return instance;
        }

    }
    public virtual void OnInit()
    {
#if UNITY_EDITOR
        Debug.LogFormat("A {0} as been Initialized as Singleton.",typeof(T));
#endif
    }
    protected virtual void Awake()
    {
        if(instance == null)
        {
            OnInit();
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat(" Singleton object Already Added..deleting this :{0},GameObjeName:{1}",typeof(T),name);
#endif
            Destroy(this.gameObject);
        }
    }


}
