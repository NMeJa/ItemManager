using UnityEngine;

public class Tt : MonoBehaviour
{
    public string sd;

    [ContextMenu("D")]
    private void S()
    {
        sd = Application.dataPath;
        //C:/Users/nmela/Projects/Tools/ItemManager/Assets
    }
}