using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{ 

   public float StartUpTime  { get; set; }
    public float Passedtime  { get; set; }
    public float Progress    { get; set; }
    public bool CanControll  { get; set; }
    public bool IsNowTalking { get; set; }
    public bool IsShowFookGuide { get; set; }
    public PlayerInfo()
    {
        StartUpTime   = 0;
        Passedtime    = 0;
        Progress      = 0;
        IsNowTalking  = false;
        CanControll   = true;
        IsShowFookGuide = true;
    }

}
