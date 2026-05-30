using Systems.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Utils
{
    public class IntroPass : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionText;
        [SerializeField] private UnityEvent onIntroComplete;
        
        private void Start()
        {
            versionText.text = "v." + Application.version;
            
            // Si es un restart desde GameOver, saltar la intro y cargar el menú directamente
            if (SceneManager.IsRestarting)
            {
                ChangeToMenuScene();
                return;
            }
            
            Invoke(nameof(ChangeToMenuScene), 6.5f);
        }
        
        private void ChangeToMenuScene()
        {
            LoadMainMenu();
            onIntroComplete?.Invoke();
        }

        private void LoadMainMenu()
        {
            SceneManager.Instance.LoadScene("MainMenuScene");
        }
    }
}