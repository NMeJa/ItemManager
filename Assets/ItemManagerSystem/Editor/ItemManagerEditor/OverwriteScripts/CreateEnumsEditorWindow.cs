using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItemManagerSystem.Enums;
using ItemManagerSystem.ItemManagerEditor.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ItemManagerSystem.ItemManagerEditor
{
    public class CreateEnumsEditorWindow : EditorWindow
    {
        private const string Rarity_Btn = "Rarity_Btn";
        private const string Category_Btn = "Category_Btn";
        private const string Effects_Btn = "Effects_Btn";
        private const string RarityContainer_VE = "RarityContainer_VE";
        private const string CategoryContainer_VE = "CategoryContainer_VE";
        private const string EffectsContainer_VE = "EffectsContainer_VE";

        private VisualElement root;

        [MenuItem("Tool/NMJ/Create Enums")]
        public static void ShowExample()
        {
            CreateEnumsEditorWindow wnd = GetWindow<CreateEnumsEditorWindow>();
            wnd.titleContent = new GUIContent("CreateEnumsEditorWindow");
        }

        private Dictionary<Button, VisualElement> buttons;

        public void CreateGUI()
        {
            root = rootVisualElement;

            const string windowName = nameof(CreateEnumsEditorWindow);
            AssetDatabaseUtilities.LoadUxmlWithName(windowName).CloneTree(root);
            root.styleSheets.Add(AssetDatabaseUtilities.LoadUssWithName(windowName));

            buttons = new Dictionary<Button, VisualElement>
            {
                {root.Q<Button>(Rarity_Btn), root.Q<VisualElement>(RarityContainer_VE)},
                {root.Q<Button>(Category_Btn), root.Q<VisualElement>(CategoryContainer_VE)},
                {root.Q<Button>(Effects_Btn), root.Q<VisualElement>(EffectsContainer_VE)}
            };

            foreach ((Button button, VisualElement container) in buttons)
            {
                button.clicked += () => Change(button);
                ScrollView scrollView = container.Q<ScrollView>();
                container.Q<Button>("Plus_Btn").clicked += () => { CreateTextField(scrollView); };
                container.Q<Button>("Minus_Btn").clicked +=
                    () =>
                        {
                            if (scrollView.childCount > 0)
                                scrollView.RemoveAt(scrollView.childCount - 1);
                        };
                container.Q<Button>("Create_Btn").clicked += () => Generate(container);
            }

            Change(buttons.FirstOrDefault().Key);
            SetValues();
        }

        private void SetValues()
        {
            ScrollView rarityScrollView = root.Q<VisualElement>(RarityContainer_VE).Q<ScrollView>();
            foreach (string element in Enum.GetNames(typeof(RarityEnum)))
            {
                CreateTextField(rarityScrollView, element);
            }

            ScrollView categoryScrollView = root.Q<VisualElement>(CategoryContainer_VE).Q<ScrollView>();
            foreach (string element in Enum.GetNames(typeof(CategoryEnum)))
            {
                CreateTextField(categoryScrollView, element);
            }

            ScrollView effectsScrollView = root.Q<VisualElement>(EffectsContainer_VE).Q<ScrollView>();
            foreach (string element in Enum.GetNames(typeof(EffectsEnum)))
            {
                CreateTextField(effectsScrollView, element);
            }
        }

        private static void CreateTextField(ScrollView scrollView, string element = null)
        {
            TextField textField = new TextField
            {
                label = element ?? $"Element {scrollView.childCount}",
                value = element ?? ""
            };
            textField.RegisterValueChangedCallback(evt => textField.label = evt.newValue);
            scrollView.Add(textField);
        }

        private static void Generate(VisualElement container)
        {
            switch (container.name)
            {
                case RarityContainer_VE:
                    GenerateRarity(container);
                    break;
                case CategoryContainer_VE:
                    GenerateCategory(container);
                    break;
                case EffectsContainer_VE:
                    GenerateEffects(container);
                    break;
                default: throw new ArgumentOutOfRangeException(container.name, "Container name not found");
            }

            AssetDatabase.Refresh();
        }

        private const string LocalPath = "ItemManagerSystem/Runtime/Enums";

        private static void GenerateRarity(VisualElement visualElement)
        {
            using StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, $"{LocalPath}/RarityEnum.cs"));
            sw.WriteLine("namespace ItemManagerSystem.Enums");
            sw.WriteLine("{");
            {
                sw.WriteLine("    public enum RarityEnum");
                sw.WriteLine("    {");
                {
                    foreach (VisualElement child in visualElement.Q<ScrollView>().contentContainer.Children())
                    {
                        string text = child.Q<TextField>().text;
                        if (ParseText(ref text)) continue;
                        sw.WriteLine($"        {text},");
                    }
                }
                sw.WriteLine("    }");
            }

            sw.WriteLine("}");
        }

        private static void GenerateCategory(VisualElement visualElement)
        {
            using StreamWriter sw =
                new StreamWriter(Path.Combine(Application.dataPath, $"{LocalPath}/CategoryEnum.cs"));
            sw.WriteLine("namespace ItemManagerSystem.Enums");
            sw.WriteLine("{");
            {
                sw.WriteLine("    [System.Flags]");
                sw.WriteLine("    public enum CategoryEnum");
                sw.WriteLine("    {");
                {
                    int i = 1;
                    foreach (VisualElement child in visualElement.Q<ScrollView>().contentContainer.Children())
                    {
                        string text = child.Q<TextField>().text;
                        if (ParseText(ref text)) continue;
                        sw.WriteLine($"        {text} = 1 << {i++},");
                    }
                }
                sw.WriteLine("    }");
            }

            sw.WriteLine("}");
        }

        private static void GenerateEffects(VisualElement visualElement)
        {
            using StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, $"{LocalPath}/EffectsEnum.cs"));
            sw.WriteLine("namespace ItemManagerSystem.Enums");
            sw.WriteLine("{");
            {
                sw.WriteLine("    [System.Flags]");
                sw.WriteLine("    public enum EffectsEnum");
                sw.WriteLine("    {");
                {
                    int i = 1;
                    foreach (VisualElement child in visualElement.Q<ScrollView>().contentContainer.Children())
                    {
                        string text = child.Q<TextField>().text;
                        if (ParseText(ref text)) continue;
                        sw.WriteLine($"        {text} = 1 << {i++},");
                    }
                }
                sw.WriteLine("    }");
            }

            sw.WriteLine("}");
        }

        private static bool ParseText(ref string text)
        {
            if (string.IsNullOrEmpty(text)) return true;
            text = text.Replace(" ", string.Empty);
            if (!text.All(char.IsLetter)) return true;
            text = $"{char.ToUpper(text[0])}{text[1..].ToLower()}";
            return false;
        }

        private void Change(Button pressedButton)
        {
            foreach ((Button button, VisualElement container) in buttons)
            {
                if (pressedButton == button)
                {
                    button.style.backgroundColor = Color.green;
                    container.style.display = DisplayStyle.Flex;
                }
                else
                {
                    button.style.backgroundColor = Color.grey;
                    container.style.display = DisplayStyle.None;
                }
            }
        }
    }
}