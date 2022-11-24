using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Detection))]
public class ShopsInTown : MonoBehaviour
{
    private Detection Detect;
    private GameObject Outline;
    public string SceneName;
    private void Awake()
    {
        Outline = transform.GetChild(0).gameObject;
        Detect = GetComponent<Detection>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Detect.Detected)
        {
            Outline.SetActive(true);
            Movement.Instance.ActionPrompt = true;
     
            if (Detect.ObjectsInArea.CompareTag("Player") && Movement.Instance.Interact.triggered)
            {

                NPCDictionary.WhatNPC.Clear();
                ScreenTrans.CallLevel(SceneName);

            }
        }
        if (!Detect.Detected)
        {
            Outline.SetActive(false);
        }
    }
}
