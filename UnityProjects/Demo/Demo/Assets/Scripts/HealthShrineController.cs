using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShrineController : Interactable
{
    [SerializeField]
    private float healthRegen = 0.25f; //25% health regen
    [SerializeField]
    private float cooldownTime = 5f; 

    private CharacterInfo playerStats;
    private PlayerController playerController;

    private Coroutine cooldownRoutine = null;
    private bool cooldownRoutineActive = false;

    public override void Start()
    {
        base.Start();
        base.uses = 3;
        base.interactType = InteractableType.HealthShrine;
    }

    public override void Update()
    {
        if (playerStats != null)
        {
            IsInteractable();
        }
    }

    public override void Interact()
    {
        if (base.isInteractable && playerStats != null)
        {
            playerController.Heal((int)(playerStats.currentStats.MaxHealth * healthRegen));
            base.Interact();
            if (base.uses > 0) {
                cooldownRoutine = StartCoroutine(Cooldown());
            }
        }
    }

    public override bool OnTriggerEnter(Collider other)
    {
        if (base.OnTriggerEnter(other))
        {
            playerController = base.player.GetComponent<PlayerController>();
            playerStats = base.player.GetComponent<CharacterInfo>();
            IsInteractable();
        }
        return true;
    }

    public override void OnTriggerExit()
    {
        base.OnTriggerExit();
        playerStats = null;
        playerController = null;
    }

    public override bool IsInteractable()
    {
        if (base.IsInteractable()) 
        {
            if(playerStats.currentStats.CurrentHealth < playerStats.currentStats.MaxHealth)
            {
                base.outline.enabled = true;
                base.isInteractable = true;
                GameEvents.Instance.InteractionUITriggerEnter();
                return true;
            }   
        }
        else
        {
            base.outline.enabled = false;
            base.isInteractable = false;
            GameEvents.Instance.InteractionUITriggerExit();
        }
        return false;
    }

    IEnumerator Cooldown()
    {
        cooldownRoutineActive = true;
        yield return new WaitForSeconds(cooldownTime);
        base.isUsed = false;
        cooldownRoutineActive = false;
    }
}
