using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{ 
    public interface ICameraOrientationForThirdPersonCamera {
        Quaternion GetCameraLookAtTargetPoint(Transform anchor, Transform cameraTransform);
    }
}
