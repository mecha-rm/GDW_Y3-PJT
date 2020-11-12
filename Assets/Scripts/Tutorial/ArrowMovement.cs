using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public Transform MovingArrow;   
    float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Moves arrow up and down using LERP
        if(MovingArrow.position.y > 15.0f)
        {
            speed = 10.0f;
        }
        if(MovingArrow.position.y < 5.0f)
        {
            speed = -10.0f;
        }

        MovingArrow.Translate(0, speed * Time.deltaTime, 0);
    }
}
