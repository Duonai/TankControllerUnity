using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Camera_Rotate_Input_01_Desktop_CS : Camera_Rotate_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        [Tooltip("카메라 회전 속도")] public float keyRotationSpeed = 30f; // The speed of camera rotation by key input.

        private Socket_Communicator2_CS socket;
        private Socket_Communicator3_CS socket3;

        private void Start()
        {
            socket = GameObject.Find("Communicator2").GetComponent<Socket_Communicator2_CS>();
            try { socket3 = GameObject.Find("Communicator3").GetComponent<Socket_Communicator3_CS>(); } catch { }
        }

        public override void Get_Input()
        {
            /*
            // Cancel while the cursor is displayed.
            if (Cursor.lockState == CursorLockMode.None)
            {
                cameraRotateScript.rotationInput = Vector3.zero;
                return;
            }
            */

            // Get the input.
            // cameraRotateScript.rotationInput = Key_Bindings_CS.GetCameraRotationAxis() * General_Settings_CS.cameraRotationSensibility;

            float horizontal = 0f;
            float vertical = 0f;

            float pitch = socket.turret_yaw;
            float yaw = socket.turret_pitch;

            if (socket3 != null)
            {
                if (socket3.command == "turret_rotate_right")
                {
                    pitch = 1.2f;
                }
                else if (socket3.command == "turret_rotate_left")
                {
                    pitch = -1.2f;
                }
                else if (socket3.command == "turret_up")
                {
                    yaw = 1.0f;
                }
                else if (socket3.command == "turret_down")
                {
                    yaw = -1.0f;
                }
            }

            // J = Left, L = Right, I = Up, K = Down
            if (pitch != 0)
            {
                horizontal += pitch * 1.2f;
            }
            if (yaw != 0)
            {
                vertical -= yaw * 0.5f;
            }

            cameraRotateScript.rotationInput = new Vector3(0f, horizontal, vertical) * keyRotationSpeed * Time.deltaTime;
        }

#endif
    }

}