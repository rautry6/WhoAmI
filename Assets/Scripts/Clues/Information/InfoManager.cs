using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;

    public List<InfoUI> CurrentConnections;
    public UICurvedLineController currentLine;
    public List<InfoUI> AllCurrentInfo;

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
