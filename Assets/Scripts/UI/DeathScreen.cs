using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public GameObject DeathCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallDeath()
    {
        if (Health.Instance == null || Health.Instance != null && Health.Instance.Hearts == -1)
        {
            DeathCanvas.SetActive(true);
            Cursor.visible = true;
        }
    }
}
