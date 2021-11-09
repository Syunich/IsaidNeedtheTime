using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ユーザーに関する設定など
/// </summary>
public class UserManager
{
    public static void Show_or_HideVisualHook(Player_Moving player) 
    {
      var fj =  player.GetComponent<FookGenerator>();
        if(fj == null)
        {
            Debug.LogError("フックが参照できません");
            return;
        }

        var sprite = fj.visualfook.GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer sr in sprite)
        {
            sr.enabled = !sr.enabled;
        }
        GameManager.instance.playerinfo.IsShowFookGuide = sprite[0].enabled;
    }

    public static void Show_or_HideVisualHook(Player_Moving player , bool isShow)
    {
        var fj = player.GetComponent<FookGenerator>();
        if (fj == null)
        {
            Debug.LogError("フックが参照できません");
            return;
        }

        var sprite = fj.visualfook.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in sprite)
        {
            sr.enabled = isShow;
        }
        GameManager.instance.playerinfo.IsShowFookGuide = isShow;
    }

    public static void Show_or_HideOprion()
    {
        AudioManager.instance.OnOffPanel();
    }
}
