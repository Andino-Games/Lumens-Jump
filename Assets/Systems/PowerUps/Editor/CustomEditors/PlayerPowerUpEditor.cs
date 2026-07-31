using System;
using System.Collections.Generic;
using System.Linq;
using Systems.PowerUps.Components;
using Systems.PowerUps.Editor.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.PowerUps.Editor.CustomEditors
{
    [CustomEditor(typeof(PlayerPowerUps))]
    public class PlayerPowerUpEditor : UnityEditor.Editor
    {
        private readonly Dictionary<PowerUpComponent, bool> _foldouts = new Dictionary<PowerUpComponent, bool>();
        private PlayerPowerUps _manager;

        public override VisualElement CreateInspectorGUI()
        {
            _manager = (PlayerPowerUps) target;
            _manager.powerUps = _manager.GetComponents<PowerUpComponent>().ToList();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("PowerUps", EditorStyles.boldLabel);

            foreach (PowerUpComponent powerUp in _manager.powerUps)
            {
                if (!powerUp) continue;
                
                _foldouts.TryAdd(powerUp, true);
                
                _foldouts[powerUp] = EditorGUILayout.Foldout(
                    _foldouts[powerUp],
                    powerUp.GetType().Name,
                    true,
                    EditorStyles.foldoutHeader
                );
                
                if (_foldouts[powerUp])
                {
                    EditorGUILayout.BeginVertical("box");

                    PowerUpComponentEditor powerUpEditor = CreateEditor(powerUp) as PowerUpComponentEditor;
                    powerUpEditor?.OnInspectorGUI();

                    if (GUILayout.Button("Remove"))
                    {
                        DestroyImmediate(powerUp);
                        _foldouts.Remove(powerUp);
                        PowerUpSettingsSo.DeletePowerUpId(powerUp.powerUpComponentId);
                        break;
                    }

                    EditorGUILayout.EndVertical();
                }
                
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("+ Add PowerUp"))
            {
                ShowAddMenu(_manager);
            }
        }

        private void ShowAddMenu(PlayerPowerUps manager)
        {
            List<Type> types = PowerUpSettingsSo.PowerUpTypes;
            
            GenericMenu menu = new GenericMenu();
            if (types.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("No PowerUps available"));
                menu.ShowAsContext();
                return;
            }
            
            foreach (Type type in types)
            {
                menu.AddItem(new GUIContent(type.Name), false, () =>
                {
                    Component component = Undo.AddComponent(manager.gameObject, type);
                    component.hideFlags = HideFlags.HideInInspector;
                    EditorUtility.SetDirty(manager.gameObject);
                    _manager.powerUps.Add((PowerUpComponent) component);
                });
            }
            
            menu.ShowAsContext();
        }
    }
}