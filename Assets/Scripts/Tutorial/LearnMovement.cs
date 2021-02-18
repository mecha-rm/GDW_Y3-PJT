using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnMovement : MonoBehaviour
{
    public bool learnJump;

    public GameObject player;
    public GameObject flag;
    public GameObject arrow;
    public GameObject jumpPlatform;

    public Text Stage;
    public Text Prompt;
    public Text Instructions;


    // Start is called before the first frame update
    void Start()
    {
        learnJump = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (learnJump == true)
        {
            Stage.text = "Pumpkin Jumping";
            Prompt.text = "Follow the arrow to the big pumpkin!";
            Instructions.text = "Use the SPACE bar to jump. Use WASD to control where you are heading.";
            learnJump = false;

            // arrow doesn't move over to next step. This was an attempt to fix it (it didn't work).
            // arrow.transform.position = new Vector3(transform.position.x, arrow.transform.position.y, transform.position.z);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        arrow.transform.position = jumpPlatform.transform.position;
        learnJump = true;

    }
}
