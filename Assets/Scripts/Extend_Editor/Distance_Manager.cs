using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 点と距離に関するクラス
/// </summary>
public static class Distance_Manager
{
    private static WhereP wherep;
    /// <summary>
    /// 二点start,endを通る直線の式をax+by+c = 0で返します
    /// </summary>
    /// <param name="start">点1</param>
    /// <param name="end">点2</param>
    /// <returns></returns>
    private static Coef GetStraightLine(Vector2 start , Vector2 end)
    {
        if (start.x == end.x)
        {

            return new Coef(1, 0, -start.x);
        }
        else
        {
            float slope = (end.y - start.y) / (end.x - start.x);
            float seppen = (start.y - slope * start.x);
            return new Coef(slope , -1 , seppen);
        }
    }

    /// <summary>
    /// 直線Coefと点ForcusPointの距離を返します
    /// </summary>
    /// <returns></returns>
    public static float GetDistance_StraightLine(Vector2 point , Vector2 start, Vector2 end)
    {
        Coef Line = GetStraightLine(start, end);
        return Mathf.Abs(Line.a * point.x + Line.b * point.y + Line.c) / Mathf.Sqrt(Line.a * Line.a + Line.b * Line.b);
    }

    /// <summary>
    /// 1つの点pointに対して、線分(start,end)からの距離を返します。
    /// </summary>
    /// <param name="point">参照点</param>
    /// <param name="start">線分始発店</param>
    /// <param name="end">線分終着点</param>
    /// <returns></returns>
    public static float GetDistance_Segment(Vector2 point, Vector2 start, Vector2 end)
    {
        
        float distance_start = Vector2.Distance(point, start);
        float distance_end   = Vector2.Distance(point, end);
        float smalldistance;
        if (distance_start > distance_end)
        {
            wherep = WhereP.END;
            smalldistance = distance_end;
        }
        else
        {
            wherep = WhereP.START;
            smalldistance = distance_start;
        }

        float distance_vertical = GetDistance_StraightLine(point ,start , end);
        //線分の長さ
        float linelength = Vector2.Distance(start, end);
        //内積の射影
        float CosShita   = Vector2.Dot(point - start, end - start) / (distance_start * linelength);
        float syaei      = Vector2.Distance(point, start) * CosShita;


        //法線範囲にない場合
        if (syaei < 0 || syaei > linelength)
        {
            return smalldistance;
        }

        //法線範囲にある場合
        wherep = WhereP.SEGMENT;
        return distance_vertical;

    }

    /// <summary>
    /// 点からおろした垂線の足がどの位置にいるかを0~1で返却
    /// </summary>
    /// <param name="point"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static float GetPerCentage(Vector2 point, Vector2 start, Vector2 end)
    {
        float a = GetDistance_Segment(point, start, end);
        if(wherep == WhereP.START)
        {
            return 0;
        }
        if(wherep == WhereP.END)
        {
            return 1;
        }

        Vector2 MiddlePoint; //線分上の点を格納
        //線分上の点を求める
        if(start.x == end.x)
        {
            MiddlePoint.x = start.x;
            MiddlePoint.y = point.y;
        }
        else if(start.y == end.y)
        {
            MiddlePoint.x = point.x;
            MiddlePoint.y = start.y;
        }
        else
        {
            Coef linecoef = GetStraightLine(start, end);
            Coef segmentcoef = new Coef(-1 / linecoef.a, 1, (point.x / linecoef.a) - point.y);

            MiddlePoint.x = (segmentcoef.c - linecoef.c) / (linecoef.a - segmentcoef.a);
            MiddlePoint.y = -segmentcoef.a * MiddlePoint.x - segmentcoef.c;
        }

        float MinDistance = 10000; //参照点ベクトルと刻みベクトルの大きさの差を保存
        float result = 0;
        float nowdistance = 0;
        Vector2 deltavector = new Vector2(0, 0);

        //tは刻み幅
        for (float t = 1f / 50f; t < 1f; t += 1f / 50f)
        {
            deltavector = (1 - t) * start + t * end;
            nowdistance = Vector2.Distance(deltavector, MiddlePoint);
            if (MinDistance > nowdistance)
            {
                MinDistance = nowdistance;
                result = t;
            }
        }
        return result;
    }
    /// <summary>
    /// 直線の係数ax + by + c = 0
    /// </summary>
    private struct Coef
    {
        public float a { get; }
        public float b { get; }
        public float c { get; }

        public Coef(float a = 0 , float b = 0 , float c = 0)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }

    private enum WhereP
    {
    START ,
    END ,
    SEGMENT
    }

}
