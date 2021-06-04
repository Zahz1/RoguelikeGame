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

    #region UI Triggers
    //For Opening menu
    public event Action OnMenuTriggerEnter;
    public void MenuTriggerEnter()
    {
        if(OnMenuTriggerEnter != null)
        {
            OnMenuTriggerEnter();
        }
    }

    //For Closing menu
    public event Action OnMenuTriggerExit;
    public void MenuTriggerExit()
    {
        if(OnMenuTriggerExit != null)
        {
            OnMenuTriggerExit();
        }
    }

    //For Opening InteractionUI
    public event Action OnInteractionUITriggerEnter;
    public void InteractionUITriggerEnter()
    {
        if(OnInteractionUITriggerEnter != null)
        {
            OnInteractionUITriggerEnter();
        }
    }

    //For Closing InteractionUI
    public event Action OnInteractionUITriggerExit;
    public void InteractionUITriggerExit()
    {
        if(OnInteractionUITriggerExit != null)
        {
            OnInteractionUITriggerExit();
        }
    }

    //For Interacting with interactable
    public event Action OnInteractionTriggerEnter;
    public void InteractionTriggerEnter()
    {
        if(OnInteractionTriggerEnter != null)
        {
            OnInteractionTriggerEnter();
        }
    }

    //Triggers if player dies
    public event Action OnPlayerDiedTriggerEnter;
    public void PlayerDiedTriggerEnter()
    {
        if(OnPlayerDiedTriggerEnter != null)
        {
            OnPlayerDiedTriggerEnter();
        }
    }
    #endregion

    #region Health Triggers
    //Trigger if player max health increases
    public event Action OnPlayerMaxHealthIncreaseEnter;
    public void PlayerMaxHealthIncreaseEnter()
    {
        if(OnPlayerMaxHealthIncreaseEnter != null)
        {
            OnPlayerMaxHealthIncreaseEnter();
        }
    }

    //Trigger if player max health decreases
    public event Action OnPlayerMaxHealthDecreaseEnter;
    public void PlayerMaxHealthDecreaseEnter()
    {
        if (OnPlayerMaxHealthDecreaseEnter != null)
        {
            OnPlayerMaxHealthDecreaseEnter();
        }
    }

    //Trigger if player current health increases (regen, etc.)
    public event Action OnPlayerHealingTriggerEnter;
    public void PlayerHealingTriggerEnter()
    {
        if (OnPlayerHealingTriggerEnter != null)
        {
            OnPlayerHealingTriggerEnter();
        }
    }

    //Trigger if player current health decreases (damage, etc.)
    public event Action OnPlayerDamageTriggerEnter;
    public void PlayerDamageTriggerEnter()
    {
        if(OnPlayerDamageTriggerEnter != null)
        {
            OnPlayerDamageTriggerEnter();
        }
    }
    #endregion

    #region Interaction Events
    //Trigger if player interacts with interactable
    public event Action<int, int> OnPlayerInteractTriggerEnter;
    public void PlayerInteractTriggerEnter(int playerID, int interactableID)
    {
        if(OnPlayerInteractTriggerEnter != null)
        {
            OnPlayerInteractTriggerEnter(playerID, interactableID);
        }
    }
    
    #endregion

}
