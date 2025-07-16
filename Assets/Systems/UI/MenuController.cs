using System;
using Systems.Audio;
using Systems.Manager;
using Systems.UI.MouseClick;
using UnityEngine;

namespace Systems.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject creditsPanel;

        private void Start()
        {
            MouseClicks.Instance.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            SceneManager.Instance.LoadScene("GameScene");
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
            SceneManager.Instance.LoadScene("MainMenuScene");
        }

        public void PlayUISfx(string sfxName)
        {
            AudioManager.Instance.PlaySfx(sfxName);
        }
    }
}