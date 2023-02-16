using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{
    public class CameraPositionRayForThirdPersonCamera : MonoBehaviour, ICameraPositionRayForThirdPersonCamera
    {
        public Ray CreateRayForCameraPosition(Transform anchor)
        {
            return new Ray(anchor.position, -anchor.forward);
        }
    }
}