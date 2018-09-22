using UnityEngine;
class Util 
{
    public static void RemoveAllChildren(Transform trans)
    {
        foreach(Transform child in trans) {
            GameObject.Destroy(child);
        }
    }
}