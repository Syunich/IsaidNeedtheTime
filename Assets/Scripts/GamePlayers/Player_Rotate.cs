using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_Rotate : MonoBehaviour
{
    [SerializeField] Transform RotateCore; //回転するオブジェクトのトランスフォーム。上向きが0度である必要がある
    private FookGenerator fj;
    private Player_Moving pm;
    void Start()
    {
        fj = GetComponent<FookGenerator>();
        pm = GetComponent<Player_Moving>();
    }

    private void Update()
    {
        if (pm.PlayerState != Player_Moving.State.FOOKING)
            return;

        float Zangle = GetFirstFookAngle();

        if (IsPlayerRight())
        {
            RotateCore.rotation = Quaternion.Euler(0, 0, Zangle);
        }
        else
        { 
            RotateCore.rotation = Quaternion.Euler(0, 0, -Zangle);
        }
    }
    
    /// <summary>
    /// プレイヤーとアンカー根本の角度を入手
    /// </summary>
    /// <returns></returns>
    private float GetFookAngle()
    {
        Vector2 Anchor_to_below  = -fj.AnchorTransform.up;
        Vector2 Anchor_to_Player = transform.position - fj.AnchorTransform.position;
        return Vector2.Angle(Anchor_to_Player, Anchor_to_below);
    }


    /// <summary>
    /// プレイヤーとフック一本目の角度を入手
    /// </summary>
    /// <returns></returns>
    private float GetFirstFookAngle()
    {
        Vector2 player_to_up = Vector2.up;
        Vector2 player_to_firstfook = (fj.FirstFookTransform.position + fj.FirstFookTransform.right * 100) - transform.position;
        return Vector2.Angle(player_to_up , player_to_firstfook);
    }

    /// <summary>
    /// プレイヤーの位置がアンカー位置の右側ならtrue、そうでなければfalse
    /// /// </summary>
    /// <returns></returns>
    private bool IsPlayerRight()
    {
        return (transform.position.x >= (fj.FirstFookTransform.position + fj.FirstFookTransform.right * 0.5f).x) ? true : false;
    }

    public void SetRotationZero()
    {
        RotateCore.DORotate(new Vector3(0, 0, 0), 0.5f).Play();
    }

}
