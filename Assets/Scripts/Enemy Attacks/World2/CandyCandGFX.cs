using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Enemy_Attacks.World2
{
    public class CandyCandGFX : MonoBehaviour
    {
        public void Launch()
        {
            StartCoroutine(GetComponentInParent<CandyCane>().SpawnExp());
        }
    }
}