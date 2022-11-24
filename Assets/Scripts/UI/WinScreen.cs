using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public GameObject WinCanvas;

    void Start()
    {
        BossHealth.Death += BossHealth_Death;
    }

    private void BossHealth_Death()
    {
        WinCanvas.SetActive(true);
        BossHealth.Death -= BossHealth_Death;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
