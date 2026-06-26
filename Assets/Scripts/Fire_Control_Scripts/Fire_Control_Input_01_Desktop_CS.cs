using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Fire_Control_Input_01_Desktop_CS : Fire_Control_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        private Socket_Communicator2_CS socket;

        private void Start()
        {
            socket = GameObject.Find("Communicator2").GetComponent<Socket_Communicator2_CS>();
        }

        public override void Get_Input()
        {
            //if (Key_Bindings_CS.IsFireKeyPressing())
            //{
            //    fireControlScript.Fire();
            //}

            string fire = socket.fire;

            //Debug.Log("Fire Input: " + fire);

            if (fire != "idle")
            {
                fireControlScript.Fire();
            }
        }
#endif
    }

}