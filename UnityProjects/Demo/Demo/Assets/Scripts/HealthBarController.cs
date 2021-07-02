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
    private CharacterInfo playerStats;

    private void Start()
    {
        slider = GetComponent<Slider>();
        
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerStats = playerController.GetComponent<CharacterInfo>();
        

        Invoke("UpdateHealthBar", 0f);

        //Instead of calling update every frame to check for health changes, we will
        //Have subscribed methods to events that will only be called when said events
        //Are triggered by other scripts

        //For when max health increases or decreases
        GameEvents.Instance.OnPlayerMaxHealthChangeEnter += SetMaxHealth;
        GameEvents.Instance.OnPlayerMaxHealthChangeEnter += UpdateHealthBar;
        //For when current health increases or decreases
        GameEvents.Instance.OnPlayerHealingTriggerEnter += AddHealth;
        GameEvents.Instance.OnPlayerDamageTriggerEnter += RemoveHealth;
        GameEvents.Instance.OnPlayerHealingTriggerEnter += UpdateHealthBar;
        GameEvents.Instance.OnPlayerDamageTriggerEnter += UpdateHealthBar;

    }

    private void AddHealth()
    {
        newHealth = playerStats.currentStats.CurrentHealth;
        healthChange = newHealth - (int)slider.value;
        slider.value += healthChange;
        UpdateHealthBar();
    }

    private void RemoveHealth()
    {
        newHealth = playerStats.currentStats.CurrentHealth;
        healthChange = (int)slider.value - newHealth;
        slider.value -= healthChange;
        UpdateHealthBar();
    }

    private void SetMaxHealth()
    {
        slider.maxValue = playerStats.currentStats.MaxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        slider.maxValue = playerStats.currentStats.MaxHealth;
        slider.value = playerStats.currentStats.CurrentHealth;
        currentHealth = playerStats.currentStats.CurrentHealth.ToString();
        maxHealth = playerStats.currentStats.MaxHealth.ToString();
        healthBarString = currentHealth + "/" + maxHealth;
        text.text = healthBarString;
    }
}
