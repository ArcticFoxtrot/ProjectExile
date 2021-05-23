using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T[] RemoveFromArray<T> (this T[] originalArray, T itemToRemove){
        int numIndex = System.Array.IndexOf(originalArray, itemToRemove);
        if(numIndex == -1) return originalArray;
        List<T> temp = new List<T>(originalArray);
        temp.RemoveAt(numIndex);
        return temp.ToArray();
    }

    public static T[] ClearArray<T>(this T[] originalArray){
        
        List<T> temp = new List<T>(originalArray);
        temp.Clear();
        originalArray = temp.ToArray();
        return originalArray;
    }

}
