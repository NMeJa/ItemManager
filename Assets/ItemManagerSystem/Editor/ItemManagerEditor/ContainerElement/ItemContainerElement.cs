using ItemManagerSystem.ItemManagerEditor.Utilities;
using UnityEngine.UIElements;

namespace ItemManagerSystem.ItemManagerEditor
{
    public class ItemContainerElement : VisualElement
    {
        public ItemContainerElement()
        {
            ItemContainerElement root = this;
            const string windowName = nameof(ItemContainerElement);
            AssetDatabaseUtilities.LoadUxmlWithName(windowName).CloneTree(root);
            root.styleSheets.Add(AssetDatabaseUtilities.LoadUssWithName(windowName));
        }

        public new class UxmlFactory : UxmlFactory<ItemContainerElement> { }
    }
}