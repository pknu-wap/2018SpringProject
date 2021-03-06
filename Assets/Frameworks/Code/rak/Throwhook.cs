﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwhook : MonoBehaviour
{
    public GameObject hook;
    public bool ropeActive;
    private GameObject curHook;

    [Range(0, 2)]
    public float ropeBlockTime;
    private bool isBlockedRope;

    // 로프 취소
    public static Action RopeCancle;
    public PlayerAniController ani;

    private void Start()
    {
        RopeCancle += CancelRope;
    }

    // Update is called once per frame
    void Update()
    {

    #if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            Throw();
        }
    #endif
    #if (UNITY_ANDROID || UNITY_IOS)
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Throw();
        }
    #endif
    }
    
    private void Throw()
    {
        if (ropeActive == false && !isBlockedRope)
        {
            Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            curHook = Instantiate(hook, transform.position, Quaternion.identity);

            curHook.GetComponent<RopeScript>().destiny = destiny;

            ropeActive = true;
            PlayerAniController.swingAni();
            AudioManager.instance.PlaySingleEffect(GameManager.instance.ropeSound);
        }
        else
        {
            Destroy(curHook);
            ani.StateReset();
            ropeActive = false;
          //  AudioManager.instance.PlaySingleEffect(GameManager.instance.flySound);
        }
    }

    // 로프가 지형이 아닌곳에 던져진 경우 바로 지워지도록
    public void CancelRope()
    {
        isBlockedRope = true;
        if (this != null)
        {
            Invoke("UnBlockRope", ropeBlockTime);
        }
        if (curHook != null)
        {
            Destroy(curHook);
        }
        ropeActive = false;
    }
    private void UnBlockRope()
    {
        isBlockedRope = false;
    }
}
