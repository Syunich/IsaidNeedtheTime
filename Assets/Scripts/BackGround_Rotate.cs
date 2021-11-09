using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackGround_Rotate : MonoBehaviour
{
    [SerializeField] GameObject BackGround;
    void Start()
    {
        BackGround.transform.DORotate(new Vector3(0, 0, 360),240.0f , RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1).Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
