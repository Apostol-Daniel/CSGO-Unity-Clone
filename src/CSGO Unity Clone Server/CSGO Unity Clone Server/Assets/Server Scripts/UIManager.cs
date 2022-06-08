using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager UIManagerInstance;

        public GameObject StartMenu;
        public Button ButtonHostOnLocalhost;
        public Button ButtonHostOnIpv4;


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
        
        public void HostOnLocalHost() 
        {
            StartMenu.SetActive(false); ;
            Server.StartOnLocalhost(10, 26950);
        }
    }
}
