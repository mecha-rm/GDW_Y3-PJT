using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GED;

// Command Design Pattern - Undo/Redo
// log entry for undo/redo 
public struct LogEntry
{
    public GameObject entity; // the object this log applies to.

    // used for tracking object creation/deletion: -1 = deletion entry, 0 = transformation data, 1 = activation entry
    // the functionality was not completed in time, and thus is not used.
    public int type; 

    // the object is active (i.e. visible). 
    // This does not work because invisible objects are not updated, and thus can't track the change.
    public bool active; 

    // transformation variables
    public Vector3 position; // position
    public Quaternion rotation; // rotation
    public Vector3 localScale; // scale
}

// class for the undo and redo system.
public class UndoRedoSystem : MonoBehaviour
{
    // parents for undo and redo (unused)
    // the idea was to save the created/deleted object (or a copy) as a child of an empty object (which would not be visible).
    // if the user restores the object, it would be taken from that empty object and restored. This was not completed.
    // public static GameObject undoParent = new GameObject(); // create empty object
    // public static GameObject redoParent = new GameObject(); // create empty object

    // the undo list. When an action is made, an undo entry is logged.
    // when an action occurs, it puts an item on the undo list.
    // this is saved as a linked list since items at the front need to be deleted if the undo list surpasses a specific size.
    private static LinkedList<LogEntry> undoList = new LinkedList<LogEntry>();

    // a redo stack. When an item is pulled out of the undo list, it is placed on the redo stack.
    // if something new is placed in the undo list, then the redo stack is cleared.
    private static Stack<LogEntry> redoStack = new Stack<LogEntry>();

    // the undo limit for the undo-redo system.
    public static int undoLimit = 30;

    // NOTE: the built-in Unity undo/redo system does not allow for undoing and redoing deletions.
    // * however, you shouldn't be using it anyway.

    // Start is called before the first frame update
    void Start()
    {
    }

    // records the object and the action comitted. (TODO: make static)
    // public static void RecordObject(Object entity, string desc)
    // {
    //     Undo.RecordObject(entity, desc);
    // }

    // registers a created object, since created objects can not be destroyed by standard undo/redo functions.
    // https://docs.unity3d.com/ScriptReference/Undo.RegisterCreatedObjectUndo.html
    // public static void RegisterCreatedObject(Object entity, string desc)
    // {
    //     Undo.RegisterCreatedObjectUndo(entity, desc);
    // }

    // records the object and its transformation.
    public static void RecordAction(GameObject entity, int type, bool active, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        // records the action
        LogEntry entry;
        entry.entity = entity;
        entry.type = type;
        entry.active = active;

        entry.position = position;
        entry.rotation = rotation;
        entry.localScale = scale;

        
        // calls other function of the same name
        RecordAction(entry);
    }

    // records an action from a log entry
    public static void RecordAction(LogEntry entry)
    {
        undoList.AddFirst(entry);

        // undo limit has been reached.
        if(undoList.Count > undoLimit) // undo list has surpassed the undo limit
        {
            do
            {
                undoList.RemoveLast();
            } while (undoList.Count > undoLimit); // remove oldest logs to reduce size.
        }

        // redo stack is cleared out.
        // TODO: work out how to track object creation and destruction.
        redoStack.Clear();
    }

    // PerformUndo and PerformRedo can only undo the deletion of an object but not the creation of an object. They should not be used.
    // undos the last action.
    public static void UndoAction()
    {
        // Undo.PerformUndo();

        // if there are no actions to undo
        if (undoList.Count == 0)
            return;

        LogEntry e0; // the entry that will be undone and put on the redo stack.
        LogEntry e1; // the entry that will be at the top of the undo "stack" and will be the current action.
        // int typeTemp = 0; // temporary object for switching types

        e0 = undoList.First.Value; // gets the first entry
        // e1.entity = e0.entity; // gets the first entry again (this doesn't mean anything)

        // if the entity has been deleted.
        if(e0.entity == null)
        {
            undoList.RemoveFirst();
            UndoAction(); // calls the function again to deal with the redo operation
            return;
        }

        // object creation is being undone. This was not completed.
        // undoing object creation
        // if(undoList.First.Value.type == 1)
        // {
        //     undoList.First.Value.entity.gameObject.transform.SetParent(undoParent.transform);
        //     // undoParent;
        // }


        // steps
        // 1 - get transformation information from object, and save it to e1's PRS variables.
        // 2 - override object transformation information with data in e0's PRS variables
        // 3 - set e0's PRS variables to the PRS variables in e1

        // swaps transform values between game object and attached Transform object.
        // e1 gets the values from the object on e0. This is the same object that's on e1.
        // typeTemp = e0.type;
        // e1.type = e0.type;
        e1.active = e0.entity.active;
        e1.position = e0.entity.transform.position; // copies the object's current position
        e1.rotation = e0.entity.transform.rotation; // copies the object's current rotation
        e1.localScale = e0.entity.transform.localScale; // copies the object's local scale.

        // e0's object is overriden with the values of e0's transform.
        // e0.entity.alive;
        e0.entity.SetActive(e0.active);
        e0.entity.transform.position = e0.position;
        e0.entity.transform.rotation = e0.rotation;
        e0.entity.transform.localScale = e0.localScale;

        // e0's transform gets given the values from e1's transform
        // e0.alive
        e0.active = e1.active;
        e0.position = e1.position;
        e0.rotation = e1.rotation;
        e0.localScale = e1.localScale;

        undoList.RemoveFirst(); // removes first entry in the undo list.
        redoStack.Push(e0); // puts the entry on the redo stack.
        e0.entity.GetComponent<ObjectScript>().ResetPreviousTransform(); // resets the previous transformation to current transform 
    }

    // redos the last action.
    public static void RedoAction()
    {
        // Undo.PerformRedo();

        // if there are no actions to redo
        if (redoStack.Count == 0)
            return;

        LogEntry e0; // the entry that will be redone and put on the undo "stack".
        LogEntry e1; // the entry that will be at the top of the redo "stack" and will be the current action.

        e0 = redoStack.Peek(); // gets the first entry
        e1.entity = e0.entity; // gets the first entry again
        // e1.entity = e0.entity; // gets the first entry again (this doesn't mean anything)

        // if the entity has been deleted.
        if (e0.entity == null)
        {
            redoStack.Pop();
            RedoAction(); // calls the function again to deal with the redo operation
            return;
        }

        // steps
        // 1 - get transformation information from object, and save it to e1's PRS variables.
        // 2 - override object transformation information with data in e0's PRS variables
        // 3 - set e0's PRS variables to the PRS variables in e1

        // swaps transform values between game object and attached Transform object.
        // e1 gets the values from the object on e0. This is the same object that's on e1.
        // e1.alive;
        e1.active = e0.entity.active;
        e1.position = e0.entity.transform.position; // copies the object's current position
        e1.rotation = e0.entity.transform.rotation; // copies the object's current rotation
        e1.localScale = e0.entity.transform.localScale; // copies the object's local scale.

        // e0's object is overriden with the values of e0's transform.
        // e0.alive;
        e0.entity.SetActive(e0.entity.active);
        e0.entity.transform.position = e0.position;
        e0.entity.transform.rotation = e0.rotation;
        e0.entity.transform.localScale = e0.localScale;

        // e0's transform gets given the values from e1's transform
        // e0.alive;
        e0.active = e1.active;
        e0.position = e1.position;
        e0.rotation = e1.rotation;
        e0.localScale = e1.localScale;

        redoStack.Pop(); // removes first entry in the undo list.
        undoList.AddFirst(e0); // puts the entry on the redo stack.
        e0.entity.GetComponent<ObjectScript>().ResetPreviousTransform(); // resets the previous transformation to current transform 
    }

    // update is called once per frame
    void Update()
    {
        // CTRL + Z undo shortcut and CTRL + Y redo shortcut
        // if the left or right control keys are pressed.
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            if(Input.GetKeyDown(KeyCode.Z)) // undo
            {
                UndoAction();
            }
            else if(Input.GetKeyDown(KeyCode.Y)) // redo
            {
                RedoAction();
            }
        }

    }
}
