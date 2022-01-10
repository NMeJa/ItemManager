using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GoblinAdventures.ItemSystem.Runtime
{
    [CreateAssetMenu(fileName = "Container", menuName = "ItemManager/Container", order = 0)]
    public class ItemContainer : ScriptableObject
    {
        #region Editor

#if UNITY_EDITOR
        public string Name
        {
            get
            {
                var str = AssetDatabase.GetAssetPath(this).Split('/')
                    [AssetDatabase.GetAssetPath(this).Split('/').Length - 1];
                str = str.Remove(str.Length - ".asset".Length);
                return str;
            }
        }

#endif

        #endregion

        [SerializeField] private List<ItemData> items;
        public List<ItemData> Items => items;
    }
}