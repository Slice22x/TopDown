using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueHandeler : MonoBehaviour
{
    public Transform FirstPos;
    public float PositionSizes;
    public int QueueSize;
    public WaitingQueue Queue;
    void Start()
    {
        List<Vector3> QueuePositions = new List<Vector3>();
        Vector3 FirstPosition = FirstPos.position;
        for (int i = 0; i < QueueSize; i++)
        {
            QueuePositions.Add(FirstPosition + Vector3.down * PositionSizes * i);
        }
        Queue = new WaitingQueue(QueuePositions, PositionSizes);
    }

}
