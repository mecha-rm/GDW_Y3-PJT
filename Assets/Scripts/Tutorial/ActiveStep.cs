using System.Collections.Generic;
using UnityEngine;

public class ActiveStep : Step
{
    // the subject being watched 
    public GameObject subject = null;

    // a list of objects that are relevant to the step.
    public List<GameObject> objects;

    // if 'true', the objects pause the game when active.
    public bool objectsPause = true;

    // watch to see when the object becomes active, or inactive
    public bool watchForActive = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // sets whether all objects should be active or not.
    public void SetActiveObjects(bool active)
    {
        foreach (GameObject entity in objects)
            entity.SetActive(active);

        // if the objects pause the game when active
        if (objectsPause && objects.Count > 0)
        {
            Time.timeScale = (active) ? 0.0F : 1.0F;
        }
    }

    // activate all objects
    public void ActivateAllObjects()
    {
        SetActiveObjects(true);
    }

    // deactivate all objects
    public void DeactivateAllObjects()
    {
        SetActiveObjects(false);
    }

    // called when this step starts.
    public override void OnStepActivation()
    {
        SetActiveObjects(true);
    }

    // called when the step is completed.
    public override void OnStepCompletion()
    {
        // makes all objects part of the step inactive.
        SetActiveObjects(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // trigger
        if (subject.activeSelf == watchForActive && checklist.GetCurrentStep() == this)
        {
            checklist.CompleteStep();
        }
    }
}
