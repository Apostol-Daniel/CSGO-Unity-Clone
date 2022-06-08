using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager UIManagerInstance;

        public GameObject StartMenu;
        public Button ButtonHostOnLocalhost;
        public Button ButtonHostOnIpv4;
        public InputField InputHostedOn;


        private void Awake()
        {
            InputHostedOn.interactable = false;

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
        
        public void HostOnLocalHost() 
        {
            StartMenu.SetActive(false); ;
            Server.StartOnLocalhost(10, 26950);
        }

        public void HostOnIPV4() 
        {
            StartMenu.SetActive(false); ;
            Server.StartOnIPV4(10, 26950);
        }

        public void EscapeToMainMenu()
        {
            StartMenu.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Server.Stop();
        }
    }
}
