using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
       private void update()
    {    
        transform.position = Input.mousePosition;
    }
}
