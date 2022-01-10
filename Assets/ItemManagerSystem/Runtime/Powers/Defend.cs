using UnityEngine;

namespace ItemManagerSystem
{
    [CreateAssetMenu(fileName = "Defend", menuName = "ItemManager/Powers/Defend", order = 0)]
    public class Defend : APower
    {
        public override void Play()
        {
            Debug.Log("Defend");
        }
    }
}