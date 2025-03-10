using UnityEngine;

[CreateAssetMenu(fileName = "Clue", menuName = "Scriptable Objects/Clue")]
public class Clue : ScriptableObject
{
    public string ClueName; // name for UI
    public EInfoTypes ClueType; // type of clue
}
