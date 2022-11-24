using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour
{
    public void Freeae()
    {
        GetComponentInParent<Enemies>().DontSearch = false;
    }
    public void Cont()
    {
        GetComponentInParent<Enemies>().DontSearch = true;
    }
}
