using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region Singleton
    private static UIController instance;
    public static UIController Instance { get { return instance; } }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of UIController found!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    private PlayerController playerController;
    private GameObject ActiveUI = null;
    public GameObject GameUI;
    private GameObject PlayerUI;
    private GameObject InteractUI;
    private GameObject MenuUI;
    private GameObject DiedEndGameUI;
    private GameObject MenuMainPanel;
    private GameObject MenuSettingsPanel;

    private bool isAlive;

    private void Start()
    {
        //Get playerController object for information on health and other values
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
        //Get GameUI GameObject from scene hierarchy, it contains all UI
        //Which will have their references set below
        GameUI = GameObject.Find("GameUI");
        PlayerUI = GameUI.transform.GetChild(0).gameObject;
        MenuUI = GameUI.transform.GetChild(1).gameObject;
        DiedEndGameUI = GameUI.transform.GetChild(2).gameObject;
        InteractUI = GameUI.transform.GetChild(3).gameObject;
        MenuMainPanel = MenuUI.transform.GetChild(0).gameObject;
        MenuSettingsPanel = MenuUI.transform.GetChild(1).gameObject;
        
        //Make sure all required UI is found
        #region CheckForNullUIs
        if (GameUI == null)
        {
            Debug.LogWarning("No GameUI found!");
        }
        if(PlayerUI == null)
        {
            Debug.LogWarning("No PlayerUI found!");
        }
        if(MenuUI == null)
        {
            Debug.LogWarning("No MenuUI found!");
        }
        if(DiedEndGameUI == null)
        {
            Debug.LogWarning("No DiedEndGameUI found!");
        }
        if(InteractUI == null)
        {
            Debug.LogWarning("No InteractUI found!");
        }
        if(MenuMainPanel == null)
        {
            Debug.LogWarning("No MenuMainPanel found!");
        }
        if (MenuSettingsPanel == null)
        {
            Debug.LogWarning("No MenuSettingsPanel found!");
        }
        #endregion

        //Set PlayerUI as only active UI by default
        DisplayPlayerUI();

        GameEvents.Instance.OnMenuTriggerEnter += DisplayMenuUI;
        GameEvents.Instance.OnMenuTriggerExit += CloseMenuUI;
    }

    

    private void SetHealthBar()
    {
        //No health bar implementation to manipulate through GameEvents system yet
    }

    private void DisplayPlayerUI()
    {
        //As the default UI, no need to store previous active UI
        //Iterate through all UI, SetActive to false except PlayerUI which will be true
        foreach(Transform child in GameUI.transform)
        {
            if (child.gameObject.name != "PlayerUI")
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void DisplayInteractUI()
    {
        //InteractUI can and should be displayed on top of the PlayerUI
        InteractUI.SetActive(true);
    }

    private void CloseInteractUI()
    {
        InteractUI.SetActive(false);
    }

    private void DisplayPlayerDiedUI()
    {
        if (!GetActiveUI())
        {
            Debug.LogWarning("No ActiveUI found!");
        }
        if(DiedEndGameUI != null)
        {
            DiedEndGameUI.SetActive(true);
        }
    }

    private void ClosePlayerDiedUI()
    {
        DiedEndGameUI.SetActive(false);
        if(ActiveUI != null)
        {
            ActiveUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No ActiveUI found!");
        }
        ActiveUI = null;
    }

    private void DisplayEndGameUI()
    {
        //No UI built yet
        DisplayPlayerDiedUI();
    }

    private void CloseDisplayEndGameUI()
    {
        DiedEndGameUI.SetActive(false);
        if(ActiveUI != null)
        {
            ActiveUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No ActiveUI found!");
        }
        ActiveUI = null; 
    }

    private void DisplayMenuUI()
    {
        if (!GetActiveUI())
        {
            Debug.LogWarning("No ActiveUI found!");
        }
        if(MenuUI != null)
        {
            MenuUI.SetActive(true);
            if (MenuMainPanel != null)
            {
                MenuMainPanel.SetActive(true);
            }
        }
    }

    private void CloseMenuUI()
    {
        MenuUI.SetActive(false);
        MenuMainPanel.SetActive(false);
        if (ActiveUI != null)
        {
            ActiveUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No ActiveUI found!");
        }
        ActiveUI = null;
    }

    private void DisplayMenuSettingsUI()
    {
        if (!GetActiveUI())
        {
            Debug.LogWarning("No ActiveUI found!");
        }
        if(MenuSettingsPanel != null)
        {
            MenuSettingsPanel.SetActive(true);
        }
    }

    private bool GetActiveUI()
    {
        foreach(Transform child in GameUI.transform)
        {
            if (child.gameObject.activeSelf)
            {
                ActiveUI = child.gameObject;
                ActiveUI.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
