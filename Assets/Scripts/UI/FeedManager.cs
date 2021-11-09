using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class FeedManager : MonoBehaviour
{
    [SerializeField] CanvasGroup Feeder;
    private static FeedManager Instance;
    public static FeedManager instance
    { get { return Instance; } }
    public static float BGMvolTEMP;

    private void Awake()
    {
        if (instance == null)
        {
            //恐らくここでセーブデータ読み込み
            BGMvolTEMP = 0.4f;
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance違うよ");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void FeedOut(string scenename)
    {
        BGMvolTEMP = AudioManager.instance.BGMsource.volume;
        Instance.Feeder.blocksRaycasts = true;
        Sequence seq = DOTween.Sequence();
        seq.Append    (Instance.Feeder.DOFade(1, 1.5f));
        seq.Join      (AudioManager.instance.BGMsource.DOFade(0, 1.0f));
        seq.OnComplete(() => SceneManager.LoadScene(scenename));
        seq.Play();
    }

    public void Feedin(Action callback)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append    (Instance.Feeder.DOFade(0, 1f));
        seq.Join      (AudioManager.instance.BGMsource.DOFade(BGMvolTEMP, 1.0f));
        seq.OnComplete(() => { callback(); Instance.Feeder.blocksRaycasts = false; });
        seq.Play();
    }
}
