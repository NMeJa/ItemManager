using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GoblinAdventures.UIElements
{
    internal class EnumFieldFlags : EnumFlagsField
    {
        public new class UxmlFactory : UxmlFactory<EnumFieldFlags, UxmlTraits> { }
    }
}