using UnityEngine;

public class Tt : MonoBehaviour
{
    public string sd;

    [ContextMenu("D")]
    private void S()
    {
        sd = sd.Remove(sd.Length - ".asset".Length);
    }
}