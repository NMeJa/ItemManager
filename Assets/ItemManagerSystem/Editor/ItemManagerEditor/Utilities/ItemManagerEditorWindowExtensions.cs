using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemManagerSystem.ItemManagerEditor.Utilities
{
    internal static class ItemManagerEditorWindowExtensions
    {
        public static List<ItemContainer> Search(this List<ItemContainer> list, string searchString,
                                                 Action onSearchEnd = null)
        {
            List<ItemContainer> result = new List<ItemContainer>();
            if (string.Equals(searchString, "All", StringComparison.CurrentCultureIgnoreCase) ||
                string.IsNullOrEmpty(searchString))
            {
                result = list;
            }
            else
            {
                result.AddRange(list.Where(item => SearchString(searchString, item.Name)));
            }

            onSearchEnd?.Invoke();
            return result;
        }

        public static List<ItemData> Search(this List<ItemData> list, string searchString,
                                            Action onSearchEnd = null)
        {
            List<ItemData> result = new List<ItemData>();
            if (string.Equals(searchString, "All", StringComparison.CurrentCultureIgnoreCase) ||
                string.IsNullOrEmpty(searchString))
            {
                result = list;
            }
            else
            {
                result.AddRange(list.Where(item => SearchString(searchString, item.Name)));
            }

            onSearchEnd?.Invoke();
            return result;
        }

        private static bool SearchString(string searchString, string itemFilterString)
        {
            return searchString.All(element =>
                {
                    bool contains = itemFilterString.ToLower().Contains(char.ToLower(element));
                    int amount = itemFilterString.ToLower().Count(c => c == element);
                    bool isMore = amount >= searchString.ToLower().Count(c => c == element);
                    return contains && isMore;
                });
        }
    }
}