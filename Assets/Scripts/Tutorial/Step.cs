using UnityEngine;

// a step in the list
public abstract class Step : MonoBehaviour
{
    // the checklist
    public Checklist checklist = null; // originally protected

    // the number of the step in the list. Step numbers start at 1.
    public string title = "";
    public string description = "";

    // Start is called before the first frame update
    void Start()
    {

    }

    // gets the checklist this step is part of.
    public Checklist GetChecklist()
    {
        return checklist;
    }

    // called when a step has been added to the list.
    public virtual void OnStepAddition(Checklist newList)
    {
        checklist = newList;
    }

    // called when the step is activated (i.e. it is now the current step)
    public abstract void OnStepActivation();

    // called when a step is completed.
    public abstract void OnStepCompletion();

    // checks to see if this is the current step
    public bool IsCurrentStep()
    {
        // if a list has been added, check to see if this is the current step.
        if (checklist != null)
            return this == checklist.GetCurrentStep();

        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
