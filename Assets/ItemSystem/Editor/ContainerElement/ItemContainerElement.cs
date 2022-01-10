using ItemSystem.ItemManagerWindow;
using UnityEngine.UIElements;


public class ItemContainerElement : VisualElement
{
    public ItemContainerElement()
    {
        var root = this;
        const string windowName = nameof(ItemContainerElement);
        AssetDatabaseUtilities.LoadUxmlWithName(windowName).CloneTree(root);
        root.styleSheets.Add(AssetDatabaseUtilities.LoadUssWithName(windowName));
    }

    public new class UxmlFactory : UxmlFactory<ItemContainerElement> { }
}