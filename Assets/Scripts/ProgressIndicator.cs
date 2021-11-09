using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 進捗状況を管理するクラス
/// </summary>
public class ProgressIndicator : MonoBehaviour
{
    //プレイヤー
    [SerializeField] Transform Player;
    [SerializeField] Transform[] Nodes;
    private float passedtime;
    //1区間の長さ
    public float nodelength
    {
        get { return 100f / (Nodes.Length - 1f); }
    }

    /// <summary>
    /// 進捗を0~100で返す
    /// </summary>
    /// <returns></returns>
    public float GetProgress()
    {
        int nearNodeNumber = GetNearNodeNumber();
        return nodelength * (nearNodeNumber +
        Distance_Manager.GetPerCentage(Player.position, Nodes[nearNodeNumber].position, Nodes[nearNodeNumber + 1].position));
    }


    private int GetNearNodeNumber()
    {
        if(Nodes.Length < 2)
        {
            Debug.LogError("ノードが2つありません");
            return 0;
        }

        float leastdistance = 10000;
        float checkdistance = 0;
          int nodenumber = 0;
        for (int i = 0; i < Nodes.Length - 1; i++)
        {
            checkdistance = Distance_Manager.GetDistance_Segment(Player.position, Nodes[i].position, Nodes[i + 1].position);
        //    Debug.Log(i + "ノードの距離:" + checkdistance);
            if (leastdistance >= checkdistance)
            {
                leastdistance = checkdistance;
                nodenumber = i;
            }
        }
        return nodenumber;
    }
}
