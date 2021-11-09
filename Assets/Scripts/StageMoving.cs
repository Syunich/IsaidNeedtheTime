using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 動くステージにつけるスクリプト。縦方向は上を正に移動、横方向は右を正に移動する
/// </summary>
public class StageMoving : MonoBehaviour
{
    [Header("縦方向移動...Vertical , 横方向移動...Horizontal")]
    [SerializeField] Direction direction;

    [Header("移動速度")]
    [Range(0 , 10000)]
    [SerializeField] float speed;

    [Header("移動幅")]
    [Range(0, 10000)]
    [SerializeField] float movelength;

    [Header("端点での停止時間(秒)")]
    [Range(0, 10000)]
    [SerializeField] private float stoptime;

    [Header("正方向に進むかどうかを初めに設定。falseなら正方向と逆へ")]
    [SerializeField] bool IsGoingPositive;

    protected Rigidbody2D rigid;
    protected float passedtime; //停止中にすぎた時間
    protected float moveamout;  //移動した距離
    protected bool  Canmove;    //移動できるか?
    protected bool is_appear_to_base; //原点方向に進んでいるか？
    protected Vector2 StartPosition; //誤差消し用のオブジェクト初期座標

    public void Start()
    {
        passedtime    = 0;
        rigid         = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
        Canmove       = true;
        is_appear_to_base = false;


        if (rigid == null)
        {
            Debug.LogError("オブジェクト" + gameObject.name + "にrigidbodyがありません");
        }
        if(rigid.bodyType == RigidbodyType2D.Static)
        {
            Debug.LogError("オブジェクト" + gameObject.name + "は静的です");
        }
        if(!IsGoingPositive)
        {
            speed = -speed;
        }
    }

    private void FixedUpdate()
    {
        if(Canmove)
        {
            Moving(); //動けるなら動く
            CheckState(); //移動量確認
        }
        else
        {
            Action_on_Stopping();  //停止中の処理(時間計測など)
        }
    }

    private void Moving()
    {
        if (direction == Direction.Horizontal)
        {
            rigid.transform.position += new Vector3(speed, 0f);
        }
        else
        {
            rigid.transform.position += new Vector3(0f , speed);
        }
        moveamout += Mathf.Abs(speed);
    }

    protected void LeaveEdge()
    {
        speed = -speed;
        Canmove = true;
        passedtime = 0;
        moveamout = 0;
        is_appear_to_base = !is_appear_to_base;
    }

    //現在の状態を確認
    protected void CheckState()
    {
        if (moveamout < movelength) { return ; }

        //移動量を超えてた場合は停止
        else
        {
            Canmove = false;
            SetCorrectPosition(); //移動量誤差を0に
        }
        
    }

    //停止中に行う処理
    protected void Action_on_Stopping()
    {
        passedtime += Time.deltaTime;
        if (passedtime < stoptime) { return ; } //時間がすぎてないなら抜ける

        LeaveEdge();
    }

    protected void SetCorrectPosition() //かなり読みにくい
    {
        if(is_appear_to_base)
        {
            transform.position = StartPosition;
        }
        else
        {
            float corecct_amount = movelength;  //原点ではない端点だった場合の座標格納

            if(!IsGoingPositive) //負方向がデフォルトの場合
            {
                corecct_amount = -corecct_amount;
            }

            if(direction == Direction.Horizontal)
            {
                rigid.transform.position = StartPosition + new Vector2(corecct_amount, 0f );
            }
            else
            {
                rigid.transform.position = StartPosition + new Vector2(0f , corecct_amount);
            }
                
        }
    }

}

 public enum Direction
{
    Horizontal ,
    Vertical
}


