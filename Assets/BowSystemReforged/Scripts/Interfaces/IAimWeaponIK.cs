using System.Collections.Generic;
using UnityEngine;

public interface IAimWeaponIK {

    float WeightAim { get; set; }

    #region Start
    Transform[] GetAllSelectedHumanBones(Animator animator, List<HumanBone> humanBones);
    #endregion

    #region Update
    Vector3 GetTargetPosition(Transform mainCam);
    #endregion

    #region Late Update
    void CalculateHumanBonesOrientation(Transform aimTransform, Vector3 targetPos, int interactions, float weightAim, Transform[] boneTransforms, List<HumanBone> humanBones, float angleLimit, float distanceLimit);

    void AimAtTarget(Transform bone, Transform aim, Vector3 targetPosition, float weightAim);

    Vector3 GetTargetPositionAimFix(Transform aim, Vector3 targetPosition, Transform aimTransform, float angleLimit, float distanceLimit);
    #endregion
}
