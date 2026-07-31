using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.PowerUps.Editor.Win
{
    public class AndinoGamesPowerUp : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;

        // [MenuItem("Tools/AndinoGames/AndinoGamesPowerUp")]
        // public static void OpenWindow()
        // {
        //     AndinoGamesPowerUp wnd = GetWindow<AndinoGamesPowerUp>();
        //     wnd.titleContent = new GUIContent("AndinoGamesPowerUp");
        // }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            VisualElement labelFromUxml = visualTreeAsset.Instantiate();
            root.Add(labelFromUxml);
        }
    }
}
