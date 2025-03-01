using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    public List<InfoUI> CurrentConnections;
    public UICurvedLineController currentLine;
    public List<InfoUI> AllCurrentInfo;

    private int numInfoForClue = 3; // how many infos the player need to make a clue

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeConnection(InfoUI connectedUI)
    {
        CurrentConnections.Add(connectedUI);
        currentLine.connectionMade = true;
    }

    public void RemoveConnection(InfoUI connectedUI)
    {
        CurrentConnections.Remove(connectedUI);
    }

    public void MakeClue()
    {
        // make a new clue
    }

    private void CheckIfClueCanBeMade()
    {
        int numSameInfoType = 0;
        bool canMakeConnection = false;
        EInfoTypes lastInfoType = EInfoTypes.Favorite_Color; // default assign

        for(int i = 0; i < CurrentConnections.Count; i++)
        {
            if(i > 0)
            {
                // get current type of info
                EInfoTypes currentInfoType = CurrentConnections[i].info.InfoType;

                // see if its same as previous
                if(currentInfoType != lastInfoType)
                {
                    // break early if not same
                    canMakeConnection = false;
                    break;
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



        if(canMakeConnection)
        {
            // create new clue

            // remove used info or make them uninteractable


        }
        else
        {
            // some sort of ui indication
        }
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
