using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ItemManagerSystem.ItemManagerEditor.Utilities
{
    public class EnumFieldFlags : EnumFlagsField
    {
        public new class UxmlFactory : UxmlFactory<EnumFieldFlags, UxmlTraits> { }
    }
}