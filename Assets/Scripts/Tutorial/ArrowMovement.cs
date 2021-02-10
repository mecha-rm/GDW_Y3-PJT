using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// movement for moving an arrow.
public class ArrowMovement : MonoBehaviour
{
    // should be attached to the moving object.
    // transform
    public Transform movingArrow;

    // minimum and maximum Y for movement.
    float minY = 5.0F;
    float maxY = 15.0F;

    // speed
    public float speed = 10.0f;

    // start and end point
    // public Vector3 startPoint;
    // public Vector3 endPoint;

    // time value
    // private float t;
    // 
    // // direction
    // private bool forward = true;

    // Start is called before the first frame update
    void Start()
    {
        if (movingArrow == null)
            movingArrow = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Moves arrow up and down using LERP
        if(movingArrow.position.y > maxY) // 15.0F
        {
            // speed = 10.0f;
            speed *= -1;
        }
        if(movingArrow.position.y < minY) // 5.0F
        {
            // speed = -10.0f;
            speed *= -1;
        }

        // the rotation must be cleared in order for the arrow to move in the right direction.
        // so the rotation gets removed for the translation, then gets added back in.
        Quaternion rot = movingArrow.rotation;
        movingArrow.rotation = new Quaternion(0, 0, 0, 1);
        movingArrow.Translate(0, speed * Time.deltaTime, 0);
        movingArrow.rotation = rot;

      

        // ver. 2
        // Vector3 direcNorm = (endPoint - transform.position).normalized;
        // transform.Translate(direcNorm * speed * Time.deltaTime);

        // ver. 3
        // new position
        // Vector3 newPos;
        // 
        // // delta time variable
        // t += Time.deltaTime * speed;
        // t = Mathf.Clamp(t, 0, 1);
        // 
        // // determines what direction should be gone in.
        // if (forward)
        //     newPos = Vector3.Lerp(startPoint, endPoint, t);
        // else
        //     newPos = Vector3.Lerp(endPoint, startPoint, t);
        // 
        // // reverse direction
        // if(t >= 1.0F)
        // {
        //     t = 0.0F;
        //     forward = !forward;
        // }
    }
}
