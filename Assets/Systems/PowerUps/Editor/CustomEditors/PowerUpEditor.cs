using System.Collections.Generic;
using Systems.PowerUps.Components;
using UnityEditor;
using UnityEngine;

namespace Systems.PowerUps.Editor.CustomEditors
{
    [CustomEditor(typeof(PowerUp))]
    public class PowerUpEditor : UnityEditor.Editor
    {
        private List<PowerUpComponentId> _allIds = new List<PowerUpComponentId>();

        private void OnEnable()
        {
            _allIds = new List<PowerUpComponentId>(Resources.FindObjectsOfTypeAll<PowerUpComponentId>());
        }

        public override void OnInspectorGUI()
        {
            PowerUp powerUp = (PowerUp) target;
            
            AssignPowerUpButton(powerUp);
            
            EditorGUILayout.Space();

            ShowPowerUpComponentsIds(powerUp);
        
            
        }

        private void AssignPowerUpButton(PowerUp powerUp)
        {
            if (GUILayout.Button("Assign PowerUp"))
            {
                GenericMenu menu = new GenericMenu();

                foreach (PowerUpComponentId powerUpComponentId in _allIds)
                {
                    if (powerUpComponentId &&
                        !powerUp.powerUpComponentIds.Contains(powerUpComponentId))
                    {
                        string id = powerUpComponentId.id;
                        
                        menu.AddItem(new GUIContent(id), false, () =>
                        {
                            Undo.RecordObject(powerUp, "Assign PowerUpComponentId");
                            powerUp.powerUpComponentIds.Add(powerUpComponentId);
                            EditorUtility.SetDirty(powerUp);
                        });
                    }
                }

                if (menu.GetItemCount() == 0)
                {
                    menu.AddDisabledItem(new GUIContent("No PowerUps available"));
                }
                    
                menu.ShowAsContext();
            }
        }

        private void ShowPowerUpComponentsIds(PowerUp powerUp)
        {
            if (powerUp.powerUpComponentIds.Count > 0)
            {
                EditorGUILayout.LabelField("Assigned PowerUp IDs:", EditorStyles.boldLabel);
                foreach (PowerUpComponentId powerUpComponentId in powerUp.powerUpComponentIds)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(powerUpComponentId ? powerUpComponentId.id : "NULL", GUILayout.MaxWidth(200));
                    if (GUILayout.Button("Remove", GUILayout.MaxWidth(70)))
                    {
                        Undo.RecordObject(powerUp, "Remove PowerUpComponentId");
                        powerUp.powerUpComponentIds.Remove(powerUpComponentId);
                        EditorUtility.SetDirty(powerUp);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No PowerUp IDs assigned.", MessageType.Error);
            }
        }
    }
}