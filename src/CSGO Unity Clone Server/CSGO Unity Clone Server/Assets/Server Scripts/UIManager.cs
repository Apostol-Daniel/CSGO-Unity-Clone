using Assets.Server_Scripts;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
        public Button ButtonDisconnect;
        public Button ButtonMainMenu;
        public Button ButtonConnectToChosenIp;
        public InputField InputHostedOn;
        public Dropdown SlcServerIps;


        private void Awake()
        {
            PopulateSlcServerIps();
            InputHostedOn.interactable = false;
            ButtonDisconnect.gameObject.SetActive(false);

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
        
        public void ButtonHostOnLocalhostOnClick() 
        {
            StartMenu.SetActive(false);
            ButtonDisconnect.gameObject.SetActive(true);           
            Server.StartOnLocalhost(10, 26950);
        }

        public void ButtonHostOnIpv4OnClick() 
        {
            ButtonHostOnLocalhost.gameObject.SetActive(false);
            ButtonHostOnIpv4.gameObject.SetActive(false);            
            SlcServerIps.gameObject.SetActive(true);
            ButtonMainMenu.gameObject.SetActive(true);
            ButtonConnectToChosenIp.gameObject.SetActive(true);
        }

        public void ButtonConnectToChosenIpOnClick() 
        {
            StartMenu.SetActive(false);
            ButtonDisconnect.gameObject.SetActive(true);
            Server.StartOnIPV4(10, 26950);
        }

        public void ButtonDisconnectOnClick()
        {
            StartMenu.SetActive(true);
            ButtonDisconnect.gameObject.SetActive(false);
            InputHostedOn.text = "";
            EnemySpawnerHolder.Instance().ClearEnemyCountOnDisconnet();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Server.Stop();
        }

        public void ButtonMainMenuOnClick() 
        {
            ButtonHostOnLocalhost.gameObject.SetActive(true);
            ButtonHostOnIpv4.gameObject.SetActive(true);
            ButtonMainMenu.gameObject.SetActive(false);
            SlcServerIps.gameObject.SetActive(false);
            ButtonConnectToChosenIp.gameObject.SetActive(false);
        }

        public void PopulateSlcServerIps() 
        {
            SlcServerIps.options.Clear();

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    SlcServerIps.options.Add(new Dropdown.OptionData() { text = ip.ToString() });
                }
            }                     
        }       
    }
}
