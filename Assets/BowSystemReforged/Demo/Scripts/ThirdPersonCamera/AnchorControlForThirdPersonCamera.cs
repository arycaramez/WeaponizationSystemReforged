using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{
    public class AnchorControlForThirdPersonCamera : MonoBehaviour, IAnchorControlForThirdPersonCamera
    {
        [Header("Settings")]
        [SerializeField] private string anchorName = "CameraAnchor";
        [SerializeField] private Vector3 offset = new Vector3(0, 1, 0);

        [Header("Rotation Control")]
        [SerializeField] private string inpMouseX = "Mouse X";
        [SerializeField] private string inpMouseY = "Mouse Y";
        [SerializeField] private float rotationSpeed = 100;
        [SerializeField, Range(1, 179)] private float minAxisY = 89;
        [SerializeField, Range(181, 359)] private float maxAxisY = 320;

        private Transform anchor;

        public Transform GetAnchor()
        {
            return anchor;
        }

        public void CreateAnchor()
        {
            if (!anchor)
            {
                anchor = new GameObject(anchorName).transform;
                anchor.hideFlags = HideFlags.HideInHierarchy;
            }
        }

        public void CalculateEulerForAnchorRotation()
        {
            float axisMouseX = Input.GetAxis(inpMouseX);
            float axisMouseY = Input.GetAxis(inpMouseY);

            Vector3 anchorEuler = anchor.eulerAngles;
            anchorEuler.y += axisMouseX * rotationSpeed * Time.fixedDeltaTime;
            anchorEuler.x -= axisMouseY * rotationSpeed * Time.fixedDeltaTime;

            if (anchorEuler.x > minAxisY && anchorEuler.x < 180) anchorEuler.x = minAxisY;
            if (anchorEuler.x < maxAxisY && anchorEuler.x > 180) anchorEuler.x = maxAxisY;

            anchor.eulerAngles = anchorEuler;
        }

        public void CalculateAnchorPosition(Transform player)
        {
            anchor.position = player.TransformPoint(offset);
        }

        public void OnDestroyAnchor()
        {
            if (anchor)
            {
                Destroy(anchor);
            }
        }
    }
}
