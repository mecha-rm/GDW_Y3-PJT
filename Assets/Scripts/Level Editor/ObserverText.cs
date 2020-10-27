using GED;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// observes a subject and changes the text appropriately
public class ObserverText : StateObserver
{
    // the text object
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (subject != null)
            SetSubject(subject.GetComponent<StateMachine>());
    }

    // on state changed
    public override void OnStateChange()
    {
        GameObject selected = subject.GetComponent<UI_Manager>().GetSelectedObject();
        string newText = ""; // new text
        int stateNum = subject.GetComponent<StateMachine>().GetStateNumber();
           
        switch(stateNum)
        {
            case 0: // no selection
                newText = "Selection: None";
                text.text = newText;
                break;
            case 1:
                // gets the name of the transform object.
                // this isn't working for some reason.
                // if(selected.GetComponent<ObjectScript>() != null)
                // {
                //     newText = "Selected: " + GetComponent<ObjectScript>().name + "(" 
                //         + subject.GetComponent<UI_Manager>().GetSelectedObject().transform.name + ")";
                // }
                // else
                {
                    newText = "Selected: " + selected.transform.name;
                }

                text.text = newText;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.LogError("Update");
      
    }
}
