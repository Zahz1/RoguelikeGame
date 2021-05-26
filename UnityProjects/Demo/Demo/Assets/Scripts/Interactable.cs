using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //Distance player needs to be to an interactable
    //to interact
    private float radius = 2f;
    public bool isUsed = false;


    //Will reveal interactable area
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
