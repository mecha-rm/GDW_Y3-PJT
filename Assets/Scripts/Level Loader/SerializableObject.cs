using UnityEngine;

// subclasses of this base class have a function that can be serializable.
// this doesn't support multiple instances of the same component, but that shouldn't happen anyway.
// NOTE: this can only save serializable components, so data for other components should be put in them.
public abstract class SerializableObject : MonoBehaviour
{
    // serialize parent
    public static byte[] SerializeObject(SerializedObject entity)
    {
        byte[] arr = FileStream.SerializeObject(entity);
        return arr;
    }

    // deserialize parent
    public static SerializedObject DeserializeObject(byte[] arr)
    {
        object entity = FileStream.DeserializeObject(arr);
        SerializedObject parent = (SerializedObject)entity;
        return parent;
    }

    // packs the object and returns a serialized object
    // note. This should be called for the gameObject all components are attached to.
    public static SerializedObject Pack(GameObject entity, System.Type type, string prefabPath = "")
    {
        // creates the object
        SerializedObject serializedObject = new SerializedObject(entity.name, type, prefabPath, entity.transform);

        // gets all components of type 'SerializableObject'
        SerializableObject[] comps = entity.GetComponents<SerializableObject>();

        // adds all Serializable Components
        foreach (SerializableObject comp in comps)
        {
            serializedObject.components.Add(comp.ExportSerializedComponent());
        }

        return serializedObject;
    }

    // packs this object with a prefab. Pass in derived type.
    public SerializedObject Pack(System.Type type, string prefabPath = "")
    {
        return Pack(gameObject, type, prefabPath);
    }

    // packs data and returns it as byte array
    public static byte[] PackToBytes(GameObject entity, System.Type type, string prefabPath = "")
    {
        SerializedObject obj = Pack(entity, type, prefabPath);
        return FileStream.SerializeObject(entity);
    }

    // packs data and returns it as byte array
    public byte[] PackToBytes(System.Type type, string prefabPath = "")
    {
        return PackToBytes(gameObject, type, prefabPath);
    }

    // the unpack functions are all static since these are meant to load in game objects
    // unpacks the serialized object
    // there's no local Unpack since a new object would need to be created, then its values would need to be instaniated.
    public static GameObject Unpack(SerializedObject serializedObject)
    {
        // base object and game object
        object baseObject;
        GameObject gameObject;

        // PARENT //
        // there is a prefab - load prefab
        if(serializedObject.prefabPath != "")
        {
            Object newObject = Resources.Load(serializedObject.prefabPath);
            gameObject = Instantiate((GameObject)newObject);
            gameObject.name = serializedObject.name;
        }
        else // there is no prefab - load from type
        {
            if(serializedObject.type != null) // type is set
            {
                baseObject = System.Activator.CreateInstance(serializedObject.type);
                gameObject = Instantiate((GameObject)(baseObject));
                gameObject.name = serializedObject.name;
            }
            else // type is not set - load empty object
            {
                gameObject = new GameObject(); // empty game object
                gameObject.name = serializedObject.name;
            }
        }

        // sets values for transformation
        gameObject.transform.position = serializedObject.position;
        gameObject.transform.rotation = serializedObject.rotation;
        gameObject.transform.localScale = serializedObject.localScale;

        // COMPONENTS //
        // gets and adds components, giving them the overwritten data
        foreach (SerializedComponent comp in serializedObject.components)
        {
            // generates a serialized object from the component.
            // object objectComp = System.Activator.CreateInstance(comp.type);
            Component attachedComp = null; // component

            // if there is no type, move on to the next one.
            if (comp.type == null)
                continue;

            // checks to see if the component is already attached
            // (Name returns class name, .FullName returns class namespace with class name)
            attachedComp = gameObject.GetComponent(comp.type.FullName); 

            // if the attached component is null, it means it hasn't actually been attached at.
            if(attachedComp == null)
               attachedComp = gameObject.AddComponent(comp.type); // attaches component


            // if the component is a serializable object, call its function.
            if (attachedComp is SerializableObject)
            {
                SerializableObject serialObject = (SerializableObject)(attachedComp);
                serialObject.ImportSerializedComponent(comp); // passes content so it can be downcasted.
            }
        }

        // CHILDREN //
        // adds the children
        foreach(SerializedObject child in serializedObject.children)
        {
            GameObject importedChild = SerializableObject.Unpack(child);
            importedChild.transform.parent = gameObject.transform;
        }

        return gameObject;
    }

    // unpacks at the byte level
    public static GameObject UnpackFromBytes(byte[] data)
    {
        return Unpack(DeserializeObject(data));
    }

    // these are treated as if they are static, but they're not.
    // exports the child component with its provided values.
    // the SerializedChild should be inherited.

    // creates a serialized component in the derived class and returns it.
    // the SerializedComponent class should be overwritten so that it can be downcasted when imported.
    // this is called by the Pack() function.
    public abstract SerializedComponent ExportSerializedComponent();
    
    // imports the component and calls a function so that it can be downcasted and have its content accessible.
    // the SerializedComponent class should be overwritten so that it has something to downcast to.
    // this is called by the Unpack() function.
    public abstract void ImportSerializedComponent(SerializedComponent component);


}
