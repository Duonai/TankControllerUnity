using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Camera_Rotate_Input_02_Desktop_CS : Camera_Rotate_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        [Tooltip("카메라 회전 속도")] public float keyRotationSpeed = 30f; // The speed of camera rotation by key input.

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
            cameraRotateScript.rotationInput = Key_Bindings_CS.GetCameraRotationAxis() * General_Settings_CS.cameraRotationSensibility;

            //float horizontal = 0f;
            //float vertical = 0f;

            //float pitch = 0.0f;
            //float yaw = 0.0f;

            //if (Input.GetKeyDown(KeyCode.L))
            //    pitch = 1f;
            //else if (Input.GetKeyUp(KeyCode.L))
            //    pitch = 0f;
            //if (Input.GetKeyDown(KeyCode.J))
            //    pitch = -1f;
            //else if (Input.GetKeyUp(KeyCode.J))
            //    pitch = 0f;

            //if (Input.GetKeyDown(KeyCode.I))
            //    yaw = 1f;
            //else if (Input.GetKeyUp(KeyCode.I))
            //    yaw = 0f;
            //if (Input.GetKeyDown(KeyCode.K))
            //    yaw = -1f;
            //else if (Input.GetKeyUp(KeyCode.K))
            //    yaw = 0f;

            //// J = Left, L = Right, I = Up, K = Down
            //if (pitch != 0)
            //{
            //    horizontal += pitch * 1.2f;
            //}
            //if (yaw != 0)
            //{
            //    vertical -= yaw * 0.5f;
            //}

            //cameraRotateScript.rotationInput = new Vector3(0f, horizontal, vertical) * keyRotationSpeed * Time.deltaTime;
        }

#endif
    }

}