using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フックに関することを扱う
/// </summary>
/// 

public class FookGenerator : MonoBehaviour
{
    [SerializeField] private GameObject FookPrefab;  　   //フックのプレハブ
    [SerializeField] private GameObject VisualFook;  　   //判定用のフック
    [SerializeField] private GameObject voidObj;          //最後のアンカーひっかけ先
    [SerializeField] private SpriteRenderer MoucePointer; //マウス追従のSprite
    [SerializeField] private PlayerParticle pp;

    private HingeJoint2D  hinge; //プレイヤー本体のジョイント
    private Player_Moving pm;

    private GameObject Fook;                 //現在プレイヤーが使っているフックの情報を格納
    private GameObject ConnectObj;           //fookがささる(見かけの)オブジェクト
    private GameObject AnchorObj;            //ひっかけるとこのオブジェクト
    private int        LastFookNumber;       //フックの最後の場所
    private Vector2    AnchorObj_SpownPoint; //ひっかける場所

    private float fooklinelength;
    [SerializeField] Sprite FookLineSprite;
    [SerializeField] LayerMask StageLayer;
    [SerializeField] LayerMask BlockLayer;

    private int fookMaxlength
    {
        get { return FookPrefab.gameObject.transform.childCount; }　//プレハブの子オブジェクトの数を返却
    }
    public Transform AnchorTransform
    {
        get { return AnchorObj.transform; }
    }
    public Transform FirstFookTransform
    {
        get { return Fook.transform.GetChild(0).transform; }
    }
    public GameObject visualfook
    {
        get { return VisualFook; }
    }



    private void Start()
    {

        Fook       = null;
        ConnectObj = null;
        AnchorObj  = null;

        pm    = GetComponent<Player_Moving>();
        hinge = GetComponent<HingeJoint2D>(); //プレイヤー自身のジョイント
        hinge.enabled = false;
        fooklinelength = FookLineSprite.bounds.size.x * 0.1590202f;
    }

    private void Update()
    {
        if (!GameManager.instance.playerinfo.CanControll)
            return;

        Cursor.visible = true;
        VisualFook.gameObject.transform.rotation = Quaternion.Euler(0, 0, GetZangle()); //Z角度調整

        //if (CheckCanShootFook3())
        //{
        //    MoucePointer.color = new Color(0, 1, 0, 1);
        //}
        //else
        //{
        //    MoucePointer.color = new Color(1, 0, 0, 1);
        //}
    }

    public void CreateFook()
    {
        if (!CheckCanShootFook3())
        {
            return;
        }
        if (pm.PlayerState == Player_Moving.State.FOOKING)
        {
            return;
        }
        
        float Zangle = GetZangle();

        //フックを射出    
        Fook = Instantiate(FookPrefab, transform.position + new Vector3(0,0,10f), Quaternion.Euler(0, 0, Zangle));
        Fook.SetActive(true);

        Transform LastFookTransform = Fook.transform.GetChild(LastFookNumber).transform;
        AnchorObj_SpownPoint        = LastFookTransform.position + LastFookTransform.right * 0.3f ;


        //最後のフックの角度調整
      //  Debug.Log("フック長:" + LastFookNumber);
        var limit = new JointAngleLimits2D();
        limit.min = -180;
        limit.max = 180;
        Fook.transform.GetChild(LastFookNumber).gameObject.GetComponent<HingeJoint2D>().limits = limit;


        //フック重さ調整
        float maxweight = 5;
        float supweight = 1f;

        for (int i = 0; i < LastFookNumber + 1; i++)
        {
            Fook.transform.GetChild(i).GetComponent<Rigidbody2D>().mass = supweight + (maxweight - supweight) * i / (LastFookNumber);
                  
        }

        //余分フックを削除
        for (int i = LastFookNumber + 1; i < fookMaxlength; i++)
        {
         //   Destroy(Fook.transform.GetChild(i).gameObject);
            Fook.transform.GetChild(i).gameObject.SetActive(false);
        }

        //フックの1本目をプレイヤーのhingeのconnectに入れる
        GameObject firstfook = Fook.transform.GetChild(0).gameObject;
        hinge.connectedBody = firstfook.GetComponent<Rigidbody2D>();
        hinge.enabled = true;

        //クリックした場所にアンカーひっかけ先を生成。ひっかけ先を見かけ上オブジェクトの子に
        AnchorObj = Instantiate(voidObj, AnchorObj_SpownPoint, Quaternion.Euler(0, 0, Zangle));
        AnchorObj.transform.parent = ConnectObj.transform;

        //フックの最後に、アンカーオブジェクトのcolliderをいれる
        GameObject lastfook = Fook.transform.GetChild(LastFookNumber).gameObject;
        HingeJoint2D lastfook_joint = lastfook.GetComponent<HingeJoint2D>();
        lastfook_joint.connectedBody = AnchorObj.GetComponent<Rigidbody2D>();

        //後処理など
        pm.PlayerState = Player_Moving.State.FOOKING;

        if(ConnectObj.tag == "Goal")
        {
            GameManager.instance.GameClear();
        }
    }

    public void BreakFook()
    {
        if (Fook == null)
            return;

        Destroy(AnchorObj);
        Destroy(Fook);
        hinge.enabled = false;
        pm.PlayerState = Player_Moving.State.ON_AIR;
        pp.OnParticle();

        if (Vector3.Magnitude(pm.gameObject.GetComponent<Rigidbody2D>().velocity) < 17)
        {
            AudioManager.instance.PlaySE(0);
        }
        else
        {
            AudioManager.instance.PlaySE(1);
        }
    }

    /// <summary>
    /// クリックの場所がStageレイヤーを指していればtrue、そうでなければfalseを返します
    /// </summary>
    /// <returns></returns>
    private bool IsHitStage()
    {
        Vector3 moucepos = Input.mousePosition;
        moucepos.z = 0f;
        MoucePointer.gameObject.transform.position = Camera.main.ScreenToWorldPoint(moucepos + new Vector3(0,0,5)); //ポインタ位置更新
        Ray ray = Camera.main.ScreenPointToRay(moucepos);

        RaycastHit2D ray2d;
        ray2d = Physics2D.Raycast(ray.origin, ray.direction);
        if (!ray2d.collider)
        {
            return false;
        }

        if (ray2d.collider.gameObject.layer == 8)
        {
            ConnectObj = ray2d.collider.gameObject;
          //  Debug.Log(ConnectObj);
            return true;
        }

        return false;
    }


    //アンカー射出後の当たり判定をチェックする
    private int CheckOverlap()
    {
        if (Fook == null)
        {
            return -1;
        }

        var result = new List<Collider2D>();
        for (int i = 0; i < fookMaxlength; i++)
        {
            GameObject smallfook = Fook.transform.GetChild(i).gameObject;
            int test = smallfook.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), result);

            if(result.Exists( col => col.gameObject.layer == 9 )) //フックがかけれないレイヤーが含まれていればアウト
            {
                return -1;
            }

            if (result.Exists(col => col.gameObject.layer == 8 && col.gameObject == ConnectObj))
            {
                if (i == fookMaxlength - 1)
                {
                    AnchorObj_SpownPoint = smallfook.transform.position;
                }
                else //最大長じゃなければすこし多めにアンカーをかける場所をとる
                {
                    GameObject end_nextfook = Fook.transform.GetChild(i + 1).gameObject;
                    AnchorObj_SpownPoint = end_nextfook.transform.position;
                }
                return i;
            }
          
        }
        return -1;
    }

    /// <summary>
    /// マウスとプレイヤーキャラとのZ角を-180 ~ 180で返却
    /// /// </summary>
    /// <returns></returns>
    private float GetZangle()
    {
        Vector3 moucepos = Input.mousePosition;
        moucepos.z = 0f;
        Vector3 worldpos = Camera.main.ScreenToWorldPoint(moucepos);
        Vector3 Endvec = worldpos - transform.position;

        float Zangle;

        //Z角度を調整する
        if (transform.right.y > Endvec.y)
        {
            Zangle = -Vector2.Angle(transform.right, Endvec);
        }
        else
        {
            Zangle = Vector2.Angle(transform.right, Endvec);
        }
        return Zangle;
    }

    private bool CheckCanShootFook()  //フックが射出可ならtrue、そうでなければfalse。色判定用に、フック中かどうかの判断は含んでいない
    {
        if (!IsHitStage())
        {
            return false;
        }

        var result = new List<Collider2D>();
        for (int i = 0; i < fookMaxlength; i++)
        {
            GameObject smallfook = VisualFook.transform.GetChild(i).gameObject;
            Debug.Log(smallfook.name);
            smallfook.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), result);
            foreach (Collider2D res in result)
            {
                Debug.Log(i + "番目のHitオブジェクト" + res.gameObject.name);
            }

            if (result.Exists(col => col.gameObject.layer == 9)) //フックがかけれないレイヤーが含まれていればアウト
            {
                return false;
            }

            if (result.Exists(col => col.gameObject.layer == 8 && col.gameObject == ConnectObj))
            {
                LastFookNumber = i;
                return true;
            }
        }
        return false;
    }
    private bool CheckCanShootFook3()  //フックが射出可ならtrue、そうでなければfalse。色判定用に、フック中かどうかの判断は含んでいない
    {
        if (!IsHitStage())
        {
            return false;
        }
        
        Vector3 moucepos = Input.mousePosition;
        moucepos.z = 0f;
        Vector3 worldpos = Camera.main.ScreenToWorldPoint(moucepos);
        GameObject fooklne = VisualFook.transform.GetChild(0).gameObject;


        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, worldpos, fooklinelength*25f, StageLayer | BlockLayer);
        float distance = Vector2.Distance(transform.position, hitinfo.point);
        Debug.Log("距離 : " +  distance);
        Debug.Log("単体フック長 : " + fooklinelength);
        Debug.Log(hitinfo.collider.gameObject.name);
        Debug.DrawLine(transform.position, worldpos , Color.red , 3f);

        if(hitinfo.collider.gameObject.layer == 9)
        {
            return false;
        }
        if (hitinfo.collider.gameObject.layer == 8)
        {
            LastFookNumber = (int)(distance / fooklinelength) + 1;
            return true;
        }
        return false;

    }


    private bool CheckCanShootFook2()  //フックが射出可ならtrue、そうでなければfalse。色判定用に、フック中かどうかの判断は含んでいない
    {
        if (!IsHitStage())
        {
            return false;
        }

        var result = new List<Collider2D>();
        var FookColliders = VisualFook.GetComponentsInChildren<BoxCollider2D>();
        List<BoxCollider2D> ResultCollidor = new List<BoxCollider2D>();
        foreach(BoxCollider2D bc in FookColliders)
        {
            bc.OverlapCollider(new ContactFilter2D(), result);
            if(result.Exists(col => col.gameObject.layer == 8 && col.gameObject == ConnectObj))
            {
                ResultCollidor.Add(bc);
            }
        }

        if(ResultCollidor.Count == 0)
        {
            return false;
        }

        float distance = 100;
        var   nearfook = new BoxCollider2D();

        foreach (BoxCollider2D bc in ResultCollidor)
        {
            float magunitude = Vector3.Magnitude(bc.gameObject.transform.position - transform.position);
            if (magunitude < distance)
            {
                distance = magunitude;
                nearfook = bc;
            }
        }

        var result2 = new List<Collider2D>();
        nearfook.OverlapCollider(new ContactFilter2D(), result);
        if (result.Exists(col => col.gameObject.layer == 9))
        {
            return false;
        }

        LastFookNumber = nearfook.gameObject.transform.GetSiblingIndex();
        return true;

    }
    //フックの長さを判定する(長さがステージに届いてなければ終了)
    //    int Lastfooknumber = CheckOverlap();
    //     if (Lastfooknumber == -1)
    ////    {
    //        BreakFook();
    //        return;
    //    }

}

