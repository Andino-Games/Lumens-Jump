using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PowerUps.Editor.Models
{
    [CustomEditor(typeof(PowerUpSettingsSo))]
    public class PowerUpSettingsSoEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            InspectorElement.FillDefaultInspector(container, serializedObject, this);
            return container;
        }
    }
}