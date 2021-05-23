using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKFootBehaviorScript : MonoBehaviour //TwoBoneIKConstraint component script
{

    //achual feet
    [SerializeField] private Transform footTransformL;
    [SerializeField] private Transform footTransformR;
    private Transform[] allFootTransforms;

    //foot target
    [SerializeField] private Transform footTransformTargetL;
    [SerializeField] private Transform footTransformTargetR;
    private Transform[] allFootTransformsTargets;

    //foot rigs
    [SerializeField] private GameObject footRigL;
    [SerializeField] private GameObject footRigR;
    private TwoBoneIKConstraint[] allIKFootConstrains;

    private LayerMask groundLayerMask;

    public float maxHitDistance = 1.5f;
    public float addedHeight = 1f;

    private bool[] allGroundSpherecastHits;

    private LayerMask hitLayer;

    private Vector3[] allHitNormals;

    private float angleAboutX;
    private float angleAboutZ;

    public float yOffset = 0.08f;

    // Start is called before the first frame update
    void Start()
    {
        allFootTransforms = new Transform[2];
        allFootTransforms[0] = footTransformL;
        allFootTransforms[1] = footTransformR;

        allFootTransformsTargets = new Transform[2];
        allFootTransformsTargets[0] = footTransformTargetL;
        allFootTransformsTargets[1] = footTransformTargetR;

        allIKFootConstrains = new TwoBoneIKConstraint[2];
        allIKFootConstrains[0] = footRigL.GetComponent<TwoBoneIKConstraint>();
        allIKFootConstrains[1] = footRigR.GetComponent<TwoBoneIKConstraint>();

        groundLayerMask = LayerMask.NameToLayer("Ground");

        allGroundSpherecastHits = new bool[3];

        allHitNormals = new Vector3[2];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateCharacterFeet();
    }

    //modified Sphere cast (this is a nightmare version of check ground with Sphere casts)
    private void CheckGroundBelow(out Vector3 hitPoint, out bool gotGroundSpherecastHit, out Vector3 hitNormal, out LayerMask hitLayer, out float currentHitDistance,
        Transform objectTransform, int checkForLayerMask, float maxHitDistance, float addedHeight) 
    {
        RaycastHit hit;
        Vector3 startSphereCast = objectTransform.position + new Vector3(0f, addedHeight, 0f);

        if (checkForLayerMask == -1)
        {
            Debug.LogError("Layer does not exist!");
            gotGroundSpherecastHit = false;
            currentHitDistance = 0;
            hitLayer = LayerMask.NameToLayer("Player");
            hitNormal = Vector3.up;
            hitPoint = objectTransform.position;
        }
        else 
        {
            int layerMask = (1 << checkForLayerMask);
            if (Physics.SphereCast(startSphereCast, 0.2f, Vector3.down, out hit, maxHitDistance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                hitLayer = hit.transform.gameObject.layer;
                currentHitDistance = hit.distance - addedHeight;
                hitNormal = hit.normal;
                gotGroundSpherecastHit = true;
                hitPoint = hit.point;
            }
            else
            {
                gotGroundSpherecastHit = false;
                currentHitDistance = 0;
                hitLayer = LayerMask.NameToLayer("Player");
                hitNormal = Vector3.up;
                hitPoint = objectTransform.position;
            }
        }
    }

    Vector3 ProjectOnCentactPlane(Vector3 vector, Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal);
    }

    private void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform footTargetTransform, Vector3 hitNormal) 
    {
        Vector3 xAxisProjected = ProjectOnCentactPlane(footTargetTransform.forward, hitNormal).normalized;
        Vector3 zAxisProjected = ProjectOnCentactPlane(footTargetTransform.right, hitNormal).normalized;

        angleAboutX = Vector3.SignedAngle(footTargetTransform.forward, xAxisProjected, footTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(footTargetTransform.right, xAxisProjected, footTargetTransform.forward);
    }

    private void RotateCharacterFeet() 
    {

        for (int i = 0; i < 2; i++)
        {
            CheckGroundBelow(out Vector3 hitPoint, out allGroundSpherecastHits[i], out Vector3 hitNormal, out hitLayer, out _, allFootTransforms[i],
                groundLayerMask, maxHitDistance, addedHeight);
            allHitNormals[i] = hitNormal;

            if (allGroundSpherecastHits[i] == true)
            {
                ProjectedAxisAngles(out angleAboutX, out angleAboutZ, allFootTransforms[i], allHitNormals[i]);

                allFootTransformsTargets[i].position = new Vector3(allFootTransforms[i].position.x, hitPoint.y + yOffset, allFootTransforms[i].position.z);

                allFootTransformsTargets[i].rotation = allFootTransforms[i].rotation;

                allFootTransformsTargets[i].localEulerAngles = new Vector3(allFootTransformsTargets[i].localEulerAngles.x + angleAboutX,
                    allFootTransformsTargets[i].localEulerAngles.y, allFootTransformsTargets[i].localEulerAngles.z + angleAboutZ);

            }
            else
            {
                allFootTransformsTargets[i].position = allFootTransforms[i].position;
                allFootTransformsTargets[i].rotation = allFootTransforms[i].rotation;
            }
        }

    }


}
