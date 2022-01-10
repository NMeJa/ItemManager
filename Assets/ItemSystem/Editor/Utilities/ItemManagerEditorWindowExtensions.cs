using System;
using System.Collections.Generic;
using System.Linq;
using GoblinAdventures.ItemSystem.Runtime;

namespace ItemSystem.ItemManagerWindow
{
    internal static class ItemManagerEditorWindowExtensions
    {
        public static List<ItemContainer> Search(this List<ItemContainer> list, string searchString,
                                                 Action onSearchEnd = null)
        {
            var result = new List<ItemContainer>();
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
            var result = new List<ItemData>();
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
                    var contains = itemFilterString.ToLower().Contains(char.ToLower(element));
                    var amount = itemFilterString.ToLower().Count(c => c == element);
                    var isMore = amount >= searchString.ToLower().Count(c => c == element);
                    return contains && isMore;
                });
        }
    }
}