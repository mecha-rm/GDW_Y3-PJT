// References
// * https://www.youtube.com/watch?v=lYIRm4QEqro&feature=emb_rel_pause
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // TODO: add ability to have momentum.

    // Init the movement speed of camera and player
    public float cameraSpeed = 20.0f;
    public float movementSpeed = 20.5f;

    // rotations
    float pitch = 0.0f;
    float yaw = 0.0f;

    // locks the keys to prevent the object from moving using the keyboard or mouse.
    public bool controlLock = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Moving Camera based on mouse movement
        yaw += cameraSpeed * Input.GetAxis("Mouse X");
        pitch -= cameraSpeed * Input.GetAxis("Mouse Y");

        //Setting a min,max for camera movement, can be removed for full rotations
        //yaw = Mathf.Clamp(yaw, -90f, 90f);
        //pitch = Mathf.Clamp(pitch, -60f, 90f);

        // if the keys are not locked.
        if (!controlLock)
        {
            // if the right mouse button is held down, then the object rotates to face that direction.
            // https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html
            if (Input.GetMouseButton(1)) // 0 = primary, 1 = secondary, 2 = middle
            {
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            }


            // Moving Camera using WASD

            // forward movement and backward movement
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -movementSpeed * Time.deltaTime));
            }

            // leftward and rightward movement
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-movementSpeed * Time.deltaTime, 0, 0));
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(movementSpeed * Time.deltaTime, 0, 0));
            }

            // upward movmenet and downward movement
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(new Vector3(0, movementSpeed * Time.deltaTime, 0));
            }
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(new Vector3(0, -movementSpeed * Time.deltaTime, 0));
            }
        }
    }
}
