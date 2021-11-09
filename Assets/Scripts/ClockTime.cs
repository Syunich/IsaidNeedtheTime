using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClockTime : MonoBehaviour
{
    [SerializeField] TextMesh timeText;
    DateTime dt;

    private void Update()
    {
        dt = DateTime.Now;
        timeText.text = dt.ToString("HH  mm");
    }
}
