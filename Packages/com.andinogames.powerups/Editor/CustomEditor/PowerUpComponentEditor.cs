using PowerUps.Editor.Models;
using UnityEditor;
using UnityEngine;

namespace PowerUps.Editor
{
    [CustomEditor(typeof(PowerUpComponent), true)]
    public class PowerUpComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            PowerUpComponent comp = (PowerUpComponent) target;
            
            if (comp.powerUpComponentId)
            {
                string newId = EditorGUILayout.TextField("Id", comp.powerUpComponentId.id);
                
                if (newId != comp.powerUpComponentId.id)
                {
                    Undo.RecordObject(comp.powerUpComponentId, "Modify PowerUp ID");
                    comp.powerUpComponentId.id = newId;
                    EditorUtility.SetDirty(comp.powerUpComponentId);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("This PowerUp does not have an ID assigned. Please create a new ID.", MessageType.Error);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Id", "NONE REFERENCE");
                if (GUILayout.Button("Create New ID", GUILayout.MaxWidth(120)))
                {
                    string id = comp.GetType().Name + "_" + System.Guid.NewGuid().ToString("N").Substring(0,4);
                    PowerUpComponentId newId = PowerUpSettingsSo.CreatePowerUpIds(id);

                    Undo.RecordObject(comp, "Assign PowerUp ID");
                    comp.powerUpComponentId = newId;
                    EditorUtility.SetDirty(comp);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            SerializedProperty prop = serializedObject.GetIterator();
            bool enterChildren = true;
            
            while (prop.NextVisible(enterChildren))
            {
                enterChildren = false;
                switch (prop.name)
                {
                    case nameof(PowerUpComponent.powerUpComponentId):
                    case "m_Script":
                        continue;
                    default:
                        EditorGUILayout.PropertyField(prop, true);
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}