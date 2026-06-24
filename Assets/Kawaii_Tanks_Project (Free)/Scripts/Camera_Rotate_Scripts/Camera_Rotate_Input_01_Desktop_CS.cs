using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Camera_Rotate_Input_01_Desktop_CS : Camera_Rotate_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        [Tooltip("카메라 회전 속도")] public float keyRotationSpeed = 60f; // The speed of camera rotation by key input.

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

            // J = Left, L = Right, I = Up, K = Down
            if (Input.GetKey(KeyCode.J))
            {
                horizontal = -1f;
            }
            else if (Input.GetKey(KeyCode.L))
            {
                horizontal = 1f;
            }
            if (Input.GetKey(KeyCode.K))
            {
                vertical = 1f;
            }
            else if (Input.GetKey(KeyCode.I))
            {
                vertical = -1f;
            }

            cameraRotateScript.rotationInput = new Vector3(0f, horizontal, vertical) * keyRotationSpeed * Time.deltaTime;
        }

#endif
    }

}