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
            playerStats.Heal((int)(playerStats.GetCurrentMaxHealth() * healthRegen));
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
            playerStats = base.player.GetComponent<CharacterInfo>();
            IsInteractable();
        }
        return true;
    }

    public override void OnTriggerExit()
    {
        base.OnTriggerExit();
        playerStats = null;
    }

    public override bool IsInteractable()
    {
        if (base.IsInteractable()) 
        {
            if(playerStats.GetCurrentHealth() < playerStats.GetCurrentMaxHealth())
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