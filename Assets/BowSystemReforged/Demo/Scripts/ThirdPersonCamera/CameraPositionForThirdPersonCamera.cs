using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{
    public class CameraPositionForThirdPersonCamera : MonoBehaviour, ICameraPositionForThirdPersonCamera
    {
        [Header("Distance Control:")]
        [SerializeField] private float cameraDistance = 3;
        [SerializeField] private float radius = 0.05f;
        [SerializeField] private LayerMask lmCameraDistance;

        public RaycastHit SpherecastForCalculateCameraPosition(Ray ray)
        {
            RaycastHit hit;
            Physics.SphereCast(ray, radius, out hit, cameraDistance, lmCameraDistance);
            return hit;
        }

        public Vector3 GetCameraPosition(Ray ray, RaycastHit hit)
        {
            Vector3 newPos = ray.GetPoint(cameraDistance);
            if (hit.collider)
            {
                newPos = hit.point;
            }
            return newPos;
        }
    }
}