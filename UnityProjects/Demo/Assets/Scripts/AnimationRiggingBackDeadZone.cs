using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationRiggingBackDeadZone : MonoBehaviour
{

    [SerializeField] private GameObject Neck;
    [SerializeField] private GameObject Spine1;
    [SerializeField] private GameObject Spine2;

    [SerializeField] private GameObject AccualSpine2;

    [SerializeField] private GameObject Player;
    [SerializeField] private Camera camera;

    private PlayerMovementController PMC;

    public float NeckRotationX;
    public float NeckRotationZ;
    public float NeckRotationY;

    public float Spine2RotationX;


    MultiAimConstraint neck;
    MultiAimConstraint spine1;
    MultiAimConstraint spine2;

    // Start is called before the first frame update
    void Start()
    {
        neck = Neck.GetComponent<MultiAimConstraint>();
        spine1 = Spine1.GetComponent<MultiAimConstraint>();
        spine2 = Spine2.GetComponent<MultiAimConstraint>();

        PMC = Player.GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
     
        NeckRotationX = Neck.transform.position.x;
        NeckRotationY = Neck.transform.rotation.y;
        NeckRotationZ = Neck.transform.rotation.z;

        Spine2RotationX = AccualSpine2.transform.rotation.x;
        
        if (Spine2RotationX <= -.30)
        {
            neck.weight = 0;
            spine1.weight = 0;
            spine2.weight = 0;
        }
        else {
            neck.weight = 1;
            spine1.weight = 1;
            spine2.weight = 1;
        }



    }
}
