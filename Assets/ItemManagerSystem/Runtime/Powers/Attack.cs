using UnityEngine;

namespace ItemManagerSystem
{
    [CreateAssetMenu(fileName = "Attack", menuName = "ItemManager/Powers/Attack", order = 0)]
    public class Attack : APower
    {
        public override void Play()
        {
            Debug.Log("Attack");
        }
    }
}