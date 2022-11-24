using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInfo : MonoBehaviour
{
    public static PortalInfo Instance;

    public Level ThisLevel;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Level.ResetLevelInfo(ThisLevel,false);
    }
}
