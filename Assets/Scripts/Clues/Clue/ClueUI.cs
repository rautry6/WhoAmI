using TMPro;
using UnityEngine;

public class ClueUI : MonoBehaviour
{
    public TMP_Text text;
    public Clue CurrentClue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignClue(Clue clue)
    {
        CurrentClue = clue;
        text.text = clue.ClueName;
    }
}
