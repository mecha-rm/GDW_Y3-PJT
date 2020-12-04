using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class used for setting up the tutorial
public class TutorialScript : MonoBehaviour
{
    // checklist
    public Checklist tutorialList;

    // the list of steps to be added
    public List<Step> stepList;

    // the text used to show the step.
    public Text stepText = null;

    // the current step
    private int currStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        // gets the checklist if it hasn't been set.
        if (tutorialList == null)
            tutorialList = GetComponent<Checklist>();

        // goes through the steps list and adds them to the Checklist
        for (int i = 0; i < stepList.Count; i++)
        {
            tutorialList.AddStep(stepList[i]);
        }

        // active tutorial checklist
        tutorialList.SetActiveList(true);

        // if the tutorial list is active, show the text
        // if(tutorialList != null && tutorialList.activeList)
        // {
        //     if (!tutorialList.IsCompleteList())
        //         stepText.text = "Step " + tutorialList.GetCurrentStepNumber() + " - " + 
        //             tutorialList.GetCurrentStep().name + ": " + tutorialList.GetCurrentStep().description;
        //     else
        //         stepText.text = "Tutorial is Complete!";
        // 
        //     currStep++;
        // }


        // steps.Add(new Step());
    }

    // pauses the tutorial when a prompt appears
    public void PauseTutorial()
    {
        Time.timeScale = 0.0F;
    }

    // unpauses the tutorial when a prompt disappears
    public void UnpauseTutorial()
    {
        Time.timeScale = 1.0F;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialList.activeList)
        {
            // step as changed
            if (currStep < tutorialList.GetCurrentStepNumber() && !tutorialList.IsCompleteList())
            {
                currStep++;

                stepText.text = "Step " + tutorialList.GetCurrentStepNumber() + " - " +
                    tutorialList.GetCurrentStep().title + ":\n" + tutorialList.GetCurrentStep().description;
            }
            // if all steps have been completed.
            else if (tutorialList.IsCompleteList())
            {
                string str = "All Steps Completed!";

                if (stepText.text != str)
                    stepText.text = str;
            }

        }
    }
}
