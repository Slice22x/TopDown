using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaitingQueue
{
    public List<QueueingUp> CustomerList;
    public List<Vector3> PositionList;
    private Vector3 EntrancePos;

    public WaitingQueue(List<Vector3> PositionList, float PositionSize)
    {
        this.PositionList = PositionList;
        EntrancePos = PositionList[PositionList.Count - 1] + new Vector3(-PositionSize, 0);

        CustomerList = new List<QueueingUp>();
    }

    public void AddCustomer(QueueingUp Customer)
    {
            CustomerList.Add(Customer);
            Customer.MoveToDo(EntrancePos, () =>
            {
                Customer.MoveTo(PositionList[CustomerList.IndexOf(Customer)]);
            });

    }

    public bool IsInFirst()
    {
        return CustomerList[0] != null;
    }

    public QueueingUp GetFirst()
    {
        if(CustomerList.Count <= 0)
        {
            return null;
        }
        else
        {
            QueueingUp Up = CustomerList[0];
            return Up;
        }
    }

    public void QuitLine(QueueingUp Index)
    {
        CustomerList.Remove(Index);
        UpdateLine();
    }

    public void UpdateLine()
    {
        for (int i = 0; i < CustomerList.Count; i++)
        {
            CustomerList[i].MoveTo(PositionList[i]);
        }
    }
}