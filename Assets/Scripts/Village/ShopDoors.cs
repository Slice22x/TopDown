using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopDoors : MonoBehaviour
{
    public Transform[] ShopDoorTransfrom;

    public static ShopDoors Doors { get; private set; }

    public Transform GetTransformFromDoor(int Index)
    {
        return ShopDoorTransfrom[Index];
    }

    void Awake()
    {
        Doors = this;
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }
}
