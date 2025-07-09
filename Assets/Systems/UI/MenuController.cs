using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject creditsPanel;
        
        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }
        
        public void ShowCredits()
        {
            
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            
            Application.Quit();
        }
        
        public void ShowMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}