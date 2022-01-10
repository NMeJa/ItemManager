using UnityEngine;

namespace GoblinAdventures.ItemSystem.Runtime
{
    public class ItemData : ScriptableObject
    {
        public const string CName = nameof(name);
        public const string CIcon = nameof(icon);
        public const string CTag = nameof(tag);
        public const string CStackable = nameof(stackable);
        public const string CMinLevel = nameof(minLevel);
        public const string CPrefab = nameof(prefab);
        public const string CRarity = nameof(rarity);
        public const string CCategory = nameof(category);
        public const string CEffects = nameof(effects);
        public const string CDescription = nameof(description);
        public const string CPrice = nameof(price);
        public const string CSellPercentage = nameof(sellPercentage);
        public const string CSellPrice = nameof(sellPrice);
        public const string CStrength = nameof(strength);
        public const string CIntelligence = nameof(intelligence);
        public const string CDexterity = nameof(dexterity);
        public const string Cconstitution = nameof(constitution);
        public const string CWisdom = nameof(wisdom);
        public const string CCharisma = nameof(charisma);

        [SerializeField] private new string name;
        [SerializeField] private Sprite icon;
        [SerializeField] private string tag;
        [SerializeField] private bool stackable;
        [SerializeField] private int minLevel;
        [SerializeField] private GameObject prefab;

        [SerializeField] private RarityEnum rarity;
        [SerializeField] private CategoryEnum category;
        [SerializeField] private EffectsEnum effects;
        [SerializeField] [TextArea] private string description;
        [SerializeField] private float price;
        [SerializeField] private float sellPercentage;
        [SerializeField] private float sellPrice;

        [SerializeField] private Strength strength;
        [SerializeField] private Intelligence intelligence;
        [SerializeField] private Dexterity dexterity;
        [SerializeField] private Constitution constitution;
        [SerializeField] private Wisdom wisdom;
        [SerializeField] private Charisma charisma;

        public string Name => name;
        public Sprite Icon => icon;
        public string Tag => tag;
        public bool Stackable => stackable;
        public int MinLevel => minLevel;
        public GameObject Prefab => prefab;
        public RarityEnum Rarity => rarity;
        public CategoryEnum Category => category;
        public EffectsEnum Effects => effects;
        public string Description => description;
        public float Price => price;
        public float SellPercentage => sellPercentage;
        public float SellPrice => sellPrice;
        public Strength Strength => strength;
        public Intelligence Intelligence => intelligence;
        public Dexterity Dexterity => dexterity;
        public Constitution Constitution => constitution;
        public Wisdom Wisdom => wisdom;
        public Charisma Charisma => charisma;

        public void CopyTo(ItemData other)
        {
            other.name = name;
            other.icon = icon;
            other.tag = tag;
            other.stackable = stackable;
            other.minLevel = minLevel;
            other.prefab = prefab;

            other.rarity = rarity;
            other.category = category;
            other.effects = effects;
            other.description = description;
            other.price = price;
            other.sellPercentage = sellPercentage;
            other.sellPrice = sellPrice;

            strength.CopyTo(other.strength = new Strength());
            intelligence.CopyTo(other.intelligence = new Intelligence());
            dexterity.CopyTo(other.dexterity = new Dexterity());
            constitution.CopyTo(other.constitution = new Constitution());
            wisdom.CopyTo(other.wisdom = new Wisdom());
            charisma.CopyTo(other.charisma = new Charisma());
        }
    }
}