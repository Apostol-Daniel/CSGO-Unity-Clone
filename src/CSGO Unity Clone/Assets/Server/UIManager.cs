using UnityEngine;
using UnityEngine.UI;

namespace Assets.Server
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager UIManagerInstance;

        public GameObject StartMenu;
        public GameObject Player;
        public InputField UsernameField;

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
            StartMenu.SetActive(false);
            UsernameField.interactable = false;
            Player.SetActive(true);
        }

        public void EscapeToMainMenu() 
        {
            Player.SetActive(false);
            StartMenu.SetActive(true);
            UsernameField.interactable = true;
        }
    }
}
