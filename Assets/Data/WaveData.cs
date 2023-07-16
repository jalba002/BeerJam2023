using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DATA_WaveData_Default", menuName = "BeerJam/Data/WaveData")]
public class WaveData : ScriptableObject
{
    // This will go with instructions
    public List<WaveDataEntry> waveInstructions = new List<WaveDataEntry>();
}

[Serializable]
public class WaveDataEntry
{
    [SerializeField] public Verbs verb;
    [SerializeField] public int amount;
    [SerializeField] public AreaGroup side;
    [SerializeField] public DoorStatus doorStatus;
}

public enum DoorStatus
{
   Open,
   Close
}

public enum Verbs
{
    None,
    Spawn,
    Container,
    ContainerRandom,
    ContainerCloseAll,
    Wait
}
