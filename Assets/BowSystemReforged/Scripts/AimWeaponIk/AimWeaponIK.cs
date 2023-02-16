using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWeaponIK : MonoBehaviour, IAimWeaponIK
{
    [Header("Settings:")]
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform aimTransform;

    [Header("Aim IK Control:")]
    [SerializeField] private float angleLimit = 90;
    [SerializeField] private float distanceLimit = 1.5f;
    [SerializeField] int interactions = 10;
    [SerializeField,Range(0,1)] private float weightAim = 1;
    [Space(20)]
    [SerializeField] private List<HumanBone> humanBones = new List<HumanBone>();

    private Vector3 targetPos;
    private Transform[] boneTransforms;

    public float WeightAim
    { 
        get { return weightAim; } 
        set { weightAim = value; } 
    }

    void Start()
    {
        if(!animator) animator = GetComponent<Animator>();
        boneTransforms = GetAllSelectedHumanBones(animator, humanBones);
    }

    public Transform[] GetAllSelectedHumanBones(Animator animator,List<HumanBone> humanBones) {
        Transform[] boneTransforms = new Transform[humanBones.Count];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
        return boneTransforms;
    }

    private void Update()
    {
        targetPos = GetTargetPosition(Camera.main.transform);
    }

    public Vector3 GetTargetPosition(Transform mainCam) {
        Vector3 targetPos = mainCam.position + mainCam.forward * 100;

        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            targetPos = hit.point;
        }
        return targetPos;
    }

    private void LateUpdate()
    {
        CalculateHumanBonesOrientation(
            aimTransform, targetPos, interactions, weightAim, boneTransforms,humanBones, angleLimit, distanceLimit);
    }

    public void CalculateHumanBonesOrientation(Transform aimTransform,Vector3 targetPos, int interactions, float weightAim, Transform[] boneTransforms,List<HumanBone> humanBones, float angleLimit, float distanceLimit) {
        if (aimTransform == null) return;

        Vector3 _relTargetPos = GetTargetPositionAimFix(aimTransform, targetPos, aimTransform, angleLimit, distanceLimit);

        for (int i = 0; i < interactions; i++)
        {
            for (int b = 0; b < boneTransforms.Length; b++)
            {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weightAim;
                AimAtTarget(bone, aimTransform, _relTargetPos, boneWeight);
            }
        }
    }

    public void AimAtTarget(Transform bone,Transform aim,Vector3 targetPosition,float weightAim) {
        Vector3 aimDirection = aim.forward;
        Vector3 targetDirection = targetPosition - aim.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity,aimTowards,weightAim);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public Vector3 GetTargetPositionAimFix(Transform aim,Vector3 targetPosition,Transform aimTransform,float angleLimit,float distanceLimit) {
        Vector3 aimDirection = aim.forward;
        Vector3 targetDirection = targetPosition - aim.position;

        float blendeOut = 0;
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit) {
            blendeOut += (targetAngle - angleLimit) / 50;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit) {
            blendeOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendeOut);
        return aimTransform.position + direction;
    }
}