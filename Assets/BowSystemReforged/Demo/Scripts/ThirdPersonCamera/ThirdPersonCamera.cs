using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace WeaponizationSystemReforged.Demo.ThirdPersonCamera
{
    public class ThirdPersonCamera : MonoBehaviour
    {

        private IAnchorControlForThirdPersonCamera _anchorControl;
        
        private ICameraPositionRayForThirdPersonCamera _cameraPositionRay;
        private ICameraPositionForThirdPersonCamera _cameraPosition;
        private ICameraOrientationForThirdPersonCamera _cameraOrientation;

        [SerializeField] private Transform player;
       
        private RaycastHit hitForCameraPos = new RaycastHit();
        private Ray rayForCameraPos;

        private void Awake()
        {            
            if (GetComponent<ICameraPositionRayForThirdPersonCamera>() == null) 
            {
                gameObject.AddComponent<CameraPositionRayForThirdPersonCamera>();
            }
            _cameraPositionRay = GetComponent<ICameraPositionRayForThirdPersonCamera>();

            if (GetComponent<IAnchorControlForThirdPersonCamera>() == null) {
                gameObject.AddComponent<AnchorControlForThirdPersonCamera>();
            }
            _anchorControl = GetComponent<IAnchorControlForThirdPersonCamera>();

            if (GetComponent<ICameraPositionForThirdPersonCamera>() == null)
            {
                gameObject.AddComponent<CameraPositionForThirdPersonCamera>();
            }
            _cameraPosition = GetComponent<ICameraPositionForThirdPersonCamera>();

            if (GetComponent<ICameraOrientationForThirdPersonCamera>() == null)
            {
                gameObject.AddComponent<CameraOrientationForThirdPersonCamera>();
            }
            _cameraOrientation = GetComponent<ICameraOrientationForThirdPersonCamera>();

            _anchorControl.CreateAnchor();
        }

        void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Transform anchor = _anchorControl.GetAnchor();
            _anchorControl.CalculateEulerForAnchorRotation();
            rayForCameraPos = _cameraPositionRay.CreateRayForCameraPosition(anchor);
            hitForCameraPos = _cameraPosition.SpherecastForCalculateCameraPosition(rayForCameraPos);

            _anchorControl.CalculateAnchorPosition(player);
            rayForCameraPos = _cameraPositionRay.CreateRayForCameraPosition(anchor);
            transform.position = _cameraPosition.GetCameraPosition(rayForCameraPos, hitForCameraPos);
            transform.rotation = _cameraOrientation.GetCameraLookAtTargetPoint(anchor, transform);
        }

        void Update() {
            _anchorControl.CreateAnchor();

            _anchorControl.CalculateEulerForAnchorRotation();
            rayForCameraPos = _cameraPositionRay.CreateRayForCameraPosition(_anchorControl.GetAnchor());
            hitForCameraPos = _cameraPosition.SpherecastForCalculateCameraPosition(rayForCameraPos);
        }

        void LateUpdate()
        {
            Transform anchor = _anchorControl.GetAnchor();
            _anchorControl.CalculateAnchorPosition(player);
            rayForCameraPos = _cameraPositionRay.CreateRayForCameraPosition(anchor);
            transform.position = _cameraPosition.GetCameraPosition(rayForCameraPos, hitForCameraPos);
            transform.rotation = _cameraOrientation.GetCameraLookAtTargetPoint(anchor, transform);
        }

        private void OnDestroy() {
            _anchorControl.OnDestroyAnchor();
        }
    }
}