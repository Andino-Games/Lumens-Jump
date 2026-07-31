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
            SceneManager.Instance.LoadScene("GameScene");

            AdsManager.Instance.ResetRevive();
        }
        
        public void ShowMainMenu()
        {
            SceneManager.Instance.RestartGame();

            AdsManager.Instance.ResetRevive();

            AudioManager.Instance.PlayAmb("Amb");
            AudioManager.Instance.PlayMusic("Music");
        }

        public void PlayUISfx(string sfxName)
        {
            AudioManager.Instance.PlaySfx(sfxName);
        }
    }
}