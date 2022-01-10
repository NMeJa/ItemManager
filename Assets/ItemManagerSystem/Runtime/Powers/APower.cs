using UnityEngine;

namespace ItemManagerSystem
{
    // [CreateAssetMenu(fileName = "APower", menuName = "ItemManager/Powers/Change This Name", order = 0)]
    public abstract class APower : ScriptableObject
    {
        // [SerializeField] public string name;
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;


        public abstract void Play();
    }
}