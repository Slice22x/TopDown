using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Effect : MonoBehaviour
{
    public void KillSelf()
    {
        Destroy(gameObject);
    }

    public void Transition()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
