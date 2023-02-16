using UnityEngine;

namespace WeaponizationSystemReforged.WeaponizationSystem
{
    [CreateAssetMenu(fileName = "Item", menuName = "Weponization System/New Item", order = 1)]
    public class Item : ScriptableObject {
        [SerializeField] private new string name;
        [TextArea(5, 10), SerializeField] private string description;
        [SerializeField] private Sprite representation;

        #region Item interface propertyes
        public string Name { get => name; }
        public string Description { get => description; }
        public Sprite Representation { get => representation; }
        #endregion
    }
}