using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartAdder : MonoBehaviour
{
    public int Index;

    public void LoadHearts()
    {
        FindObjectOfType<HeartManager>().HeartImgs.Add(Index,GetComponent<Image>());
    }
}
