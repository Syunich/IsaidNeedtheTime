using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TitleTexts : MonoBehaviour
{
    private TextMeshProUGUI TMP;
    [SerializeField] string Name;
    private void Start()
    {
        TMP = GetComponent<TextMeshProUGUI>();
    }

    public void PointerEnter()
    {
        TMP.text = "<u>" + Name + "</u>";
    }
    public void PointerExit()
    {
        TMP.text = Name;
    }
    public void Click(string scenename)
    {
        AudioManager.instance.PlaySE(5);
    }

}
