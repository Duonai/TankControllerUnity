using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Fire_Control_Input_02_Desktop_CS : Fire_Control_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        private UnityServer server;
        private UnityClient client;
        private void Start()
        {
            if (transform.parent.CompareTag("Enemy"))
                server = GameObject.Find("VersusServer").GetComponent<UnityServer>();
            else if (transform.parent.CompareTag("Enemy2"))
                client = GameObject.Find("VersusClient").GetComponent<UnityClient>();
        }

        public override void Get_Input()
        {
            //Debug.Log("Fire Input: " + fire);
            //Debug.Log($"Get_Input : {server.fire}");

            if (transform.parent.CompareTag("Enemy"))
            {
                if (server.fire2P)
                {
                    fireControlScript.Fire();
                    server.fire2P = false; // Reset the fire input after firing
                }
            }
            else if (transform.parent.CompareTag("Enemy2"))
            {
                if (client.fire1P)
                {
                    fireControlScript.Fire();
                    client.fire1P = false; // Reset the fire input after firing
                }
            }
        }
#endif
    }

}