using UnityEngine;
using UnityEngine.UIElements;

namespace ItemManagerSystem.ItemManagerEditor.Utilities
{
    public class ImageElement : BindableElement, INotifyValueChanged<Object>
    {
        private Object image;

        private readonly ImageElement root;

        public ImageElement()
        {
            root = this;
            const string windowName = nameof(ImageElement);
            AssetDatabaseUtilities.LoadUxmlWithName(windowName).CloneTree(root);
            root.styleSheets.Add(AssetDatabaseUtilities.LoadUssWithName(windowName));
        }

        public void SetValueWithoutNotify(Object newValue) => SetImage(newValue);

        public Object value
        {
            get => image;
            set => SetImage(value);
        }

        private void SetImage(Object val)
        {
            root.Q<VisualElement>("Image").style.backgroundImage = new StyleBackground(val as Sprite);
            image = val;
        }

        public new class UxmlFactory : UxmlFactory<ImageElement, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits { }
    }
}