using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] float savespan;
    [SerializeField] Transform Playertransform;
    float deltatime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playerinfo.CanControll)
            return;

        deltatime += Time.deltaTime;
        if(deltatime < savespan)
        {
            deltatime = 0;
            PlayerPrefs.SetFloat("passedtime", GameManager.instance.playerinfo.Passedtime);
            PlayerPrefsUtils.SetObject("playerposition", Playertransform.position);
        }
    }
}
