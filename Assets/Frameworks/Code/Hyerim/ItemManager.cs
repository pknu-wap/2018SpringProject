﻿using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public bool isUsingItem = false;
    
    public void BigItem(int runTime)
    {
        // 거대화 아이템 효과
        print("거대화 아이템 발동");
    }

    public void SlowItem(int runTime)
    {
        // 슬로우 아이템 효과
        Time.timeScale = 0.5f;
        Invoke("ResetTime", runTime);
    }

    public void FastItem(int runTime)
    {
        Time.timeScale = 2f;
        Invoke("ResetTime", runTime);
    }
    private void ResetTime()
    {
        Time.timeScale = 1;
    }


}