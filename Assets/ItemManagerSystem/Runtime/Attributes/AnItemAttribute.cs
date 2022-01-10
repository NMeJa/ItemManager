using System;
using UnityEngine;

namespace ItemManagerSystem
{
    [Serializable]
    public abstract class AnItemAttribute
    {
        public const string CUseAttribute = nameof(useAttribute);

        public const string CUseBonusNumber = nameof(useBonusNumber);
        public const string CUseBonusPercentage = nameof(useBonusPercentage);
        public const string CUseMinRequirement = nameof(useMinRequirement);

        public const string CBonusNumber = nameof(bonusNumber);
        public const string CBonusPercentage = nameof(bonusPercentage);
        public const string CMinRequirement = nameof(minRequirement);

        [SerializeField] private bool useAttribute;

        [SerializeField] private bool useBonusNumber;
        [SerializeField] private bool useBonusPercentage;
        [SerializeField] private bool useMinRequirement;

        [SerializeField] private float bonusNumber;
        [SerializeField] private float bonusPercentage;
        [SerializeField] private float minRequirement;

        public float BonusNumber => useAttribute && useBonusNumber ? bonusNumber : 0.0f;
        public float BonusPercentage => useAttribute && useBonusPercentage ? bonusPercentage : 0.0f;
        public float MinRequirement => useAttribute && useMinRequirement ? minRequirement : 0.0f;

        public void CopyTo(AnItemAttribute other)
        {
            other.useAttribute = useAttribute;
            other.useBonusNumber = useBonusNumber;
            other.useBonusPercentage = useBonusPercentage;
            other.useMinRequirement = useMinRequirement;
            other.bonusNumber = bonusNumber;
            other.bonusPercentage = bonusPercentage;
            other.minRequirement = minRequirement;
        }
    }
}