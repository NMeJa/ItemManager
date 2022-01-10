using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GoblinAdventures.ItemSystem.Runtime;
using GoblinAdventures.UIElements;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ItemSystem.ItemManagerWindow
{
    public class ItemManagerEditorWindow : EditorWindow
    {
        private const string ItemConfigurationContainer_VE = "ItemConfigurationContainer_VE";
        private const string ItemName_L = "ItemName_L";
        private const string SaveBtn_TB = "SaveBtn_TB";
        private const string CancelBtn_TB = "CancelBtn_TB";
        private const string DeleteBtn_TB = "DeleteBtn_TB";
        private const string Icon_IE = "Icon_IE";
        private const string Name_TF = "Name_TF";
        private const string Tag_TF = "Tag_TF";
        private const string Icon_OF = "Icon_OF";
        private const string Stackable_T = "Stackable_T";
        private const string MinLevelReq_IF = "MinLevelReq_IF";
        private const string Prefab_OF = "Prefab_OF";
        private const string Rarity_EF = "Rarity_EF";
        private const string Category_EF = "Category_EF";
        private const string Effects_EF = "Effects_EF";
        private const string Description_TF = "Description_TF";
        private const string BuyPrice_FF = "BuyPrice_FF";
        private const string SellPricePercentage_S = "SellPercentage_S";
        private const string SellPrice_FF = "SellPrice_FF";

        private VisualElement ItemConfigurationContainer => root.Q<VisualElement>(ItemConfigurationContainer_VE);
        private Label ItemName => root.Q<Label>(ItemName_L);
        private Button SaveBtn => root.Q<Button>(SaveBtn_TB);
        private Button CancelBtn => root.Q<Button>(CancelBtn_TB);
        private Button DeleteBtn => root.Q<Button>(DeleteBtn_TB);
        private ImageElement Icon => root.Q<ImageElement>(Icon_IE);
        private TextField Name => root.Q<TextField>(Name_TF);
        private TagField Tag => root.Q<TagField>(Tag_TF);
        private EnumFlagsField Category => root.Q<EnumFlagsField>(Category_EF);
        private EnumFlagsField Effects => root.Q<EnumFlagsField>(Effects_EF);
        private ObjectField IconObj => root.Q<ObjectField>(Icon_OF);
        private Toggle Stackable => root.Q<Toggle>(Stackable_T);
        private IntegerField MinLevel => root.Q<IntegerField>(MinLevelReq_IF);
        private ObjectField Prefab => root.Q<ObjectField>(Prefab_OF);
        private EnumField Rarity => root.Q<EnumField>(Rarity_EF);
        private TextField Description => root.Q<TextField>(Description_TF);
        private FloatField BuyPrice => root.Q<FloatField>(BuyPrice_FF);
        private Slider SellPricePercentage => root.Q<Slider>(SellPricePercentage_S);
        private FloatField SellPrice => root.Q<FloatField>(SellPrice_FF);

        private VisualElement root;

        [MenuItem("NMJ/ItemManagerEditorWindow")]
        public static void ShowExample()
        {
            ItemManagerEditorWindow wnd = GetWindow<ItemManagerEditorWindow>();
            wnd.titleContent = new GUIContent("ItemManagerEditorWindow");
            wnd.minSize = new Vector2(750, 800);
            wnd.maxSize = new Vector2(750, 800);
        }

        public void CreateGUI()
        {
            root = rootVisualElement;
            const string windowName = nameof(ItemManagerEditorWindow);
            AssetDatabaseUtilities.LoadUxmlWithName(windowName).CloneTree(root);
            root.styleSheets.Add(AssetDatabaseUtilities.LoadUssWithName(windowName));
            HeaderContainer();
            BodyContainer();
        }

        private List<ItemContainer> containers;
        private ItemContainer selectedContainer;
        private ItemData selectedItem;
        private ItemData tmpItem;
        private List<ItemData> selectedItems;

        private const string LoadBtn_TB = "LoadBtn_TB";
        private const string Containers_TPSF = "Containers_TPSF";

        private Button LoadBtn => root.Q<Button>(LoadBtn_TB);
        private ToolbarPopupSearchField SearchContainers => root.Q<ToolbarPopupSearchField>(Containers_TPSF);

        private void HeaderContainer()
        {
            containers = AssetDatabaseUtilities.GetAllInstances<ItemContainer>().ToList();
            LoadBtn.clicked += FillItemContainer;

            SetContainerItems(containers);
            EditorCoroutine coroutine = null;
            var editorWaitForSeconds = new EditorWaitForSeconds(0.45f);

            SearchContainers.RegisterValueChangedCallback(evt =>
                {
                    if (coroutine != null) EditorCoroutineUtility.StopCoroutine(coroutine);
                    var newList = containers.Search(evt.newValue);
                    SetContainerItems(newList);
                    coroutine = EditorCoroutineUtility.StartCoroutine(ShowDropDown(), this);
                });

            IEnumerator ShowDropDown()
            {
                yield return editorWaitForSeconds;
                SearchContainers.ShowMenu();
                Repaint();
            }

            void SetContainerItems(List<ItemContainer> items)
            {
                SearchContainers.menu.MenuItems().Clear();
                foreach (var element in items)
                {
                    SearchContainers.menu.AppendAction(element.name, evt =>
                        {
                            selectedContainer = element;
                            SearchContainers.Q<TextField>().SetValueWithoutNotify(evt.name);
                        });
                }
            }
        }

        private const string ContainerName_L = "ContainerName_L";
        private const string ItemContainers_LV = "ItemContainers_LV";
        private const string SearchItems_TSF = "SearchItems_TSF";
        private const string AddItemBtn_TB = "AddItemBtn_TB";
        private Label ContainerName => root.Q<Label>(ContainerName_L);
        private ListView ItemDatas => root.Q<ListView>(ItemContainers_LV);
        private ToolbarSearchField SearchItems => root.Q<ToolbarSearchField>(SearchItems_TSF);
        private Button AddItemBtn => root.Q<Button>(AddItemBtn_TB);

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private void BodyContainer()
        {
            Action unregisterEvents = null;
            tmpItem = CreateInstance<ItemData>();
            ItemConfigurationContainer.visible = false;

            void ItemsContainer()
            {
                if (selectedContainer is not null) FillItemContainer();

                ItemDatas.makeItem = () => new ItemContainerElement();
                ItemDatas.bindItem = (e, i) =>
                    {
                        var item = (ItemContainerElement) e;
                        item.Q<ImageElement>().value = selectedItems[i].Icon;
                        item.Q<Label>().text = selectedItems[i].Name;
                    };
                ItemDatas.onItemsChosen += evt =>
                    {
                        ItemConfigurationContainer.visible = true;
                        ItemConfigurationContainer.Unbind();
                        selectedItem = evt.FirstOrDefault() as ItemData;
                        if (selectedItem is null) throw new NullReferenceException("Selected Item Data is null");
                        selectedItem.CopyTo(tmpItem);
                        var so = new SerializedObject(selectedItem);
                        ItemConfigurationContainer.Bind(so);
                        unregisterEvents?.Invoke();
                        unregisterEvents = null;
                        AttributesGroupBox(false);
                        Repaint();
                    };
                SearchItems.RegisterValueChangedCallback(evt =>
                    {
                        selectedItems = selectedContainer.Items.Search(evt.newValue);
                        ItemDatas.itemsSource = selectedItems;
                        ItemDatas.RefreshItems();
                    });
                AddItemBtn.clicked += () =>
                    {
                        var item = CreateInstance<ItemData>();
                        item.name = GUID.Generate().ToString()[..10];
                        AssetDatabase.AddObjectToAsset(item, selectedContainer);
                        AssetDatabase.SaveAssets();
                        selectedContainer.Items.Add(item);
                        ItemDatas.RefreshItems();
                    };
            }

            void GeneralGroupBox()
            {
                ItemName.bindingPath = ItemData.CName;
                Icon.bindingPath = ItemData.CIcon;
                Name.bindingPath = ItemData.CName;
                Tag.bindingPath = ItemData.CTag;
                IconObj.bindingPath = ItemData.CIcon;
                Stackable.bindingPath = ItemData.CStackable;
                MinLevel.bindingPath = ItemData.CMinLevel;
                Prefab.bindingPath = ItemData.CPrefab;

                ItemName.RegisterValueChangedCallback(_ => ItemDatas.Rebuild());
                IconObj.RegisterValueChangedCallback(_ => ItemDatas.Rebuild());
            }

            void AttributesGroupBox(bool isFirst = true)
            {
                const string Strength_TB = "Strength_TB";
                const string Intelligence_TB = "Intelligence_TB";
                const string Dexterity_TB = "Dexterity_TB";
                const string Constitution_TB = "Constitution_TB";
                const string Wisdom_TB = "Wisdom_TB";
                const string Charisma_TB = "Charisma_TB";
                SetValues(Strength_TB, ItemData.CStrength);
                SetValues(Intelligence_TB, ItemData.CIntelligence);
                SetValues(Dexterity_TB, ItemData.CDexterity);
                SetValues(Constitution_TB, ItemData.Cconstitution);
                SetValues(Wisdom_TB, ItemData.CWisdom);
                SetValues(Charisma_TB, ItemData.CCharisma);

                void SetValues(string attributeName, string variableName)
                {
                    const string UseAttribute_T = "UseAttribute_T";
                    const string NumberBonus_FF = "NumberBonus_FF";
                    const string PercentageBonus_FF = "PercentageBonus_FF";
                    const string MinReq_FF = "MinReq_FF";
                    const string UseNumber_T = "UseNumber_T";
                    const string UsePercentage_T = "UsePercentage_T";
                    const string UseMinReq_T = "UseMinReq_T";

                    Toolbar group = root.Q<Toolbar>(attributeName);

                    Toggle useAttribute = group.Q<Toggle>(UseAttribute_T);
                    FloatField numberBonus = group.Q<FloatField>(NumberBonus_FF);
                    FloatField percentageBonus = group.Q<FloatField>(PercentageBonus_FF);
                    FloatField minReq = group.Q<FloatField>(MinReq_FF);
                    Toggle useNumber = group.Q<Toggle>(UseNumber_T);
                    Toggle usePercentage = group.Q<Toggle>(UsePercentage_T);
                    Toggle useMinReq = group.Q<Toggle>(UseMinReq_T);

                    numberBonus.bindingPath = $"{variableName}.{AnItemAttribute.CBonusNumber}";
                    percentageBonus.bindingPath = $"{variableName}.{AnItemAttribute.CBonusPercentage}";
                    minReq.bindingPath = $"{variableName}.{AnItemAttribute.CMinRequirement}";
                    useAttribute.bindingPath = $"{variableName}.{AnItemAttribute.CUseAttribute}";
                    useNumber.bindingPath = $"{variableName}.{AnItemAttribute.CUseBonusNumber}";
                    usePercentage.bindingPath = $"{variableName}.{AnItemAttribute.CUseBonusPercentage}";
                    useMinReq.bindingPath = $"{variableName}.{AnItemAttribute.CUseMinRequirement}";

                    var useAttributeValue = useAttribute.value;
                    numberBonus.SetEnabled(useAttributeValue && useNumber.value);
                    percentageBonus.SetEnabled(useAttributeValue && usePercentage.value);
                    minReq.SetEnabled(useAttributeValue && useMinReq.value);
                    useNumber.SetEnabled(useAttributeValue);
                    usePercentage.SetEnabled(useAttributeValue);
                    useMinReq.SetEnabled(useAttributeValue);

                    if (isFirst) return;

                    unregisterEvents += () => useAttribute.UnregisterValueChangedCallback(UseAttributeEvent);
                    unregisterEvents += () => useNumber.UnregisterValueChangedCallback(UseNumberEvent);
                    unregisterEvents += () => usePercentage.UnregisterValueChangedCallback(UsePercentageEvent);
                    unregisterEvents += () => useMinReq.UnregisterValueChangedCallback(UseMinReqEvent);

                    useAttribute.RegisterValueChangedCallback(UseAttributeEvent);
                    useNumber.RegisterValueChangedCallback(UseNumberEvent);
                    usePercentage.RegisterValueChangedCallback(UsePercentageEvent);
                    useMinReq.RegisterValueChangedCallback(UseMinReqEvent);

                    void UseAttributeEvent(ChangeEvent<bool> evt)
                    {
                        var val = evt.newValue;
                        numberBonus.SetEnabled(val);
                        percentageBonus.SetEnabled(val);
                        minReq.SetEnabled(val);
                        useNumber.SetEnabled(val);
                        usePercentage.SetEnabled(val);
                        useMinReq.SetEnabled(val);

                        if (val) return;
                        useNumber.value = false;
                        usePercentage.value = false;
                        useMinReq.value = false;
                    }

                    void UseNumberEvent(ChangeEvent<bool> evt)
                    {
                        var val = evt.newValue;
                        numberBonus.SetEnabled(val);
                    }

                    void UsePercentageEvent(ChangeEvent<bool> evt)
                    {
                        var val = evt.newValue;
                        percentageBonus.SetEnabled(val);
                    }

                    void UseMinReqEvent(ChangeEvent<bool> evt)
                    {
                        var val = evt.newValue;
                        minReq.SetEnabled(val);
                    }
                }
            }

            void DescriptionGroupBox()
            {
                Rarity.bindingPath = ItemData.CRarity;
                Category.bindingPath = ItemData.CCategory;
                Effects.bindingPath = ItemData.CEffects;
                Description.bindingPath = ItemData.CDescription;
            }

            void PriceGroupBox()
            {
                BuyPrice.bindingPath = ItemData.CPrice;
                SellPricePercentage.bindingPath = ItemData.CSellPercentage;
                SellPrice.bindingPath = ItemData.CSellPrice;
            }

            SaveBtn.clicked += () =>
                {
                    selectedItem.CopyTo(tmpItem);
                    AssetDatabase.SaveAssets();
                };
            CancelBtn.clicked += () => tmpItem.CopyTo(selectedItem);

            DeleteBtn.clicked += () =>
                {
                    if (!EditorUtility.DisplayDialog("Delete", "Are you sure you want to delete this item?", "Yes",
                                                     "No")) return;

                    AssetDatabase.RemoveObjectFromAsset(selectedItem);
                    selectedContainer.Items.Remove(selectedItem);
                    AssetDatabase.SaveAssets();
                    ItemDatas.RefreshItems();
                    ItemConfigurationContainer.visible = false;
                };

            ItemsContainer();
            GeneralGroupBox();
            AttributesGroupBox();
            DescriptionGroupBox();
            PriceGroupBox();
        }

        private void FillItemContainer()
        {
            ContainerName.text = selectedContainer.Name;
            selectedItems = selectedContainer.Items;
            ItemDatas.itemsSource = selectedItems;
            ItemDatas.RefreshItems();
        }
    }
}