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
        public InputField UsernameField;
        public Button ButtonMultiplayer;
        public Button ButtonSinglePlayer;
        public Button ButtonConnectToServer;
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

        public void ConnectToServer()
        {
            StartMenu.SetActive(false);
            UsernameField.interactable = false;
            Client.ClientInstance.ConnectToServer();
        }

        public void StartSinglePlayer()
        {
            Cursor.lockState = CursorLockMode.Locked;
            StartMenu.SetActive(false);
            UsernameField.interactable = false;
            Player.SetActive(true);
        }

        public void EscapeToMainMenu() 
        {
            Player.SetActive(false);
            StartMenu.SetActive(true);
            UsernameField.interactable = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MultiplayerButtonOnClick() 
        {
            ButtonMultiplayer.gameObject.SetActive(false);
            ButtonSinglePlayer.gameObject.SetActive(false);
            UsernameField.gameObject.SetActive(true);
            UsernameField.interactable = true;
            ButtonConnectToServer.gameObject.SetActive(true);
        }
    }
}
