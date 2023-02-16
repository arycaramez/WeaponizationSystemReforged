using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{ 
    public interface IAnchorControlForThirdPersonCamera
    {
        Transform GetAnchor();

        void CreateAnchor();

        /// <summary></summary>
        void CalculateEulerForAnchorRotation();

        /// <summary>Used in LateUpdate method.</summary>
        /// <param name="player"></param>
        void CalculateAnchorPosition(Transform player);

        void OnDestroyAnchor();
}
}
