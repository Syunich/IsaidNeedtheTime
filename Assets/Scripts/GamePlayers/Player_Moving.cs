using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの操作を反映したクラス。ここをオフにすれば操作不可能になる
/// </summary>
public sealed class Player_Moving : MonoBehaviour
{
    [SerializeField] float Force; //上へのリフト力

    private Player_JumpScript_new pj;
    private PlayerWalkScript pw;
    private FookGenerator fg;
    private Player_Rotate pr;
    private bool Isretrying;
    private float countretry;

    public State PlayerState { get; set; }
    void Start()
    {
        PlayerState = State.ON_GROUND;
        pj = GetComponent<Player_JumpScript_new>();
        pw = GetComponent<PlayerWalkScript>();
        fg = GetComponent<FookGenerator>();
        pr = GetComponent<Player_Rotate>();
        Isretrying = false;
        countretry = 0;
        UserManager.Show_or_HideVisualHook(this , true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && !GameManager.instance.IsTutorial)
        {
            countretry += Time.deltaTime;
            if (!Isretrying && countretry > 2)
            {
                Isretrying = true;
                GameManager.instance.ReStart();
            }
        }
        else
        {
            countretry = 0;
        }

        if (!GameManager.instance.playerinfo.CanControll)
        {
            return;
        }

        //スペースでジャンプ
        if(Input.GetKeyDown(KeyCode.Space))
        {
            pj.Jump();
        }

        //Dで右移動
        if (Input.GetKey(KeyCode.D))
        {
            pw.WalkToRight();
        }

        //Aで左移動
        if (Input.GetKey(KeyCode.A))
        {
            pw.WalkToLeft();
        }

        //マウス左クリックでフック射出
        if(Input.GetMouseButtonDown(0))
        {
            fg.CreateFook();
        }

        //マウス右クリックでフック削除
        if (Input.GetMouseButtonDown(1))
        {
            if(PlayerState != State.FOOKING)
            {
                return;
            }

            fg.BreakFook();
            pr.SetRotationZero();
        }
        //タブキーでフックの切り替え
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UserManager.Show_or_HideVisualHook(this);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            UserManager.Show_or_HideOprion();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            FeedManager.instance.FeedOut("TitleScene");
        }
    }

    private void OnTriggerStay2D(Collider2D collision) //パーティクルに触れてるときの移動処理
    {
        if (collision.tag == "Lift")
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * Force, ForceMode2D.Force);

        if (collision.tag == "Lift_Left")
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * Force * 3f, ForceMode2D.Force);

        if (collision.tag == "Lift_Right")
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * Force * 3f, ForceMode2D.Force);

    }
    private void OnTriggerEnter2D(Collider2D collision) //パーティクルに触れてるときの移動処理
    {
        if (collision.tag == "EndTutorial")
        {
            GameManager.instance.playerinfo.CanControll = false;
            FeedManager.instance.FeedOut("TitleScene");
        }

    }

    public enum State
    {
      ON_GROUND, //接地
      FOOKING , //フック中
      ON_AIR, //空中
      ON_CANTJUMP
    }
}
