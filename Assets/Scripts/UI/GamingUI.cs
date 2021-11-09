using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// UI表示に関するクラス(利用される側)
/// </summary>
public class GamingUI : MonoBehaviour
{
    [SerializeField] Text ProgressText;
    [SerializeField] Text TimeText;
    [SerializeField] Text TalkText;

    private static GamingUI Instance;
    public static GamingUI instance
    {
        get { return Instance; }
    }
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// 進捗テキストの書き換え
    /// </summary>
    /// <param name="value"></param>
    public void ChangeProgressText(float value)
    {
        ProgressText.text = "Progress : " + value.ToString("f2") + "%";
    }

    /// <summary>
    /// 秒を引数にとり、テキストを"Time : 00:00:00"に変換
    /// </summary>
    public void ChangeTimeText(float sec)
    {
        int hour = (int)sec / 3600;
        int min  = (int)(sec % 3600) / 60;
        int byou = (int)(sec % 60);
        TimeText.text = "Time : " + hour.ToString("D2") + ":" + min.ToString("D2") + ":" + byou.ToString("D2");
    }

    /// <summary>
    /// 文字を1つずつ表示する
    /// </summary>
    /// <param name="time">1文字の表示時間</param>
    /// <param name="text">表示文字</param>
    /// <param name="Callback">テキスト表示終了時の動作</param>
    /// <returns></returns>
    public  IEnumerator IndicateText(float time , string text , Action Callback)
    {
        TalkText.text = null;
        for(int i = 0; i < text.Length; i++)
        {
            TalkText.text = text[i].ToString();
            yield return new WaitForSeconds(time);
        }
        Callback();
    }

    /// <summary>
    /// 文字を1つずつ表示する(音つき)
    /// </summary>
    /// <param name="time"></param>
    /// <param name="text"></param>
    /// <param name="SEindex">効果音の番号</param>
    /// <param name="Callback"></param>
    /// <returns></returns>
    /// 
    public IEnumerator IndicateText(float time, string text, int SEindex , Action Callback)
    {
        TalkText.text = null;
        TalkText.GetComponent<CanvasGroup>().alpha = 1;
        AudioManager.instance.PlaySE(SEindex);
        for (int i = 0　; i < text.Length; i++)
        {
            TalkText.text += text[i].ToString();
            yield return new WaitForSeconds(time);
        }
        yield return new WaitForSeconds(2.0f);
        TalkText.GetComponent<CanvasGroup>().DOFade(0, 1.0f)
        .OnComplete(() => Callback()).Play();
    }
}
