using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
		 * So here's what's going to be done:
		 * (1) You get the type of the parent, its name, its prefab (if applicable) and save that as System.Type.
		 * (2) The parent class is inherited by children that store any other base values they need, which also get saved.
		 * (3) You get all components, and save every component that can be upcasted to SerializableObject. You use ExportSerializeComponent for this.
			* Anything that can't be upcasted would just be set in one of the classes that CAN be upcasted. 
		 * (4) You serialize the object and its data. You put it in a file.
		 * (5) You take the data out of the file and unpack it back to a SerializedObject.
		 * (6) You use the parent's type (System.Type) to create the base object, or you load up the prefab if that's provided.
			* You also give it its name.	
		 * (7) You go through each component, creating the type (or prefab) it applies to. You add each component at this stage.
		 * (8) If a component can be upcasted to SerializableObject, you do that.
		 * (9) You call the ImportSerializedComponent function, giving it the component and the SerializedComponent class.
		 * (10) When downcasted, the SerializedComponent should have the values that are needed to be filled. Said values are then filled.
*/


// CLASSES
// subclasses of this base class have a function that can be serializable.
// this doesn't support multiple instances of the same component, but that shouldn't happen anyway.
// NOTE: this can only save serializable components, so data for other components should be put in them.
public abstract class SerializableObject : MonoBehaviour
{
    // the prefab for the object from the resources folder. Do NOT include the file extension.
    // e.g. a path of "Assets/Resources/Prefabs/Player.prefab" would be entered as "Prefabs/Player"
    public string prefabPath = "";

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
    // if 'prefab path' is left blank, an empty game object is made. 
    public static SerializedObject Pack(GameObject entity, string prefabPath = "")
    {
        // creates the object
        // SerializedObject serializedObject = new SerializedObject(entity.name, type, prefabPath, entity.transform);
        SerializedObject serializedObject = new SerializedObject(entity.name, prefabPath, entity.transform);

        // gets all components of type 'SerializableObject'
        SerializableObject[] comps = entity.GetComponents<SerializableObject>();

        // adds all Serializable Components
        foreach (SerializableObject comp in comps)
        {
            serializedObject.components.Add(comp.ExportSerializedComponent(serializedObject));
        }

        return serializedObject;
    }

    // packs this object with a prefab. Pass in derived type.
    public SerializedObject Pack()
    {
        return Pack(gameObject, prefabPath);
    }

    // for some reason Unity doesn't like these functions
    // packs data and returns it as byte array
    // public static byte[] PackToBytes(GameObject entity, string prefabPath = "")
    // {
    //     SerializedObject obj = Pack(entity, prefabPath);
    //     byte[] data = FileStream.SerializeObject(entity);
    //     return data;
    // }
    // 
    // // packs data and returns it as byte array
    // public byte[] PackToBytes(string prefabPath = "")
    // {
    //     return PackToBytes(gameObject, prefabPath);
    // }

    // the unpack functions are all static since these are meant to load in game objects
    // unpacks the serialized object
    // there's no local Unpack since a new object would need to be created, then its values would need to be instaniated.
    public static GameObject Unpack(SerializedObject serializedObject)
    {
        // if the serialized object is empty, return null.
        if (serializedObject == null)
            return null;

        // game object
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
            gameObject = new GameObject(serializedObject.name);

            // if(serializedObject.type != null) // type is set
            // {
            //     baseObject = System.Activator.CreateInstance(serializedObject.type);
            //     gameObject = Instantiate((GameObject)(baseObject));
            //     gameObject.name = serializedObject.name;
            // }
            // else // type is not set - load empty object
            // {
            //     gameObject = new GameObject(); // empty game object
            //     gameObject.name = serializedObject.name;
            // }
        }

        // sets values for transformation
        gameObject.transform.position = SerializedObject.Vec3ToUnityVector3(serializedObject.position);
        gameObject.transform.rotation = SerializedObject.QuatToQuaternion(serializedObject.rotation);
        gameObject.transform.localScale = SerializedObject.Vec3ToUnityVector3(serializedObject.localScale);

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
                serialObject.ImportSerializedComponent(serializedObject, comp); // passes content so it can be downcasted.
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


    // for some reason Unity doesn't like this function.
    // unpacks at the byte level
    // public static GameObject UnpackFromBytes(byte[] data)
    // {
    //     return Unpack(DeserializeObject(data));
    // }

    // these are treated as if they are static, but they're not.
    // exports the child component with its provided values.
    // the SerializedChild should be inherited.

    // creates a serialized component in the derived class and returns it.
    // the SerializedComponent class should be overwritten so that it can be downcasted when imported.
    // this is called by the Pack() function.
    // the serializable object is provided in case it's needed.
    public abstract SerializedComponent ExportSerializedComponent(SerializedObject serialObject);
    
    // imports the component and calls a function so that it can be downcasted and have its content accessible.
    // the SerializedComponent class should be overwritten so that it has something to downcast to.
    // this is called by the Unpack() function.
    // the serializable object is provided in case it's needed.
    public abstract void ImportSerializedComponent(SerializedObject serialObject, SerializedComponent component);


}
