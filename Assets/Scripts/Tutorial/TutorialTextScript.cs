using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextScript : MonoBehaviour
{
    //Texts that are displayed onto the screen
    public Text Stage; //Stage of tutorial, ie. Movement, Gaining Points, etc.
    public Text Prompt; //prompts user, ie. Move to where the arrow is pointing at
    public Text Instructions; //instructions to help player fulfill prompt

    // Start is called before the first frame update
    void Start()
    {
        //First prompt 
        Stage.text = "Movement";
        Prompt.text = "Make your way around the spiders to the yellow platform!";
        Instructions.text = "Use the 'W' and 'S' to forwards and backwards. " +
            "Use 'A' and 'D' to rotate left and right.";

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
