using UnityEngine;
using UnityEngine.SceneManagement;


namespace ChobiAssets.KTP
{

    public class Fire_Control_Input_01_Desktop_CS : Fire_Control_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        private Socket_Communicator2_CS socket;
        private Socket_Communicator3_CS socket3;
        private UnityServer server;
        private UnityClient client;

        public int ammoCount = 5;
        float reloadCount = 0;
        bool reloading = false;
        public Ammo_Script_CS ammoText;

        private void Start()
        {
            try { socket = GameObject.Find("Communicator2").GetComponent<Socket_Communicator2_CS>(); } catch { }
            try { socket3 = GameObject.Find("Communicator3").GetComponent<Socket_Communicator3_CS>(); } catch { }
            try { ammoText = GameObject.Find("Game_Controller").transform.Find("Canvas_Texts/Text_Ammo").GetComponent<Ammo_Script_CS>(); } catch { }

            if (transform.parent.CompareTag("Player"))
                 try { server = GameObject.Find("VersusServer").GetComponent<UnityServer>(); } catch { }
            else if (transform.parent.CompareTag("Player2"))
                 try { client = GameObject.Find("VersusClient").GetComponent<UnityClient>(); } catch { }
        }

        public override void Get_Input()
        {
            //if (Key_Bindings_CS.IsFireKeyPressing())
            //{
            //    fireControlScript.Fire();
            //}

            string fire = socket.fire;

            //Debug.Log("Fire Input: " + fire);

            if (ammoText != null)
                ammoText.ammoCount = ammoCount;

            if (ammoCount <= 0)
            {
                string sceneName = SceneManager.GetActiveScene().name;
                if (socket3 != null)
                {
                    if (socket3.reloading)
                    {
                        reloadCount = 1.0f;
                        socket3.reloading = false;
                    }
                }
                else if (reloading == false)
                {
                    reloadCount = 3.0f;
                    reloading = true;
                }

                if (reloadCount > 0.0f)
                {
                    ammoText.reloading = true;
                    reloadCount -= Time.deltaTime;

                    if (reloadCount <= 0f)
                    {
                        ammoCount = 5;
                        ammoText.reloading = false;
                        reloading = false;
                    }
                }

                return;
            }

            if (fire != "idle")
            {
                ammoCount--;
                fireControlScript.Fire();
                if (transform.parent.CompareTag("Player") && server != null)
                    server.fire = true;
                else if (transform.parent.CompareTag("Player2") && client != null)
                    client.fire = true;
            }
        }
#endif
    }

}