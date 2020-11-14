using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnJump : MonoBehaviour
{
    public bool getFlag;

    public GameObject player;
    public GameObject flag;
    public GameObject arrow;

    public Text Stage;
    public Text Prompt;
    public Text Instructions;


    // Start is called before the first frame update
    void Start()
    {
        getFlag = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (getFlag == true)
        {
            Stage.text = "Getting Points";
            Prompt.text = "Go over to the player with the Candy Basket. Follow the arrow!";
            Instructions.text = "Depending on the party, the object players fight to hold onto changes." +
                " Go ahead and walk over the Candy Basket to pick it up. ";
        }
        getFlag = false;
    }

    void OnCollisionEnter(Collision other)
    {
        arrow.transform.position = flag.transform.position;
        getFlag = true;

    }
}
