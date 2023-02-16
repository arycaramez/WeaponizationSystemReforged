using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace WeaponizationSystemReforged.Demo.Character
{
    public class CharacterAnimatiorControl : MonoBehaviour
    {
        [Header("Settings:")]
        [SerializeField] private Animator animator;

        [Header("Animation Variables:")]
        [SerializeField] private string varInVertical = "inVertical";
        [SerializeField] private string varInHorizontal = "inHorizontal";
        [SerializeField] private string varInSpeed = "inSpeed";
        [SerializeField] private string varIsBowKeeped = "isBowKeeped";
        [SerializeField] private string varIsRunning = "isRunning";
        [SerializeField] private string varIsPreparingShot = "isPreparingShot";
        [SerializeField] private string varNowShootArrow = "nowShootArrow";

        [Header("Animation Courves:")]
        [SerializeField] private string varAimWeightCourve = "AimWeightCourve";

        void Start()
        {
            if(!animator) animator = GetComponent<Animator>();
            if (!animator) {
                Debug.LogError("Error: Animatior component not found!");
            }
        }

        #region Layer Controllers
        /// <summary> Control the value of layer weight </summary>
        public void LayerWeightControl(int idAnimationLayer,float weight) {
            if (animator) {
                animator.SetLayerWeight(idAnimationLayer, weight);
            }
        }
        #endregion

        #region Animation Variables
        /// <summary> Vertical Axis </summary>
        public float InVertical {
            get{
                if (animator) return animator.GetFloat(varInVertical);
                return 0; 
            }
            set {
                if (animator) animator.SetFloat(varInVertical, value);
            }
        }
        /// <summary> Horizontal Axis </summary>
        public float InHorizontal {
            get{
                if (animator) return animator.GetFloat(varInHorizontal);
                return 0; 
            }
            set {
                if (animator) animator.SetFloat(varInHorizontal, value);
            }
        }
        /// <summary> Character Speed </summary>
        public float InSpeed {
            get{
                if (animator) return animator.GetFloat(varInSpeed);
                return 0; 
            }
            set {
                if (animator) animator.SetFloat(varInSpeed, value);
            }
        }
        /// <summary> Keep the bow and enable Bow Mode. </summary>
        public bool IsBowKeeped {
            get
            {
                if (animator) return animator.GetBool(varIsBowKeeped);
                return false;
            }
            set
            {
                if (animator) animator.SetBool(varIsBowKeeped, value);
            }
        }
        /// <summary> Enable the animations of running. </summary>
        public bool IsRunning
        {
            get
            {
                if (animator) return animator.GetBool(varIsRunning);
                return false;
            }
            set
            {
                if (animator) animator.SetBool(varIsRunning, value);
            }
        }
        /// <summary> Enable Bow Shot Preparing </summary>
        public bool IsPreparingShot {
            get
            {
                if (animator) return animator.GetBool(varIsPreparingShot);
                return false;
            }
            set
            {
                if (animator) animator.SetBool(varIsPreparingShot, value);
            }
        }
#endregion

        #region Triggers
        /// <summary> Trigger - Shot animation activation</summary>
        public void TriggerNowShootArrow(){
            if (animator) {
                animator.SetTrigger(varNowShootArrow);
            }
        }
        #endregion

        #region Animation Courves
        public float AimWeightCourve
        {
            get {
                if (animator) {
                    return animator.GetFloat(varAimWeightCourve);
                }
                return 0;
            }
            set { 
                if (animator) {
                    animator.SetFloat(varAimWeightCourve, value);
                }
            }
        }
        #endregion

    }
}
