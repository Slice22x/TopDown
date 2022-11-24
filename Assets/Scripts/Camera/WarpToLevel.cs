using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpToLevel : MonoBehaviour
{
    private Detection Detect;
    private GameObject Outline;
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
                UIPlayerMover.Instance.SetActive(true);
            }
        }
        if (!Detect.Detected)
        {
            Outline.SetActive(false);
        }
    }
}
