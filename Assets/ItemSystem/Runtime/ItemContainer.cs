using System.Collections.Generic;
using UnityEngine;

namespace GoblinAdventures.ItemSystem.Runtime
{
    [CreateAssetMenu(fileName = "Container", menuName = "Items", order = 0)]
    public class ItemContainer : ScriptableObject
    {
#if UNITY_EDITOR
        public string Name
        {
            get
            {
                var str = UnityEditor.AssetDatabase.GetAssetPath(this).Split('/')
                    [UnityEditor.AssetDatabase.GetAssetPath(this).Split('/').Length - 1];
                str = str.Remove(str.Length - ".asset".Length);
                return str;
            }
        }
#endif
        public List<ItemData> items;
    }
}