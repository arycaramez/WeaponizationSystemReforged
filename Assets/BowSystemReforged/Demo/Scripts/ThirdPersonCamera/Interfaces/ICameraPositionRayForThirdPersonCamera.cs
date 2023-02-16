using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{ 
    /// <summary>Responsible for create a Ray that it determinate camera position and distance.</summary>
    public interface ICameraPositionRayForThirdPersonCamera
    {
        Ray CreateRayForCameraPosition(Transform anchor);
    }
}
