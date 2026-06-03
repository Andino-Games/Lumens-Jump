using Systems.Audio;
using Systems.Manager;
using Systems.UI.MouseClick;
using UnityEngine;

namespace Systems.UI
{
    public class MenuController : MonoBehaviour
    {
        private void Start()
        {
            MouseClicks.Instance.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            SceneManager.Instance.LoadScene("GameScene_Test");
            AdsManager.Instance.ResetRevive();
        }
        
        public void ShowMainMenu()
        {
            SceneManager.Instance.LoadScene("MainMenuScene");
            AdsManager.Instance.ResetRevive();
        }

        public void PlayUISfx(string sfxName)
        {
            AudioManager.Instance.PlaySfx(sfxName);
        }
    }
}