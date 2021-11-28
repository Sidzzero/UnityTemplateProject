using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple Serivce Locator for holding all the Monobehaviour which can be
/// quried from here.
/// Prefer using this with a singleton so they will stay across Scenes !
/// (, else a plane class will do...remember to remove OnInit() if you dont use it and init the dicAvailableMonoBehaviour(line 32) somewhere before use  MBSingleton!>
/// </summary>
public sealed class ServiceLocator :MBSingleton<ServiceLocator>
{
    private Dictionary<string, Component> dicAvailableMonoBehaviour;


    /// <summary>
    /// Checks If Component is already present !
    /// </summary>
    /// <param name="a_strTypeName"></param>
    /// <returns></returns>
    private bool HasMBAlready(string a_strTypeName)
    {
        return dicAvailableMonoBehaviour.ContainsKey(a_strTypeName);
    }

    /// <summary>
    /// Using this to initiliza the Dictonary !
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
        dicAvailableMonoBehaviour = new Dictionary<string, Component>();
    }

    /// <summary>
    /// Add a component to the Service
    /// </summary>
    /// <param name="a_Component"></param>
    public void AddService(Component a_Component)
    {
        if(HasMBAlready(a_Component.GetType().Name) ==false)
        {
            dicAvailableMonoBehaviour.Add(a_Component.GetType().Name, a_Component);
            a_Component.transform.parent = transform;
        }
        else
        {

#if UNITY_EDITOR
        Debug.LogErrorFormat("The Compnoent is already added {0}", a_Component.GetType().Name);
#endif
        }
    }

    /// <summary>
    /// Remove a component to the Service
    /// </summary>
    /// <param name="a_Component"></param>
    /// <param name="a_bDestroy"></param>
    public void RemoveService(Component a_Component, bool a_bDestroy = false) 
    {
        if (HasMBAlready(a_Component.GetType().Name) == true)
        {
            dicAvailableMonoBehaviour.Remove(a_Component.GetType().Name);
            if(a_bDestroy == false)
            {
             a_Component.transform.parent = null;
            }
            else
            {
                Destroy(a_Component.gameObject);
            }
        }
        else
        {

#if UNITY_EDITOR
            Debug.LogErrorFormat("The Compnoent is not present added {0}", a_Component.GetType().Name);
#endif
        }
    }

    /// <summary>
    /// Returns the Service from the stored place !
    /// </summary>
    /// <typeparam name="T">Type of Componnet required !</typeparam>
    /// <returns></returns>
    public T GetService<T>() where T:Component
    {
        var strTypeName = typeof(T).Name;
        if(HasMBAlready(strTypeName) ==false)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("The Service {0} not present !", strTypeName);
#endif
            return null;
        }

        return dicAvailableMonoBehaviour[strTypeName] as T;

    }
}
//Source: http://www.unitygeek.com/unity_c_singleton/
//Solve most of my problems !