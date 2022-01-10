using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ItemSystem.ItemManagerWindow
{
    internal static class AssetDatabaseUtilities
    {
        internal const string AssetPath = "Assets/";

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">For every type derived from ScriptableObject </typeparam>
        /// <returns></returns>
        internal static IEnumerable<T> GetAllInstances<T>() where T : ScriptableObject
        {
            //FindAssets uses tags check documentation for more info
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++) //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        internal static StyleSheet LoadUssWithName(string name)
        {
            //FindAssets uses tags check documentation for more info
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(StyleSheet));
            if (guids.IsEmpty()) throw new NullReferenceException("No StyleSheet/USS found at all!");
            const string extension = ".uss";
            string correctedName = $"{name.TrimEnd(extension)}{extension}";
            int count = 0;
            string styleSheetPath = "";
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileName(path);
                if (!fileName.Equals(correctedName)) continue;
                styleSheetPath = path;
                count++;
                if (count > 1) throw new Exception("More than one StyleSheet/USS found with the same name!");
            }

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);
            if (styleSheet is null) throw new NullReferenceException("StyleSheet/USS is null! Path was incorrect");
            return styleSheet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception"></exception>
        internal static VisualTreeAsset LoadUxmlWithName(string name)
        {
            //FindAssets uses tags check documentation for more info
            string[] guids = AssetDatabase.FindAssets("t:" + nameof(VisualTreeAsset));
            if (guids.IsEmpty()) throw new NullReferenceException("No VisualTreeAsset/UXML found at all!");
            const string extension = ".uxml";
            string correctedName = $"{name.TrimEnd(extension)}{extension}";
            int count = 0;
            string visualTreePath = "";
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileName(path);
                if (!fileName.Equals(correctedName)) continue;
                visualTreePath = path;
                count++;
                if (count > 1) throw new Exception("More than one VisualTreeAsset/UXML found with the same name!");
            }

            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(visualTreePath);
            if (visualTree is null)
                throw new NullReferenceException("VisualTreeAsset/UXML is null! Path was incorrect");
            return visualTree;
        }

        private static bool IsEmpty(this IEnumerable<string> source) => !source.Any();

        private static string TrimEnd(this string str, string wordToRemove)
        {
            if (str.EndsWith(wordToRemove))
            {
                str = str.Remove(str.Length - wordToRemove.Length, wordToRemove.Length);
            }

            return str;
        }
    }
}