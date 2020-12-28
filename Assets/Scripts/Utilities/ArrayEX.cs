using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// extra array functions; this is a singleton since the user shouldn't make any objects of this function.
public class ArrayEX<T>
{
    // instance of array EX manager.
    // private static ArrayEX<T> instance = null;

    // constructor
    private ArrayEX()
    {
    }

    // gets instance of class (for bonus)
    // public static ArrayEX<T> GetInstance()
    // {
    //     if (instance == null) // no instance generated.
    //     {
    //         instance = new ArrayEX<T>(); // creates instance
    //     }
    // 
    //     return instance;
    // }

    // adds element to provided array
    public static void AddElement(T[] array, T newElement)
    {
        int len = array.Length;
        Array.Resize(ref array, len + 1);
        array[len] = newElement;
    }

    // insert element into provided array
    public static void InsertElement(T[] array, int index, T newElement)
    {
        // clamps index so that negative indexes equal 0, and out of bounds indexes add to the end.
        index = Mathf.Clamp(index, 0, array.Length - 1);

        // puts value at last index, then moves it back to its desired index
        int oldLen = array.Length;
        Array.Resize(ref array, array.Length + 1);
        array[oldLen] = newElement;

        // moves the element back to its intended place.
        for (int i = array.Length - 2; i >= index && i > 0; i--)
        {
            T temp = array[i + 1]; // gets next value in array
            array[i + 1] = array[i]; // replaces next value in array with current value
            array[i] = temp; // replaces current value with next alue in array
        }
    }

    // removes an element from the array and returns it
    public static T RemoveElement(T[] array, int index)
    {
        // item not found
        if (index < 0 || index >= array.Length)
            return default(T);

        // gets the desired element and switches its place with the element at the end of the array.
        T removedElement = array[index];
        array[index] = array[array.Length - 1];
        array[array.Length - 1] = removedElement;

        // removes last component from the array by resizing it
        Array.Resize<T>(ref array, array.Length - 1);

        // moves swapped item back to its original position
        for (int i = index; i < array.Length - 1; i++)
        {
            T temp = array[i];
            array[i] = array[i + 1];
            array[i + 1] = temp;

        }

        return removedElement;
    }

    // sets an element with a new value
    public static void SetElement(T[] array, int index, T newValue)
    {
        if (index >= 0 && index < array.Length)
            array[index] = newValue;
    }

    // gets an element.Returns default value of type if invalid.
    public static T GetElement(T[] array, int index)
    {
        if (index >= 0 && index < array.Length)
            return array[index];
        else
            return default(T);
    }

    // accessor for getter and setter
    // public T this[int index]
    // {
    //     get { return array[index]; }
    //     set { array[index] = value; }
    // }

    // gets array length
    public static int GetLength(T[] array)
    {
        return array.Length;
    }
    
    // resizes the array.
    public static void Resize(T[] array, int newSize)
    {
        Array.Resize<T>(ref array, newSize);
    }


}
