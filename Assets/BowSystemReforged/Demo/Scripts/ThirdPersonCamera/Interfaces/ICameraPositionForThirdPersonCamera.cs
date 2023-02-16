using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{ 
    public interface ICameraPositionForThirdPersonCamera 
    {   
        RaycastHit SpherecastForCalculateCameraPosition(Ray ray);

        Vector3 GetCameraPosition(Ray ray,RaycastHit hit);
    }
}
