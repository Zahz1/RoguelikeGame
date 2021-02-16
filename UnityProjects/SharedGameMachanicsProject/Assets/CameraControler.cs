using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    Vector3 velocity;

    public float smoothTimeZ;

    public float smoothTimeX;

    public float smoothTimeY;

    public float offsetZ = -10f;
    public float offsetY = 4f;


    public GameObject player;

    private Vector3 originPos;

    private void Start()
    {
        originPos = transform.position;
    }

    private void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posZ = Mathf.SmoothDamp(transform.position.z, player.transform.position.z + offsetZ, ref velocity.z, smoothTimeZ);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y + offsetY, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, posZ);
    }
}
