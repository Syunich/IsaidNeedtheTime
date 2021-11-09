using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話イベント制御
/// </summary>
public class TalkManager : MonoBehaviour
{
    [SerializeField] string ThirtyText;
    [SerializeField] string HourText;
    [SerializeField] string TwoHourText;

    private TalkManager Instance;
    public TalkManager instance
    { get { return Instance; } }

    private bool IsIndicatedThirty;
    private bool IsIndicatedHour;
    private bool IsIndicatedTwoHour;
    private PlayerInfo info;
    private void Awake()
    {
        if (instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        IsIndicatedThirty = IsIndicatedHour = IsIndicatedTwoHour = false;
        info = GameManager.instance.playerinfo;
    }
    void Update()
    {
        if (info.IsNowTalking)
        {
            return;
        }

        if (info.StartUpTime < 30 * 60)
        {
            return;
        }

        if(!IsIndicatedThirty)
        {
            Debug.Log("called");
            info.IsNowTalking = true;
            IsIndicatedThirty = true;
          StartCoroutine(GamingUI.instance.IndicateText(0.1f, ThirtyText, 2, () => info.IsNowTalking = false));
            return;
        }

        if (info.StartUpTime < 60 * 60)
        {
            return;
        }

        if (!IsIndicatedHour)
        {
            info.IsNowTalking = true;
            IsIndicatedHour   = true;
           StartCoroutine(GamingUI.instance.IndicateText(0.1f, HourText, 3, () => info.IsNowTalking = false));
            return;
        }
        if (info.StartUpTime < 120 * 60)
        {
            return;
        }

        if (!IsIndicatedTwoHour)
        {
            IsIndicatedTwoHour = true;
            info.IsNowTalking  = true;
           StartCoroutine(GamingUI.instance.IndicateText(0.1f, TwoHourText, 4, () => info.IsNowTalking = false));
            return;
        }
    }
}
