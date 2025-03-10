using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    public List<InfoUI> CurrentConnections;
    public UICurvedLineController currentLine;
    public List<InfoUI> AllCurrentInfo;

    public Button MakeClueButton;

    private int numInfoForClue = 3; // how many infos the player need to make a clue

    private Dictionary<EInfoTypes, int> Connections;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        MakeClueButton.interactable = false; // disable by default
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeConnection(InfoUI connectedUI)
    {
        if(!CurrentConnections.Contains(connectedUI))
        {
             CurrentConnections.Add(connectedUI);
        }

        if(currentLine != null)
        {
            currentLine.connectionMade = true;
        }

        // check if player can make clue now
        MakeClueButton.interactable = (CurrentConnections.Count >= numInfoForClue);
    }

    public void RemoveConnection(InfoUI connectedUI)
    {
        CurrentConnections.Remove(connectedUI);
    }

    public void MakeClue()
    {
        MakeClueButton.interactable = false; // disable button

        // make a new clue
        bool makeClue = CheckIfClueCanBeMade();

        if (makeClue)
        {
            // create new clue
            ClueManager.instance.AddClue(CurrentConnections[0].info.InfoType);

            // remove used info or make them uninteractable
            foreach(var info in CurrentConnections)
            {
                info.DestroyLine();
            }

            foreach (var info in CurrentConnections)
            {
                info.DestroyInfo();
            }

        }
        else
        {
            // some sort of ui indication
            EInfoTypes mostCommonType = CurrentConnections[0].info.InfoType;

            int curHighest;
            Connections.TryGetValue(mostCommonType, out curHighest);

            foreach(var type in Connections)
            {
                if(type.Value > curHighest)
                {
                    curHighest = type.Value;
                    mostCommonType = type.Key;
                }
            }

            foreach(InfoUI infoUI in CurrentConnections)
            {
                if(infoUI.info.InfoType == mostCommonType)
                {
                    StartCoroutine(infoUI.CorrectInfo());
                }
                else
                {
                    StartCoroutine(infoUI.IncorrectInfo());
                }
            }

        }
    }

    private bool CheckIfClueCanBeMade()
    {
        int numSameInfoType = 1;
        bool canMakeConnection = false;
        bool allSameType = true;
        EInfoTypes lastInfoType = EInfoTypes.Favorite_Color; // default assign

        Connections = new Dictionary<EInfoTypes, int>();

        for(int i = 0; i < CurrentConnections.Count; i++)
        {
            if(i > 0)
            {
                // get current type of info
                EInfoTypes currentInfoType = CurrentConnections[i].info.InfoType;

                // get the num of info of that type if there is any already in dictionary
                int value;  
                Connections.TryGetValue(currentInfoType, out value);

                // add to the value
                if (Connections.ContainsKey(currentInfoType))
                {
                    Connections[currentInfoType] = value + 1;
                }
                else
                {
                    Connections.Add(currentInfoType, value + 1);
                }


                // see if its same as previous
                if (currentInfoType != lastInfoType)
                {
                    // not same type
                    allSameType = false;
                    continue;
                }

                // if same assign var and move to next info
                lastInfoType = currentInfoType;
                numSameInfoType++;


                // if above the number needed for info set bool to true
                // don't quit checking incase future info do not match
                if(numSameInfoType >= numInfoForClue)
                {
                    canMakeConnection = true;
                }
            }
            else
            {
                // get type of the first connection
                lastInfoType = CurrentConnections[i].info.InfoType;

                numSameInfoType++;
            }
        }


        if (!allSameType)
        {
            canMakeConnection = false;
        }


        return canMakeConnection;



    }

    public void AddInfo(InfoUI newInfo)
    {
        AllCurrentInfo.Add(newInfo);
    }

    public void RemoveInfo(InfoUI oldInfo)
    {
        AllCurrentInfo.Remove(oldInfo);
    }

    public void MakingConnection()
    {
        foreach(InfoUI info in AllCurrentInfo)
        {
            info.MakingConnection = true;
        }
    }

    public void ConnectionMade()
    {
        foreach (InfoUI info in AllCurrentInfo)
        {
            info.MakingConnection = false;
        }
    }
}
