using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpScript_new : MonoBehaviour
{
    [Header("ジャンプ力の強さ")]
    [SerializeField] private float jump_power;

    [Header("ジャンプ判定を出すためのTransform")]
    [SerializeField] Transform LeftFootTransform;
    [SerializeField] Transform RightFootTransform;
    [SerializeField] Transform LeftendTransform;
    [SerializeField] Transform RightendTransform;

    [Header("ジャンプ判定の対象レイヤー")]
    [SerializeField] LayerMask StageLayer;
    [SerializeField] LayerMask BlockLayer;

    [Header("ジャンプ不可対象レイヤー")]
    [SerializeField] LayerMask CantJumpLayer;
    private float Jump_delta;
    private Rigidbody2D rb;
    private bool canjump;
    private Player_Moving pm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<Player_Moving>();
        ResetValue();
    }

    private void Update()
    {
      canjump = Check_OnGround();
      Jump_delta += Time.deltaTime;
    }

    /// <summary>
    /// 呼び出し先のスクリプトと同じオブジェクトにつけていれば、この関数を呼び出すとジャンプします。RigidBodyが必要
    ///</summary>
    public void Jump()
    {
        if (!canjump) {  return; }
        if(Jump_delta < 0.1f) { return; }


        //ジャンプ処理
        rb.AddForce(transform.up * jump_power , ForceMode2D.Impulse);
        Jump_delta = 0;
    }
    private void ResetValue()
    {
        Jump_delta = 0;
        canjump = false;
    }
    private bool Check_OnGround()
    {
        if(pm.PlayerState == Player_Moving.State.FOOKING)
        {
            return false;
        }

        Vector2 StartPos1 = LeftFootTransform.position;
        Vector2 StartPos2 = RightFootTransform.position;
        Vector2 EndPos1 = LeftendTransform.position;
        Vector2 EndPos2 = RightendTransform.position;
        if (    Physics2D.Linecast(StartPos1, EndPos1, StageLayer) || Physics2D.Linecast(StartPos2, EndPos2, StageLayer) ||
                Physics2D.Linecast(StartPos1, EndPos1, BlockLayer) || Physics2D.Linecast(StartPos2, EndPos2, BlockLayer)
            )
        {
            pm.PlayerState = Player_Moving.State.ON_GROUND;
            return true;
        }

        if (Physics2D.Linecast(StartPos1, EndPos1, CantJumpLayer))
        {
            pm.PlayerState = Player_Moving.State.ON_CANTJUMP;
            return false;
        }


        pm.PlayerState = Player_Moving.State.ON_AIR;
        return false;

        
    }

}
