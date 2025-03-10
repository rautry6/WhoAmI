using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ClueGetterHelper : MonoBehaviour
{
    public List<Clue> PossibleClues;

    /// <summary>
    /// Returns a clue from the list of possible clues based on the type passed. 
    /// Returns null if no type of clue exists
    /// </summary>
    /// <param name="eInfo"></param>
    /// <returns></returns>
    public Clue GetClue(EInfoTypes eInfo)
    {
        Clue clue = null;

        // check for the requested clue in the list
        foreach (Clue cl in PossibleClues)
        {
            if(cl.ClueType == eInfo)
            {
                clue = cl;
                break;
            }
        }

        return clue;
    }
}
