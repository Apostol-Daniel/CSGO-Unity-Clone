using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Server
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager UIManagerInstance;

        public GameObject StartMenu;
        public GameObject Player;
        public InputField InputUsernameField;
        public InputField InputIpField;
        public Button ButtonMultiplayer;
        public Button ButtonSinglePlayer;
        public Button ButtonConnectToLocalhost;
        public Button ButtonConnectInputIp;
        public Button ButtonBackToMainMenu;
        

        private void Awake()
        {
            if (UIManagerInstance == null)
            {
                UIManagerInstance = this;
            }

            else if (UIManagerInstance != this)
            {
                Debug.Log("Instance already exists, detroying object.");
                Destroy(this);
            }
        }

        public static UIManager Instance() 
        {
            return UIManagerInstance;
        }

        public void ConnetToLocalhost()
        {
            Cursor.lockState = CursorLockMode.Locked;

            StartMenu.SetActive(false);
            InputUsernameField.interactable = false;
            Client.ClientInstance.ConnectToLocalhost();
        }

        public void ConnetToGivenIp()
        {
            Cursor.lockState = CursorLockMode.Locked;
            StartMenu.SetActive(false);
            InputUsernameField.interactable = false;
            InputIpField.interactable = false;           

            Client.ClientInstance.ConnectToGivenIp(InputIpField.text);
        }

        public void StartSinglePlayer()
        {
            Cursor.lockState = CursorLockMode.Locked;
            StartMenu.SetActive(false);
            InputUsernameField.interactable = false;
            Player.SetActive(true);
        }

        public void EscapeToMainMenuSinglePlayer() 
        {
            Player.SetActive(false);
            StartMenu.SetActive(true);
            InputUsernameField.interactable = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void EscapeToMainMenuSingleMultiplayer()
        {
            Client.Instance().Disconnect();
            Client.Instance().PacketHandlers.Clear();
            GameManager.Instance().ClearData();
            GameManager.Instance().Reset();
            EscapeToMainMenuSinglePlayer();
        }

        public void ButtonMultiplayerOnClick() 
        {
            ButtonMultiplayer.gameObject.SetActive(false);
            ButtonSinglePlayer.gameObject.SetActive(false);
            InputUsernameField.gameObject.SetActive(true);
            InputUsernameField.interactable = true;
            InputIpField.gameObject.SetActive(true);
            InputIpField.interactable = true;

            ButtonConnectToLocalhost.gameObject.SetActive(true);
            ButtonConnectInputIp.gameObject.SetActive(true);
            ButtonBackToMainMenu.gameObject.SetActive(true);
        }

        public void ButtonBackToMaineMenuOnClick() 
        {
            ButtonBackToMainMenu.gameObject.SetActive(false);
            ButtonConnectToLocalhost.gameObject.SetActive(false);
            ButtonConnectInputIp.gameObject.SetActive(false);
            InputUsernameField.gameObject.SetActive(false);
            InputIpField.gameObject.SetActive(false);

            ButtonSinglePlayer.gameObject.SetActive(true);
            ButtonMultiplayer.gameObject.SetActive(true);

        }
    }
}
