using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 縦を0度として振り子のうごきをする
/// </summary>
public class Huriko_Rotate : MonoBehaviour
{

    [Header("初めに左へ回転するか指定")]
    [SerializeField] bool IsGoLeft;

    [Header("回転角")]
    [SerializeField] float angle;

    [Header("回転時間")]
    [SerializeField] float rotatesec;

    protected Rigidbody2D rigid;
    private float RotateAngle
        {
        get { return (IsGoLeft) ? -angle : angle; }
        }
    public void Start()
    {

        Sequence seq = DOTween.Sequence();
        Sequence seq2 = DOTween.Sequence();

        seq2.Append(transform.DORotate(new Vector3(0, 0, -RotateAngle), rotatesec))
            .SetLoops(-1 , LoopType.Yoyo).SetEase(Ease.InOutSine);

        seq.Append(transform.DORotate(new Vector3(0, 0, RotateAngle), rotatesec / 2)).SetEase(Ease.InOutSine)
            .OnComplete(() => seq2.Play())
            .Play();
      
    }

   

}
