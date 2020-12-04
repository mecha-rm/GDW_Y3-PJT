// observes a state machine
public class StepObserver : StateObserver
{
    // the step
    public Step step;

    // the state value for a started step.
    public int stepStartState = 0;

    // the state value for a completed step.
    public int stepEndState = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // called when the state is changed.
    // for the step observer, it is called when the step is completed.
    public override void OnStateChange()
    {
        // Checklist cl = step.GetChecklist();
        Checklist cl = step.checklist;

        // if the current step is set to the observer, then the step is complete.
        if (subject.GetStateNumber() == stepEndState && cl.GetCurrentStep() == step)
        {
            // the step has been completed.
            cl.CompleteStep();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
