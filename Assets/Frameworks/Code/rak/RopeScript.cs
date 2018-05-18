﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    [Header("Values")]
    public Vector2 destiny;
    public float speed;
    public float distance;

    [Space]
    [Header("Objects")]
    public GameObject nodePrefab;
    public GameObject player;
    public GameObject lastNode;
    public LineRenderer lr;
    private Throwhook throwHook;

    private int vertexCount = 2;
    public List<GameObject> Nodes = new List<GameObject>();
    private bool done = false;

    void Start()
    {

        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        throwHook = player.GetComponent<Throwhook>();
        lastNode = transform.gameObject;

        Nodes.Add(transform.gameObject);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, destiny, speed);

        if ((Vector2)transform.position != destiny)
        {
            if (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (done == false)
        {
            done = true;

            while (Vector2.Distance(player.transform.position, lastNode.transform.position) > distance)
            {
                CreateNode();
            }

            lastNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
        }

        RenderLine();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("RopeAble"))
        {
            //print("로프가 닿을 수 없는 곳");
            //throwHook.CancelRope();
        }
    }


    void RenderLine()
    {
        // lr.SetVertexCount(vertexCount); -> 옛날 버전에서 쓰던것
        lr.positionCount = vertexCount; // -> 현재 버전에서 쓰는것

        int i;
        for (i = 0; i < Nodes.Count; i++)
        {
            lr.SetPosition(i, Nodes[i].transform.position);
        }

        lr.SetPosition(i, player.transform.position);
    }

    void CreateNode()
    {
        Vector2 pos2Create = player.transform.position - lastNode.transform.position;
        pos2Create.Normalize();
        pos2Create *= distance;
        pos2Create += (Vector2)lastNode.transform.position;

        GameObject go = Instantiate(nodePrefab, pos2Create, Quaternion.identity);

        go.transform.SetParent(transform);

        lastNode.GetComponent<HingeJoint2D>().connectedBody = go.GetComponent<Rigidbody2D>();

        lastNode = go;

        Nodes.Add(lastNode);

        vertexCount++;
    }
}