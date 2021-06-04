using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetPositioning : MonoBehaviour
{
    public GameObject PlayerHead;
    public Camera myCamera;
    public int distanceFromCam = 10;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 posOFCam = myCamera.transform.position;
        Vector3 directionOFCam = myCamera.transform.forward;
        Vector3 calcVector = posOFCam + (distanceFromCam * directionOFCam);
        calcVector.y = PlayerHead.transform.position.y;
        transform.position = calcVector;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
