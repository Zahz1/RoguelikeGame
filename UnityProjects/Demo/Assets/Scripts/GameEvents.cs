using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Singleton
    private static GameEvents instance;
    public static GameEvents Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of GameEvents found!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public event Action OnMenuTriggerEnter;
    public void MenuTriggerEnter()
    {
        if(OnMenuTriggerEnter != null)
        {
            OnMenuTriggerEnter();
        }
    }

    public event Action OnMenuTriggerExit;
    public void MenuTriggerExit()
    {
        if(OnMenuTriggerExit != null)
        {
            OnMenuTriggerExit();
        }
    }
}
