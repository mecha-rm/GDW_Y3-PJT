using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    // transform
    public Transform movingArrow;   

    // speed
    public float speed = 5.0f;

    // end point
    public Vector3 endPoint;

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
        if(movingArrow.position.y > 15.0f)
        {
            speed = 10.0f;
        }
        if(movingArrow.position.y < 5.0f)
        {
            speed = -10.0f;
        }

        movingArrow.Translate(0, speed * Time.deltaTime, 0);
    }
}
