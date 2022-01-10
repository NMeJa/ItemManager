using System.Collections.Generic;
using System.Linq;
using ItemManagerSystem.ItemManagerEditor.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ItemManagerSystem.ItemManagerEditor
{
    public class PowerElement : VisualElement
    {
        private APower selectedPower;

        public PowerElement(ItemData selectedItem)
        {
            PowerElement root = this;
            selectedPower = selectedItem.Powers.Last();
            const string windowName = nameof(PowerElement);
            AssetDatabaseUtilities.LoadUxmlWithName(windowName).CloneTree(root);
            root.styleSheets.Add(AssetDatabaseUtilities.LoadUssWithName(windowName));

            // ReSharper disable once InconsistentNaming
            const string MainContainer_VE = "MainContainer_VE";

            root.style.minHeight = root.Q<VisualElement>(MainContainer_VE).style.height;
            root.style.height = root.Q<VisualElement>(MainContainer_VE).style.height;
            root.style.maxHeight = root.Q<VisualElement>(MainContainer_VE).style.height;
            root.style.minWidth = root.Q<VisualElement>(MainContainer_VE).style.width;
            root.style.width = root.Q<VisualElement>(MainContainer_VE).style.width;
            root.style.maxWidth = root.Q<VisualElement>(MainContainer_VE).style.width;

            root.Q<Button>().clicked += () =>
                {
                    selectedItem.Powers.Remove(selectedPower);
                    RemoveFromHierarchy();
                };
            var objectField = root.Q<ObjectField>();
            objectField.objectType = typeof(APower);
            objectField.RegisterValueChangedCallback(evt =>
                {
                    APower power = evt.newValue as APower;
                    var indexOf = selectedItem.Powers.IndexOf(selectedPower);
                    selectedItem.Powers[indexOf] = power;
                    selectedPower = selectedItem.Powers[indexOf];
                    if (power != null)
                    {
                        root.Q<ImageElement>().value = power.Icon;
                        root.Q<Label>().text = power.name;
                    }
                    else
                    {
                        root.Q<ImageElement>().value = null;
                        root.Q<Label>().text = "Choose Power";
                    }
                });
        }

        public new class UxmlFactory : UxmlFactory<ItemContainerElement> { }
    }
}