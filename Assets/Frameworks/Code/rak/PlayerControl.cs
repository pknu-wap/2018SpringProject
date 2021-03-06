﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour
{
    private class EndMapCheckerX
    {
        public bool BIsEndOfMapX { set; get; }
    }

    private class EndMapCheckerY
    {
        public bool BlsEndOfMapY { set; get; }
    }

    EndMapCheckerX endMapCheckerX = new EndMapCheckerX();
    EndMapCheckerY endMapCheckerY = new EndMapCheckerY();

    private Rigidbody2D rigidBody;

    [Header("Value")]
    public float forceToAdd;
    public int hitForce;
    public PlayerAniController ani;

    public static Action<int> Big;

    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
        Big = BigItemEffect;
    }

    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetKey(KeyCode.A))
        {

            GetComponent<Rigidbody2D>().AddForce(-Vector2.right * forceToAdd);
            ani.LeftMove();
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * forceToAdd);
            ani.RightMove();
        }
#endif

#if (UNITY_IOS || UNITY_ANDROID)
        if (Input.acceleration.x < 0)
        {

            GetComponent<Rigidbody2D>().AddForce(-Vector2.right * forceToAdd);
            ani.LeftMove();
        }
        if (Input.acceleration.x > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.right * forceToAdd);
            ani.RightMove();
        }
#endif

        if (rigidBody.velocity == Vector2.zero)
        {
            ani.MovePause();
        }
        CameraControl();
    }

    // 카메라 제어
    private void CameraControl()
    {
        if (endMapCheckerY.BlsEndOfMapY == false && endMapCheckerX.BIsEndOfMapX == false)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        }
        else if (endMapCheckerY.BlsEndOfMapY == true || endMapCheckerX.BIsEndOfMapX == true)
        {
            if (endMapCheckerY.BlsEndOfMapY == true && endMapCheckerX.BIsEndOfMapX == true)
            {
                Camera.main.transform.position = new Vector3(
                                                    Camera.main.transform.position.x,
                                                    Camera.main.transform.position.y,
                                                    Camera.main.transform.position.z);
            }
            else if (endMapCheckerY.BlsEndOfMapY == true)
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            else
                Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }


    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            print("적과의 충돌");
            var vector = transform.position - col.transform.position;
            rigidBody.AddForce(vector * hitForce);
            HitEffect.OnHitEffect();
            ani.Hit();
            AudioManager.instance.PlaySingleEffect(GameManager.instance.hitSound);
        }
        if (col.gameObject.CompareTag("Spike"))
        {
            print("가시를 밟았다.");
            var vector = transform.position - col.transform.position;
            rigidBody.AddForce(vector * hitForce);
            col.gameObject.GetComponent<SpikeControl>().SpikeTrapExecute();
            AudioManager.instance.PlaySingleEffect(GameManager.instance.hitSound);
        }
    }
    // 트리거 제어
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            print("코인 획득");
            col.transform.parent.gameObject.SetActive(false);
            Coin.OnCollideCoin();
        }
        if (col.CompareTag("Item"))
        {
            print("아이템 획득");
            var data = ItemTable.GetData(col.name);
            if (data != null)
            {
                data.ItemAction(data.runTime);
                GetItemEffect.OnItemEffect();
            }
            col.gameObject.SetActive(false);
        }
        if (col.CompareTag("Fever"))
        {
            print("FEVER");
            col.gameObject.SetActive(false);
            Fever.OnHitFever(col);
        }

        if (col.CompareTag("Spike"))
        {
            print("가시가 근처에 있다.");
            col.GetComponent<SpikeControl>().SpikeTrapExecute();
        }

        if (col.CompareTag("EndMapX"))
        {
            endMapCheckerX.BIsEndOfMapX = true;
        }

        if (col.CompareTag("EndMapY"))
        {
            endMapCheckerY.BlsEndOfMapY = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("EndMapX"))
        {
            endMapCheckerX.BIsEndOfMapX = false;
        }

        if (col.CompareTag("EndMapY"))
        {
            endMapCheckerY.BlsEndOfMapY = false;
        }
    }

    private void BigItemEffect(int runTime)
    {
        transform.localScale *= 2;
        Invoke("ResetBig", runTime);
    }

    private void ResetBig()
    {
        transform.localScale /= 2;
    }
}
