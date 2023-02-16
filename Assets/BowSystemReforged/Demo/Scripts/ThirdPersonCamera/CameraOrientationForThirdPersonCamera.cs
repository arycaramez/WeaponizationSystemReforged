using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{
    public class CameraOrientationForThirdPersonCamera : MonoBehaviour, ICameraOrientationForThirdPersonCamera
    {
        [SerializeField] private Vector3 targetOffset = new Vector3(0.47f, 1f, 0.26f);

        public Quaternion GetCameraLookAtTargetPoint(Transform anchor, Transform cameraTransform)
        {
            Vector3 dirTarget = anchor.TransformPoint(targetOffset) - cameraTransform.position;
            return Quaternion.LookRotation(dirTarget);
        }
    }
}