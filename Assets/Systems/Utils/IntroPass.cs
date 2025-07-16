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