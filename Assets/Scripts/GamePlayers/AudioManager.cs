using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SESource;
    [SerializeField] AudioClip[] BGMs;
    [SerializeField] AudioClip[] SEs;
    [SerializeField] Text BGMpercent;
    [SerializeField] Text SEpercent;
    [SerializeField] GameObject BGMPanel;

    private static AudioManager Instance;
    public static int BGMnumber
    {
        get; set;
    }
    public static AudioManager instance
    {
        get { return Instance; }
    }

    public AudioSource BGMsource
    {
        get { return BGMSource; }
    }

    private void Update()
    {
    }

    private void Awake()
    {
        if(Instance == this)
        {
            return;
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        BGMpercent.text = (int)(BGMSource.volume * 100) + "%";
        SEpercent.text = (int)(SESource.volume * 100) + "%";
    }

    public void PlaySE(int index)
    {
        try
        {
            SESource.PlayOneShot(SEs[index]);
        }
        catch
        {
            Debug.Log("SE番号" + index + "が存在しません");
        }
    }

    public void PlayBGM(int index)
    {
        try
        {
            BGMSource.clip = BGMs[index];
            BGMSource.Play();
            BGMnumber = index;
        }
        catch
        {
            Debug.Log("BGM番号" + index + "が存在しません");
        }
    }

    public void PlayContinueBGM(int index)
    {
        float nowtime = BGMSource.time;
        try
        {
            BGMSource.clip = BGMs[index];
        }
        catch
        {
            Debug.Log("BGM番号" + index + "が存在しません");
        }
        BGMSource.Play();
        BGMSource.time = nowtime;
    }

    public static int GetBGMNumberFromProgress(float progress)
    {
        if (progress < 17)
            return 0;
        if (progress < 47)
            return 1;
        if (progress < 57.5f)
            return 2;
        if (progress < 82)
            return 3;

            return 4;
    }

    /// <summary>
    /// 以下音量調整用機能
    /// </summary>
    public void ChengeBGMvolume(float addvalue)
    {
        BGMSource.volume += addvalue;
        if (BGMSource.volume < 0) { BGMSource.volume = 0; }
        if (BGMSource.volume > 1) { BGMSource.volume = 1; }
        BGMpercent.text = (int)(BGMSource.volume * 100) + "%";
    }

    public void ChengeSEvolume(float addvalue)
    {
        SESource.volume += addvalue;
        if (SESource.volume < 0) { SESource.volume = 0; }
        if (SESource.volume > 1) { SESource.volume = 1; }
        SEpercent.text = (int)(SESource.volume * 100) + "%";
    }

    public void Enter(Text text)
    {
        text.color = new Color(0.3f,1,1,1);
    }

    public void Exit(Text text)
    {
        text.color = new Color(1f, 1, 1, 1);
    }

    public void OnOffPanel()
    {
        BGMPanel.SetActive(!BGMPanel.activeSelf);
    }
}
