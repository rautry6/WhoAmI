using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public Sprite ItemSprite;
    public string ItemDescription;
}
