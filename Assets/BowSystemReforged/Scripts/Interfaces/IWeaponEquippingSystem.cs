using UnityEngine;

namespace WeaponizationSystemReforged.WeaponizationSystem
{
    public interface IWeaponEquippingSystem {
        void Init();

        void CheckIfWeaponHasChanged();

        void UpdateAllWeaponsInScene();

        GameObject InstantiateWeaponOnHand(GameObject prefab, Transform anchorHand, GameObject sceneWeapon);
    }
}
