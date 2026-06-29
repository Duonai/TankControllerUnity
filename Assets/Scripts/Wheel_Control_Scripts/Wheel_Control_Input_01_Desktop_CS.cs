using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Wheel_Control_Input_01_Desktop_CS : Wheel_Control_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        Vector2 currentAxis;
        Vector2 targetAxis;
        float turnVelocity;

        [HideInInspector] public float rightTrackInput = 0.0f;
        [HideInInspector] public float leftTrackInput = 0.0f;

        private Socket_Communicator_CS socket;
        private UnityServer server;
        private UnityClient client;

        private void Start()
        {
            socket = GameObject.Find("Communicator").GetComponent<Socket_Communicator_CS>();

            if (transform.parent.CompareTag("Player"))
                server = GameObject.Find("VersusServer").GetComponent<UnityServer>();
            else if (transform.parent.CompareTag("Player2"))
                client = GameObject.Find("VersusClient").GetComponent<UnityClient>();
        }

        public override void Get_Input()
        {
            // Get the input.
            //targetAxis = Key_Bindings_CS.GetMoveAxis();

            //// Turn.
            //currentAxis.x = Mathf.SmoothDamp(currentAxis.x, targetAxis.x, ref turnVelocity, 0.2f * Time.deltaTime);

            //// Forward and Backward.
            //var changeRate = 2.0f;
            //if (targetAxis.y == 0.0f)
            //{ // No input for forward and backward.

            //    if (Input.GetKey(Key_Bindings_CS.moveStopKeyCode))
            //    {
            //        // Stop quickly.
            //        targetAxis.y = 0.0f;
            //        changeRate = 4.0f;
            //    }
            //    else
            //    {
            //        if (currentAxis.x != 0.0f)
            //        { // Turning now.
            //            if (currentAxis.y != 0.0f)
            //            { // Not pivot-turning now.
            //                // Keep the minimum speed to turn smoothly.
            //                var tempAxisY = Mathf.Abs(currentAxis.y);
            //                tempAxisY = Mathf.Clamp(tempAxisY, 0.25f, 1.0f);
            //                targetAxis.y = tempAxisY * Mathf.Sign(currentAxis.y);
            //            }
            //        }
            //        else
            //        { // Not turning now.
            //            if (currentAxis.y > 0.0f)
            //            { // Going forward now.
            //                // Slow down gently.
            //                changeRate = 0.2f;
            //            }
            //        }
            //    }
            //}
            //currentAxis.y = Mathf.MoveTowards(currentAxis.y, targetAxis.y, changeRate * Time.deltaTime);

            //// Set the aixs.
            //wheelControlScript.moveAxis = currentAxis;

            leftTrackInput = socket.leftTrack;
            rightTrackInput = socket.rightTrack;

            Vector2 moveAxis;
            moveAxis.x = (leftTrackInput - rightTrackInput) * 0.5f;
            moveAxis.y = (leftTrackInput + rightTrackInput) * 0.5f;
            moveAxis.y = Mathf.Clamp(moveAxis.y, -0.5f, 1.0f);

            // Set the aixs.
            wheelControlScript.moveAxis = moveAxis;

            //for server 1p
            if (transform.parent.CompareTag("Player"))
            {
                server.bodyRotation = transform.eulerAngles.y;
                server.posX = transform.position.x;
                server.posZ = transform.position.z;
            }
            //for client 2p
            else if (transform.parent.CompareTag("Player2"))
            {
                client.bodyRotation = transform.eulerAngles.y;
                client.posX = transform.position.x;
                client.posZ = transform.position.z;
            }
        }
#endif
    }

}