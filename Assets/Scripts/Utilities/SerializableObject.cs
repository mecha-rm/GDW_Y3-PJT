using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// these are classes so that they can be inherited.
// a parent serialized object
[System.Serializable]
public class SerializedObject
{
    public string type; // the file path to the entity, or the name of the type.
    public string name; // the name of the entity;
    public List<SerializedComponent> components;

    // transformation information
    public Vector3 position = new Vector3();
    public Quaternion rotation = new Quaternion(0, 0, 0, 1);
    public Vector3 scale = new Vector3(1, 1, 1);
}

// a child of the serialized parent
// this should be inherited and given the appropriate values.
[System.Serializable]
public class SerializedComponent
{
    public string componentName = "";
}

// subclasses of this base class have a function that can be serializable.
public abstract class SerializableObject : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }
    // 
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    // serialize parent
    public static byte[] SerializeParent(SerializedObject entity)
    {
        byte[] arr = FileStream.SerializeObject(entity);
        return arr;
    }

    // deserialize parent
    public static SerializedObject DeserializeParent(byte[] arr)
    {
        object entity = FileStream.DeserializeObject(arr);
        SerializedObject parent = (SerializedObject)entity;
        return parent;
    }

    // packs the object and returns a serialized parent
    public static SerializedObject Pack(GameObject entity)
    {
        SerializedObject serializedObject = new SerializedObject();
        serializedObject.type = entity.GetType().Name; // maybe use full name instead?
        serializedObject.name = entity.name;

        // copies transform 
        serializedObject.position = entity.transform.position;
        serializedObject.rotation = entity.transform.rotation;
        serializedObject.scale = entity.transform.localScale;

        // gets all components
        SerializableObject[] comps = entity.GetComponents<SerializableObject>();
        
        // adds all components
        foreach(SerializableObject comp in comps)
        {
            serializedObject.components.Add(comp.ExportSerializedComponent());
        }

        return serializedObject;
    }

    // packs this object
    public SerializedObject Pack()
    {
        return Pack(gameObject);
    }

    // packs data and returns it as byte array
    public static byte[] PackToBytes(GameObject entity)
    {
        SerializedObject obj = Pack(entity);
        return FileStream.SerializeObject(entity);
    }

    // packs the data and returns it as bytes
    public byte[] PackToBytes()
    {
        SerializedObject obj = Pack(gameObject);
        return FileStream.SerializeObject(obj);
    }

    // unpacks the serialized parent and returns a game object.
    public static GameObject Unpack(SerializedObject parent)
    {
        GameObject newObject;
        
        // if the entity contains a period, it's a file to be loaded.
        // if it doesn't, it's a Unity object. If it's blank, it's an empty game object.
        if(parent.type.Contains(".") && parent.type != "") // unity primitive type
        {
            newObject = (GameObject)System.Activator.CreateInstance(System.Type.GetType(parent.type));
        }
        else if(!parent.type.Contains(".") && parent.type != "") // imported asset or prefab
        {
            // loads the resource
            string path = parent.type;
            int periodIndex = parent.type.IndexOf(".");
            path = path.Substring(0, periodIndex);

            newObject = Instantiate((GameObject)Resources.Load(path));

            // if the new object is null
            if (newObject == null)
                newObject = new GameObject();
        }
        else
        {
            newObject = new GameObject();
        }

        // transformation
        newObject.transform.position = parent.position;
        newObject.transform.rotation = parent.rotation;
        newObject.transform.localScale = parent.scale;

        // gets all components
        foreach(SerializedComponent comp in parent.components)
        {
            SerializableObject newComp = (SerializableObject)System.Activator.CreateInstance(System.Type.GetType(comp.componentName));

            // new component made
            if(newComp != null)
            {
                // adds the component to the object using newComp.
                newComp.ImportSerializedComponent(newObject, comp);
                Destroy(newComp); // deletes newComp.
            }
        }

        return newObject;
    }

    // unpacks at the byte level
    public static GameObject Unpack(byte[] data)
    {
        return Unpack(DeserializeParent(data));
    }

    // these are treated as if they are static, but they're not.
    // exports the child component with its provided values.
    // the SerializedChild should be inherited.
    public abstract SerializedComponent ExportSerializedComponent();
    
    // imports the component and calls the abstract function so that it gets added by the base.
    // this applies a child component and gives it the values.
    public abstract void ImportSerializedComponent(GameObject gameObject, SerializedComponent component);


}
