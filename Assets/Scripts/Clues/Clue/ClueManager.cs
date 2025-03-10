using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ClueManager : MonoBehaviour
{
    public static ClueManager instance;

    public Transform ClueUIArea;
    public GameObject ClueUIPrefab;

    private ClueGetterHelper ClueGetter;

    private List< Clue> mClueList = new List<Clue>();

    void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClueGetter = GetComponent<ClueGetterHelper>();
    }

    public void AddClue(EInfoTypes ClueType)
    {
        // get new clue from list of clues
        Clue newClue = ClueGetter.GetClue(ClueType);

        if(newClue == null)
        {
            Debug.Log("Error: No such clue exists");
            return;
        }

        // add the clue to the list
        mClueList.Add(newClue);

        // make new UI for clue
        ClueUI clueUI = Instantiate(ClueUIPrefab, ClueUIArea).GetComponent<ClueUI>();
        clueUI.AssignClue(newClue);

    }

    public bool PlayerHasClue(EInfoTypes ClueType)
    {
        bool found = false;

        foreach(Clue clue in mClueList)
        {
            if(clue.ClueType == ClueType)
            {
                found = true;
                break;
            }
        }

        return found;
    }
}
