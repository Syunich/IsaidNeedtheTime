using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerInfo PlayerInfo;
    private static GameManager Instance;
    private float deltatime;

    public PlayerInfo playerinfo { get { return PlayerInfo; } }

    public static GameManager instance { get { return Instance; } }
    [SerializeField] ProgressIndicator progressindicator;
    [SerializeField] Player_Moving pm;
    [SerializeField] public bool IsTutorial;
    [SerializeField] Vector3 startpos;

    private void Awake()
    {
        Instance = this;

            PlayerInfo                 = new PlayerInfo();
            PlayerInfo.Passedtime      = PlayerPrefs.GetFloat("passedtime",0);

        if(!IsTutorial)
        {
            if (PlayerPrefs.HasKey("playerposition"))
                pm.gameObject.transform.position = PlayerPrefsUtils.GetObject<Vector3>("playerposition");
           
        }

        deltatime  = 0;
        FeedManager.instance.Feedin(() => Debug.Log("MainorTitle読み込み完了"));
        AudioManager.BGMnumber = 0;
        AudioManager.instance.PlayBGM(AudioManager.BGMnumber);
    }

    private void Update()
    {
        if (IsTutorial || !playerinfo.CanControll)
            return;

        deltatime  += Time.deltaTime;
        playerinfo.Passedtime  += Time.deltaTime;
        playerinfo.StartUpTime += Time.deltaTime;

        //進捗度合いと経過時間の処理
        if (deltatime > 0.5f)
        {
            deltatime = 0;
            PlayerInfo.Progress = progressindicator.GetProgress();
            GamingUI.instance.ChangeProgressText(playerinfo.Progress);
        }
        GamingUI.instance.ChangeTimeText(playerinfo.Passedtime);

        //音変更の処理
        if(AudioManager.BGMnumber != AudioManager.GetBGMNumberFromProgress(playerinfo.Progress))
        {
           AudioManager.BGMnumber = AudioManager.GetBGMNumberFromProgress(playerinfo.Progress);
           AudioManager.instance.PlayContinueBGM(AudioManager.BGMnumber);
        }


    }

    public void GameClear()
    {
        int milesec = (int)playerinfo.Passedtime * 1000;
        var timeScore = new System.TimeSpan(0, 0, 0, 0, milesec);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(timeScore , 1);
        Cursor.visible = true;
        playerinfo.CanControll = false;
        Resetinfo();
    }

    public void ReStart()
    {
        playerinfo.CanControll = false;
        Resetinfo();
        FeedManager.instance.FeedOut("MainScene");
    }

    public void Resetinfo()
    {
        PlayerPrefs.SetFloat("passedtime", 0);
        PlayerPrefsUtils.SetObject("playerposition", startpos);
    }

}
