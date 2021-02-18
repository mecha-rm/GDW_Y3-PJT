using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnFlagSystem : MonoBehaviour
{
    public bool tutorialEnd;

    public GameObject player;
    public GameObject arrow;

    public Text Stage;
    public Text Prompt;
    public Text Instructions;

    // Start is called before the first frame update
    void Start()
    {
        tutorialEnd = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (tutorialEnd == true)
        {
            Stage.text = "Congrats";
            Prompt.text = "You have completed the tutorial!";
            Instructions.text = "Look at your score increase as you hold onto the Candy Basket." +
                " Good luck at your next party!";

            // arrow doesn't move over to next step. This was an attempt to fix it (it didn't work).
            // arrow.transform.position = new Vector3(transform.position.x, arrow.transform.position.y, transform.position.z);
        }

        tutorialEnd = false;
    }

    void OnCollisionEnter(Collision other)
    {
        tutorialEnd = true;

    }
}