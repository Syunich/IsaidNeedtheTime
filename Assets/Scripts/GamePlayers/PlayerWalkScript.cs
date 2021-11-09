using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの横移動を司るクラス
/// </summary>
public class PlayerWalkScript : MonoBehaviour
{
    [Header("プレイヤーの歩くスピード")]
    [SerializeField] float movespeed;
    [Header("追従度")]
    [SerializeField] float moveforce_multiplier;


    private float moveamount_x;
    private Rigidbody2D rb;
    private Player_Moving pm;
    private float moveforce;

    private void Start()
    {
        moveforce = moveforce_multiplier;
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<Player_Moving>();
    }
    void FixedUpdate()
    {
        //横方向に　固定倍率 * (移動量ベクトル)　-  (現在の速度ベクトル)
        //の力を加える。移動が増えると速度も増えるが、移動をやめると
        //逆方向に力がかかるため相対的に素早くとまるようになる。
        //また、動き出しも固定倍率によって素早くすることができる。
        //空中ではこの移動量(戻る力)を一時的にゆるくする

        if(pm.PlayerState != Player_Moving.State.ON_GROUND && pm.PlayerState != Player_Moving.State.ON_CANTJUMP)
        {
            moveforce = moveforce_multiplier / 6;
        }
        else
        {
            moveforce = moveforce_multiplier;
        }

          rb.AddForce(moveforce * (new Vector2(moveamount_x, 0) - new Vector2(rb.velocity.x , 0)));
          moveamount_x = 0;
    }

    /// <summary>
    /// 呼び出し先のスクリプトと同じオブジェクトにつけていれば、この関数を呼び出すと右移動します
    ///</summary>
    internal void WalkToRight()
    {
        moveamount_x = movespeed;
    }

    /// <summary>
    /// 呼び出し先のスクリプトと同じオブジェクトにつけていれば、この関数を呼び出すと左移動します
    ///</summary>
    internal void WalkToLeft()
    {
        moveamount_x = -movespeed;
    }
}
