using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WeaponizationSystemReforged.WeaponizationSystem
{
    public class WeaponEquippingSystem : MonoBehaviour, IWeaponEquippingSystem
    {
        [SerializeField] private WeaponItem weaponSelected;
        [Space(10)]
        [SerializeField] private Transform anchorHandR;
        [SerializeField] private Transform anchorHandL;
        [Space(10)]
        [SerializeField] private List<ArsenalKeeped> arsenalKeeped = new List<ArsenalKeeped>();
        [Space(10)]
        [SerializeField] private List<AnchorWeaponKeepedReferences> anchorsKeepedReferences;

        private WeaponItem mWeapon;

        private GameObject sceneWeaponHandR;
        private GameObject sceneWeaponHandL;

        public WeaponItem WeaponSelected { get => weaponSelected; set => weaponSelected = value; }

        public delegate void OnWeaponChangedDelegate();
        public event OnWeaponChangedDelegate OnWeaponChanged;

        void Start() {
            Init();
        }

        void Update() {
            CheckIfWeaponHasChanged();
        }

        public void Init() {
            OnWeaponChanged += UpdateAllWeaponsInScene;
        }

        public void CheckIfWeaponHasChanged() {
            bool checkIfArsenalChanged = false;
            foreach (var ak in arsenalKeeped) {
                if (ak.Weapon != ak.MWeapon) {
                    checkIfArsenalChanged = true;
                    break;
                }
            }

            if (mWeapon != weaponSelected || checkIfArsenalChanged) {
                OnWeaponChanged();
            }
        }

        public void UpdateAllWeaponsInScene() {
            GameObject prefabHandR = weaponSelected ? weaponSelected.PrefabHandR : null;
            GameObject prefabHandL = weaponSelected ? weaponSelected.PrefabHandL : null;

            sceneWeaponHandR = InstantiateWeaponOnHand(prefabHandR, anchorHandR, sceneWeaponHandR);
            sceneWeaponHandL = InstantiateWeaponOnHand(prefabHandL, anchorHandL, sceneWeaponHandL);

            UpdateArsenalObjects();

            mWeapon = weaponSelected;
        }

        private void UpdateArsenalObjects() { 
            foreach (var ak in arsenalKeeped) {
                AnchorWeaponKeepedReferences sceneRefToKeep = GetSceneKeepReference(ak.Weapon, anchorsKeepedReferences);
                if (sceneRefToKeep != null) {
                    GameObject _prefabHandR = ak.Weapon? ak.Weapon.PrefabHandR : null;
                    GameObject _prefabHandL = ak.Weapon? ak.Weapon.PrefabHandL : null;

                    ak.SceneWeaponR = InstantiateWeaponOnHand(_prefabHandR, sceneRefToKeep.AnchorR, ak.SceneWeaponR);
                    ak.SceneWeaponL = InstantiateWeaponOnHand(_prefabHandL, sceneRefToKeep.AnchorL, ak.SceneWeaponL);
                }
            }
        }

        private AnchorWeaponKeepedReferences GetSceneKeepReference(WeaponItem weaponSelected, List<AnchorWeaponKeepedReferences> anchorsKeepedReferences)
        {
            if(weaponSelected == null || anchorsKeepedReferences.Count <= 0) return null;
            foreach (var e in anchorsKeepedReferences) {
                if (e.WeaponType == weaponSelected.Type) {
                    return e;
                }
            }
            return null;
        }

        public GameObject InstantiateWeaponOnHand(GameObject prefab,Transform anchorHand, GameObject sceneWeapon) {
            if (prefab != null)
            {
                if(sceneWeapon) Destroy(sceneWeapon);
                sceneWeapon = Instantiate(prefab, anchorHand.position, anchorHand.rotation);
                sceneWeapon.transform.SetParent(anchorHand);
            }else{
                if(sceneWeapon) Destroy(sceneWeapon);
            }
            return sceneWeapon;
        }
    }

    [System.Serializable]
    public class ArsenalKeeped {
        [SerializeField] private WeaponItem weapon;

        private WeaponItem mWeapon;

        private GameObject sceneWeaponR;
        private GameObject sceneWeaponL;

        public WeaponItem Weapon { get => weapon; set => weapon = value; }
        public GameObject SceneWeaponR { get => sceneWeaponR; set => sceneWeaponR = value; }
        public GameObject SceneWeaponL { get => sceneWeaponL; set => sceneWeaponL = value; }
        public WeaponItem MWeapon { get => mWeapon; set => mWeapon = value; }

        public ArsenalKeeped(WeaponItem weapon) { 
            this.weapon = weapon;
        }
    }

    [System.Serializable]
    public class AnchorWeaponKeepedReferences {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private Transform anchorR;
        [SerializeField] private Transform anchorL;

        public WeaponType WeaponType { get => weaponType; }
        public Transform AnchorR { get => anchorR; }
        public Transform AnchorL { get => anchorL; }
    }
}
