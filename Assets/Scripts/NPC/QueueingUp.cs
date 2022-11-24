using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueingUp : MonoBehaviour
{
    public float Speed;
    public float MoveTime;

    public void MoveTo(Vector3 Target)
    {
        //REMEMBER TO ADD THE EASE TYPE
        MoveTime = Vector2.Distance(Target, gameObject.transform.position) / Speed;
        LeanTween.move(gameObject, Target, MoveTime);
    }

    public void MoveToDo(Vector3 Target, System.Action NewAction)
    {
        //REMEMBER TO ADD THE EASE TYPE
        MoveTime = Vector2.Distance(Target, gameObject.transform.position) / Speed;
        LeanTween.move(gameObject, Target, MoveTime).setOnComplete(NewAction);
    }

    public void QueueUp(out bool InQueue, QueueHandeler Handle)
    {
        Handle.Queue.AddCustomer(this);
        InQueue = true;
    }

    public void QueueUp(QueueHandeler Handle)
    {
        Handle.Queue.AddCustomer(this);
    }

    public void QueueDown(out bool InQueue, QueueHandeler Handle)
    {
        Handle.Queue.QuitLine(this);
        InQueue = false;
    }
    public void MoveTo(Vector3 Target, NPCAI AI, NPCAI.State NextState)
    {
        //REMEMBER TO ADD THE EASE TYPE
        LeanTween.move(gameObject, Target, 1f);
        AI.ThisState = NextState;
    }

    public void MoveTo(NPCAI AI, NPCAI.State NextState)
    {
        AI.SwitchState(NextState);
    }

    public void MoveTo(Transform Target, NPCAI AI, NPCAI.State NextState,bool Bypass)
    {
        //REMEMBER TO ADD THE EASE TYPE
        AI.SwitchState(NextState);
        AI.SetTarget(Target);
        AI.Bypass = Bypass;
    }
}
