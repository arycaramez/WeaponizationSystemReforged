using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponizationSystemReforged.WeaponizationSystem
{
    [CreateAssetMenu(fileName = "Weapon Item", menuName = "Weponization System/New Weapon Item", order = 1)]
    public class WeaponItem : Item
    {
        [SerializeField] private float damage;
        [SerializeField] private WeaponType type;
        [SerializeField] private ShotType shotType;
        [SerializeField] private GameObject projectilePrefab;
        
        [SerializeField] private GameObject prefabHandR;
        [SerializeField] private GameObject prefabHandL;

        public GameObject PrefabHandR { get => prefabHandR; }
        public GameObject PrefabHandL { get => prefabHandL; }
        public float Damage { get => damage; }
        public WeaponType Type { get => type; }
        public ShotType ShotType { get => shotType; }
        public GameObject ProjectilePrefab { get => projectilePrefab; }

        public WeaponItem Instance{
            get { return this; }
        }
    }
}