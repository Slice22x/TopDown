using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().m_Follow = Movement.Instance.transform;
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Boss || SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            if(Movement.Instance.GetComponent<Health>().enabled != true)
                Movement.Instance.GetComponent<Health>().enabled = true;
        }
        else if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
        {
            Cursor.visible = true;
        }
    }
}
