/*
 * References:
    - https://docs.unity3d.com/ScriptReference/Transform-position.html
    - https://docs.unity3d.com/ScriptReference/Transform-rotation.html
    - https://docs.unity3d.com/ScriptReference/Transform-localScale.html
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// serialized as an independent object
//[System.Serializable]
//public class SerializedStageObject : SerializedObject
//{
//    public string entityName;
//    public string description;

//    public SerializedStageObject(StageObject so, string etyName, string desc) : 
//        base(so.name, typeof(StageObject), so.transform)
//    {
//        entityName = etyName;
//        description = desc;
//    }
//}

//// serialized as a component
//[System.Serializable]
//public class SerializedStageObjectComponent : SerializedComponent
//{
//    public string entityName;
//    public string description;

//    public SerializedStageObjectComponent(StageObject so, string etyName, string desc) :
//        base(typeof(StageObject))
//    {
//        entityName = etyName;
//        description = desc;
//    }
//}

// rename this class. StageObject is already used.
// things like items and player locations are saved to a particular object.
public class EditorObject : MonoBehaviour
{
    // the user interface manager. Used for selecting objects.
    public GameObject uiManager;

    // name
    public string entityName = "";

    // description
    public string description = "";

    // the main camera for all objects.
    public Camera objectCamera;

    // used for undo/redo - checks to see if anything changes.
    // the previous version of the transformation applied to the object.
    private LogEntry preTransform;

    // start
    void Start()
    {
        // if the name is blank.
        if (entityName == "")
        {
            string str = "";
            const int CHAR_COUNT = 10;

            // uses 10 characters for the name
            for (int x = 0; x < 10; x++)
            {
                // determines whether a number or letter is being used.
                int y = Random.Range(0, 2);

                switch (y)
                {
                    case 0: // add number
                        str += Random.Range(0, 10);
                        break;

                    case 1: // add letter
                    default:
                        str += (char)(Random.Range(65, 123));
                        break;
                }
            }

            entityName = str;
        }

        // leave them seperate
        // transform.name = name;

        // copies the transform infinitely
        preTransform.entity = gameObject;
        preTransform.type = 1;
        preTransform.active = gameObject.active;

        preTransform.position = transform.position;
        preTransform.rotation = transform.rotation;
        preTransform.localScale = transform.localScale;

        // object was created this iteration.
        UndoRedoSystem.RecordAction(preTransform);
        // preTransform.type = 0;

        // BinaryFormatter converter = new BinaryFormatter();
        // MemoryStream mStream = new MemoryStream();
        // 
        // converter.Serialize(mStream, entity);
        // return mStream.ToArray();

        // UndoRedoSystem.RecordObject(this, name);
        // UndoRedoSystem.RegisterCreatedObject(this, name);
    }

    // destructor
    private void OnDestroy()
    {
        // saves new values - no creation or destruction this frame
        // preTransform.entity;
        // preTransform.type = -1;
        // preTransform.active = gameObject.active;
        // preTransform.position = transform.position;
        // preTransform.rotation = transform.rotation;
        // preTransform.localScale = transform.localScale;
        // 
        // // records the action
        // UndoRedoSystem.RecordAction(preTransform);
    }

    // collisions
    private void OnTriggerEnter(Collider col)
    {

    }

    // when the mouse button is down
    private void OnMouseDown()
    {
        // TODO: optimize for specific mouse inputs
        // this doesn't work too well.
        uiManager.GetComponent<UI_Manager>().SetSelectedObject(gameObject); // this is now the selected object

    }

    // when the mouse button is up
    private void OnMouseDrag()
    {
        // TODO: figure out how to drag objects.
        // if(Input.GetKey(KeyCode.Mouse1))

        // if (Input.GetMouseButton(1)) // secondary (right) button
        // if(Input.GetKey(KeyCode.Mouse1))
        //{
        //    UnityEngine.Vector3 mousePos = Input.mousePosition;

        //    if(camera != null)
        //    {
        //        mousePos = camera.ScreenToWorldPoint(mousePos);
        //        mousePos -= camera.transform.position;
        //    }

        //    // mousePos = transform.TransformPoint(mousePos); // does nothing
        //    transform.position = mousePos;

        //}
    }

    // on mouse up
    // private void OnMouseUp()
    // {
    //     
    // }

    //private void OnMouseDown()
    //{
    //    GameObject.Find("Cube").GetComponent<CubeScript>().AddWaypointOnLeftMouseClick(false);

    //    offset = transform.position - (Input.mousePosition - new Vector3(Screen.width / 2.0F, Screen.height / 2.0F, 0.0F)) * 0.1F;
    //}


    //private void OnMouseDrag()
    //{
    //    transform.position = (Input.mousePosition - new Vector3(Screen.width / 2.0F, Screen.height / 2.0F, 0.0F)) * 0.1F + offset;


    //    t1.GetComponent<LineRenderer>().SetPosition(0, transform.position);
    //    t2.GetComponent<LineRenderer>().SetPosition(0, transform.position);


    //}


    //private void OnMouseUp()
    //{
    //    GameObject.Find("Cube").GetComponent<CubeScript>().AddWaypointOnLeftMouseClick(true);
    //}

    // resets the previous transform so that it's set to the current transform.
    public void ResetPreviousTransform()
    {
        preTransform.position = transform.position;
        preTransform.rotation = transform.rotation;
        preTransform.localScale = transform.localScale;
    }

    // update
    void Update()
    {
        // if something has changed.
        if (preTransform.active != gameObject.active ||
            preTransform.position != transform.position || preTransform.rotation != transform.rotation || preTransform.localScale != transform.localScale)
        {
            UndoRedoSystem.RecordAction(preTransform);

            // saves new values
            preTransform.type = 0; // no creation or destruction this frame
            preTransform.active = gameObject.active;
            preTransform.position = transform.position;
            preTransform.rotation = transform.rotation;
            preTransform.localScale = transform.localScale;

            // tform = Instantiate(transform); // makes a copy of the transform
        }
    }

    //// exports the serialized component.
    //public override SerializedComponent ExportSerializedComponent()
    //{
    //    SerializedStageObject comp = new SerializedStageObject();
        
    //    comp.componentName = this.GetType().Name; // gets the name of the type.
    //    comp.entityName = entityName; // gets the entity name
    //    comp.description = description; // gets the description

    //    return comp;
    //}

    //// the imported serialized component.
    //public override void ImportSerializedComponent(GameObject gameObject, SerializedComponent component)
    //{
    //    // downcasts
    //    SerializedStageObject sso = (SerializedStageObject)(component);

    //    // provides values
    //    StageObject attachedComp = gameObject.AddComponent<StageObject>();
    //    attachedComp.entityName = sso.entityName; // gives entity name
    //    attachedComp.description = sso.description; // gives entity description
    //}
}