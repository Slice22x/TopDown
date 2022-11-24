using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;
using System.Linq;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]

public static class NPCDictionary
{
    public static Dictionary<string, NPCAI> WhatNPC = new Dictionary<string, NPCAI>();
    
    public static void AddToDictionary(NPCAI NPCToAdd)
    {
        if (WhatNPC.ContainsKey(NPCToAdd.NPC.Name))
        {
            MonoBehaviour.Destroy(NPCToAdd.gameObject);
        }
        else
        {
            WhatNPC.Add(NPCToAdd.NPC.Name, NPCToAdd);
        }
    }

    public static NPCAI RandomNPC()
    {
        return WhatNPC.Values.ElementAt(Random.Range(0, WhatNPC.Count));
    }
    
    public static NPCAI GetNPC(string Name)
    {
        return WhatNPC[Name];
    }

}

public class NPCAI : MonoBehaviour
{
    public enum State
    {
        Wandering,
        Shopping,
        Waiting,
        Queueing,
        Speaking,
        HomeLonging,
        WantToSpeakToNPC,
    }

    public State ThisState;
    public AIPath Path;
    private AIDestinationSetter Destination;
    public GameObject House;
    public GameObject[] Shops;
    public Transform Hand;
    public Transform ThisTarget;
    private Rigidbody2D Body;
    public bool InQueue;
    public int Index = -1;
    public string ThisPlace;
    public bool Bypass;
    public TypeOfNPC NPC;
    public bool IsSpeaking;
    public List<TypeOfDialogue> WantToSay = new List<TypeOfDialogue>();
    private List<TypeOfDialogue.Dialogue> ThingsToSay;
    public State StateBefore;
    public ItemScriptableObject QuestItem;
    public bool InShop;
    NPCAI SpeakTo;


    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {

        House = GameObject.Find(NPC.House.HouseName);
        switch (ThisState)
        {
            case State.Wandering:
                EnterWanderingState();
                break;
            case State.Waiting:
                EnterWaitingState();
                break;
            case State.Shopping:
                EnterShoppingState();
                break;
            case State.Queueing:
                EnterQueueingState();
                break;
            case State.HomeLonging:
                EnterHomeLonging();
                break;
            case State.WantToSpeakToNPC:
                EnterWantSpeak();
                break;
        }
    }

    private void Start()
    {
        Path = GetComponent<AIPath>();
        Path.maxSpeed = NPC.Speed;
        Path.enabled = false;
        Destination = GetComponent<AIDestinationSetter>();
        Body = GetComponent<Rigidbody2D>();
        GetComponent<QueueingUp>().Speed = NPC.Speed;
        NPCDictionary.AddToDictionary(this);

        switch (ThisState)
        {
            case State.Wandering:
                EnterWanderingState();
                break;
            case State.Waiting:
                EnterWaitingState();
                break;
            case State.Shopping:
                EnterShoppingState();
                break;
            case State.Queueing:
                EnterQueueingState();
                break;
            case State.HomeLonging:
                EnterHomeLonging();
                break;
            case State.Speaking:
                EnterSpeakingState();
                break;
            case State.WantToSpeakToNPC:
                EnterWantSpeak();
                break;
        }
    }

    private void Update()
    {
        if(Path.enabled == true)
        {
            Destination.target = ThisTarget;
        }
        switch (ThisState)
        {
            case State.Wandering:
                UpdateWanderingState();
                break;
            case State.Shopping:
                UpdateShoppingState();
                break;
            case State.Queueing:
                UpdateQueueingState();
                break;
            case State.HomeLonging:
                UpdateHomeLonging();
                break;
            case State.Speaking:
                UpdateSpeakingState();
                break;
            case State.WantToSpeakToNPC:
                UpdateWantSpeak();
                break;
        }
    }


    #region WANDERING STATE
    public void EnterWanderingState()
    {
        Path.enabled = true;
        Destination.target = ThisTarget;
    }
    public void UpdateWanderingState()
    {

    }
    public void ExitWanderingState()
    {
        Path.enabled = false;
    }
#endregion

    #region SHOPPING STATE
    public void EnterShoppingState()
    {
        Path.enabled = true;
        ThisTarget = Shops[Random.Range(0,Shops.Length)].transform.GetChild(0);
    }
    public void UpdateShoppingState()
    {

    }

    public void ExitShoppingState()
    {
        Path.enabled = false;
    }
    #endregion

    #region WAITING STATE
    public void EnterWaitingState()
    {
        Path.enabled = false;     
    }

    public void ExitWaitingState()
    {
        Path.enabled = true;
    }
    #endregion

    #region QUEUING STATAE
    public void EnterQueueingState()
    {
        Path.enabled = false;
        if (!InQueue && ThisState == State.Queueing)
        {
            GetComponent<QueueingUp>().QueueUp(out InQueue, ThisTarget.GetComponentInParent<QueueHandeler>());
        }
    }

    public void UpdateQueueingState()
    {

    }

    public void ExitQueueingState()
    {
        Path.enabled = true;
    }
    #endregion

    #region HOME LONGING STATE
    public void EnterHomeLonging()
    {

        Path.enabled = true;
        if(House == null)
        {
            House = GameObject.Find(NPC.House.HouseName);
        }
        ThisTarget = House.transform.GetChild(1);

    }
    public void UpdateHomeLonging()
    {

        Path.enabled = true;
        ThisTarget = House.transform.GetChild(1);
        
    }
    public void ExitHomeLonging()
    {
        
    }
    #endregion

    #region SPEAKING STATE

    public void EnterSpeakingState()
    {
        Dialogue.MakeNewBox(WantToSay[Random.Range(0, WantToSay.Count)], this);

        Path.enabled = false;

        Vector2 LookDir = (Vector2)Movement.Instance.transform.position - Body.position;
        float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        Body.rotation = Ang;
    }
    public void UpdateSpeakingState()
    {
        
    }
    public void ExitSpeakingState()
    {
        Path.enabled = true;
    }

    #endregion

    #region WANT TO SPEAK STATE

    public Vector2 SpeakingTimerRange;
    private float SpeakingTimer;

    public void EnterWantSpeak()
    {
        SpeakTo = NPCDictionary.RandomNPC();
        SpeakingTimer = Random.Range(SpeakingTimerRange.x, SpeakingTimerRange.y);
    }
    public void UpdateWantSpeak()
    {
        if(SpeakTo != null)
        {
            if (SpeakTo == this)
            {
                SpeakTo = NPCDictionary.RandomNPC();
            }
            else
            {
                SpeakTo.SwitchState(State.Waiting);
                Destination.target = SpeakTo.transform;
                float Dist = Vector2.Distance((Vector2)transform.position, (Vector2)SpeakTo.transform.position);
                Path.enabled = true;
                if (Dist <= 2f)
                {
                    Path.enabled = false;
                    Destination.target = null;
                    SpeakTo.Destination.target = null;
                    Vector2 LookDir = (Vector2)SpeakTo.transform.position - Body.position;
                    float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
                    Body.rotation = Ang;
                    Vector2 SLookDir = Body.position - (Vector2)SpeakTo.transform.position;
                    float SAng = Mathf.Atan2(SLookDir.y, SLookDir.x) * Mathf.Rad2Deg - 90f;
                    SpeakTo.Body.rotation = SAng;
                    SpeakingTimer -= Time.deltaTime;
                    if(SpeakingTimer <= 0f)
                    {
                        SwitchState(State.HomeLonging);
                    }
                }
            }

        }
    }
    public void ExitWantSpeak()
    {
        SpeakTo.SwitchState(State.HomeLonging);
        SpeakingTimer = Random.Range(SpeakingTimerRange.x, SpeakingTimerRange.y);
        SpeakTo = null;
    }

    #endregion

    #region OTHER FUNCTIONS

    public void SetTarget(Transform Target)
    {
        ThisTarget = Target;
    }

    public State RandomState()
    {
        var Reqest = (State)Random.Range(0, 7);
        
        if(Reqest == State.Queueing)
        {
            RandomState();
        }
        else
        {
            return Reqest;
        }
        return State.HomeLonging;
    }

    public void SwitchState(State NextState)
    {
        StateBefore = ThisState;
        switch (ThisState)
        {
            case State.Wandering:
                ExitWanderingState();
                break;
            case State.Waiting:
                ExitWaitingState();
                break;
            case State.Shopping:
                ExitShoppingState();
                break;
            case State.Queueing:
                ExitQueueingState();
                break;
            case State.HomeLonging:
                ExitHomeLonging();
                break;
            case State.Speaking:
                ExitSpeakingState();
                break;
            case State.WantToSpeakToNPC:
                ExitWantSpeak();
                break;
        }

        switch (NextState)
        {
            case State.Wandering:
                EnterWanderingState();
                break;
            case State.Waiting:
                EnterWaitingState();
                break;
            case State.Shopping:
                EnterShoppingState();
                break;
            case State.Queueing:
                EnterQueueingState();
                break;
            case State.HomeLonging:
                EnterHomeLonging();
                break;
            case State.Speaking:
                StateBefore = ThisState;
                EnterSpeakingState();
                break;
            case State.WantToSpeakToNPC:
                EnterWantSpeak();
                break;
        }
        ThisState = NextState;
    }

    public void SwitchState(State NextState,Transform Target)
    {
        StateBefore = ThisState;
        switch (ThisState)
        {
            case State.Wandering:
                ExitWanderingState();
                break;
            case State.Waiting:
                ExitWaitingState();
                break;
            case State.Shopping:
                ExitShoppingState();
                break;
            case State.Queueing:
                ExitQueueingState();
                break;
            case State.HomeLonging:
                ExitHomeLonging();
                break;
            case State.Speaking:
                break;
            case State.WantToSpeakToNPC:
                ExitWantSpeak();
                break;
        }

        switch (NextState)
        {
            case State.Wandering:
                EnterWanderingState();
                break;
            case State.Waiting:
                EnterWaitingState();
                break;
            case State.Shopping:
                EnterShoppingState();
                break;
            case State.Queueing:
                EnterQueueingState();
                break;
            case State.HomeLonging:
                EnterHomeLonging();
                break;
            case State.Speaking:
                break;
            case State.WantToSpeakToNPC:
                EnterWantSpeak();
                break;
        }
        ThisState = NextState;
    }
    #endregion
}
