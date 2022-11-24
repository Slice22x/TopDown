using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New House", menuName = "Create House")]
public class HouseInfo : ScriptableObject
{
    public string HouseName;
    public string SceneName;
    public Vector3 EntrancePosition;
    public Vector3 ExitPosition;
}
