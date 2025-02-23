using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "Scriptable Objects/Info")]
public class Info : ScriptableObject
{
    public EInfoTypes InfoType;
    public string Name;
}
