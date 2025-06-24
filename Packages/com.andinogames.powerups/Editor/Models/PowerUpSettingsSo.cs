using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PowerUps.Editor.Models
{
    public class PowerUpSettingsSo : ScriptableObject
    {
        private static string _powerUpFolderPathIdsCached = "Assets/Systems/PowerUps/PowerUpIds";
        public static string PowerUpFolderPathIds
        {
            get => _powerUpFolderPathIdsCached;

            set
            {
                _powerUpFolderPathIdsCached = value;
                
                if (!AssetDatabase.IsValidFolder(_powerUpFolderPathIdsCached))
                {
                    string[] folders = _powerUpFolderPathIdsCached.Split('/');
                    string current = folders[0];
                    for (int i = 1; i < folders.Length; i++)
                    {
                        string next = $"{current}/{folders[i]}";
                        if (!AssetDatabase.IsValidFolder(next))
                        {
                            AssetDatabase.CreateFolder(current, folders[i]);
                        }
                        current = next;
                    }
                }
            }
        }
        
        public static List<Type> PowerUpTypes { get; private set; }

        
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            PowerUpTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(PowerUpComponent)))
                .ToList();
        }

        public static PowerUpComponentId CreatePowerUpIds(string powerUpId)
        {
            if (string.IsNullOrEmpty(powerUpId))
            {
                throw new ArgumentException("PowerUp ID cannot be null or empty.", nameof(powerUpId));
            }

            PowerUpComponentId newId = CreateInstance<PowerUpComponentId>();
            newId.id = powerUpId;

            string assetPath = $"{PowerUpFolderPathIds}/{newId.id}.asset";
            
            AssetDatabase.CreateAsset(newId, assetPath);
            AssetDatabase.SaveAssets();

            return newId;
        }
        
        public static void DeletePowerUpId(PowerUpComponentId powerUpId)
        {
            if (!powerUpId)
            {
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(powerUpId);
            if (string.IsNullOrEmpty(assetPath))
            {
                throw new ArgumentException("PowerUp ID is not a valid asset.", nameof(powerUpId));
            }

            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
        }
    }
}