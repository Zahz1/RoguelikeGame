using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShrine : Interactable
{
    [SerializeField]
    private int healthRegen;
    [SerializeField]
    private int maxUses;
    [SerializeField]
    private int remainingUses;
    [SerializeField]
    private float cooldown;

    private Coroutine cooldownRoutine;
    private bool cooldownRoutineActive = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

}
