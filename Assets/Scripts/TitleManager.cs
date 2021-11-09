using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
    }
    private void Start()
    {
        FeedManager.instance.Feedin(() => Debug.Log("Title読み込み完了")) ;
        AudioManager.instance.PlayBGM(5);
    }

   public void GoMainScene()
    {
        FeedManager.instance.FeedOut("MainScene");
    }

    public void GoTutorialScene()
    {
        FeedManager.instance.FeedOut("TutorialScene");
    }

    public void OnOffAudioPanel()
    {
        AudioManager.instance.OnOffPanel();
    }


}
