using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    private int newHealth;
    private int healthChange;

    private string currentHealth;
    private string maxHealth;
    private string healthBarString;

    private Slider slider;
    public TextMeshProUGUI text;
    private PlayerController playerController;
    private CharacterInfo characterInfo;

    private void Start()
    {
        slider = GetComponent<Slider>();
        
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        characterInfo = playerController.GetComponent<CharacterInfo>();
        

        Invoke("UpdateHealthBar", 0f);

        //Instead of calling update every frame to check for health changes, we will
        //Have subscribed methods to events that will only be called when said events
        //Are triggered by other scripts

        //For when max health increases or decreases
        GameEvents.Instance.OnPlayerMaxHealthIncreaseEnter += SetMaxHealth;
        GameEvents.Instance.OnPlayerMaxHealthDecreaseEnter += SetMaxHealth;
        GameEvents.Instance.OnPlayerMaxHealthIncreaseEnter += UpdateHealthBar;
        GameEvents.Instance.OnPlayerMaxHealthIncreaseEnter += UpdateHealthBar;
        //For when current health increases or decreases
        GameEvents.Instance.OnPlayerHealingTriggerEnter += AddHealth;
        GameEvents.Instance.OnPlayerDamageTriggerEnter += RemoveHealth;
        GameEvents.Instance.OnPlayerHealingTriggerEnter += UpdateHealthBar;
        GameEvents.Instance.OnPlayerDamageTriggerEnter += UpdateHealthBar;

    }

    private void AddHealth()
    {
        newHealth = characterInfo.GetCurrentHealth();
        healthChange = newHealth - (int)slider.value;
        slider.value += healthChange;
        UpdateHealthBar();
    }

    private void RemoveHealth()
    {
        newHealth = characterInfo.GetCurrentHealth();
        healthChange = (int)slider.value - newHealth;
        slider.value -= healthChange;
        UpdateHealthBar();
    }

    private void SetMaxHealth()
    {
        slider.maxValue = characterInfo.GetCurrentMaxHealth();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        slider.maxValue = characterInfo.GetCurrentMaxHealth();
        slider.value = characterInfo.GetCurrentHealth();
        currentHealth = characterInfo.GetCurrentHealth().ToString();
        maxHealth = characterInfo.GetCurrentHealth().ToString();
        healthBarString = currentHealth + "/" + maxHealth;
        text.text = healthBarString;
    }
}
