using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;
using Random = UnityEngine.Random;
using Cinemachine;
using Pathfinding;

public class Dialogue : MonoBehaviour
{
    public NPCAI TalkingTo;
    [SerializeField]TMP_Text TextBox;
    [SerializeField] TMP_Text NameBox;
    int CurrentlyShowing;
    public bool IsTalking;

    //Dialogue
    public List<TypeOfDialogue.Dialogue> Speach = new List<TypeOfDialogue.Dialogue>();
    public TypeOfDialogue DialogueHolder;
    public GameObject Caller;
    [SerializeField] string TextToDisplay;
    float Timer;
    public float TimeBtwChar;
    int CharIndex;
    [SerializeField] bool HasCompleted;

    public GameObject NextIndicator;

    public static Dialogue Instance;

    public LeanTweenType TweenType;

    public GameObject Prompt;
    GameObject NewPrompt;
    GameObject LookTarget;

    bool IsClosing;
    public bool Finished;

    GameObject Obj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
        DontDestroyOnLoad(transform.parent.gameObject);
        SetScale(new Vector3(0f, 0f), DisableThis);
    }

    void Start()
    {
        TextBox = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        Finished = false;
        // Freeze the Player and Enemy
        if (TalkingTo != null)
        {
            TalkingTo.Path.enabled = false;
        }
        if(IsTalking)
        {
            if (Movement.Instance != null)
            {
                Movement.Instance.InAction = true;
            }
            if (StoryMovement.Instance != null)
            {
                StoryMovement.Instance.InAction = true;
            }
        }

    }

    private void OnDisable()
    {
        if (Movement.Instance != null)
            Movement.Instance.InAction = false;
        if (StoryMovement.Instance != null)
            StoryMovement.Instance.InAction = false;
    }

    public static void MakeNewBox(TypeOfDialogue Talk)
    {
        Instance.IsTalking = true;
        Instance.Speach = Talk.Speach.DialogueToSay;
        Instance.DialogueHolder = Talk;
        Instance.enabled = true;
        Instance.SetScale(new Vector3(1f, 1f), Instance.LoadText);
    }

    public static void MakeNewBox(TypeOfDialogue Talk, GameObject CallObject)
    {
        Instance.IsTalking = true;
        Instance.Speach = Talk.Speach.DialogueToSay;
        Instance.Caller = CallObject;
        Instance.DialogueHolder = Talk;
        Instance.enabled = true;
        Instance.SetScale(new Vector3(1f, 1f), Instance.LoadText);
    }

    public static void MakeNewBox(TypeOfDialogue Talk, NPCAI TalkTo)
    {
        Instance.IsTalking = true;
        Instance.Speach = Talk.Speach.DialogueToSay;
        Instance.DialogueHolder = Talk;
        Instance.enabled = true;
        Instance.TalkingTo = TalkTo;
        Instance.SetScale(new Vector3(1f, 1f), Instance.LoadText);
    }

    void DisableThis()
    {
        Time.timeScale = 1f;
        Finished = true;
        IsTalking = false;
        enabled = false;
    }

    public void SetScale(Vector3 Alpha, Action Method)
    {
        LeanTween.scale(gameObject, Alpha, 0.5f).setEase(TweenType).setOnComplete(Method);
    }

    void ExitSpeaking()
    {
        //Disable everything         
        //Let player and NPC move again
        IsClosing = true;
        if(TalkingTo != null)
        {
            TalkingTo.SwitchState(TalkingTo.StateBefore);
        }
        if(Movement.Instance != null)
        {
            Movement.Instance.InAction = false;
            Movement.Instance.NPC_Checker.enabled = false;
            Movement.Instance.MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
        if(StoryMovement.Instance != null)
        {
            StoryMovement.Instance.InAction = false;
        }
        TalkingTo = null;
        enabled = false;
        TextBox.text = "";
        NextIndicator.SetActive(false);
        CurrentlyShowing = 0;
        Speach = new List<TypeOfDialogue.Dialogue>();
        TextToDisplay = null;
        HasCompleted = false;
        Finished = true;
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            PortalHealth.Instance.StartGame();
        }
        DisableThis();
    }

    void PromptPlayer()
    {
        if (HasCompleted)
        {
            NewPrompt = Instantiate(Prompt, transform.parent, false);
            Prompt PromptComponent = NewPrompt.GetComponent<Prompt>();
            PromptComponent.TypePrompt = TypeOfPrompt.Item;
            PromptComponent.ItemImage.sprite = ItemFromID.GetItemFromName(TalkingTo.QuestItem.Name).ItemOfChoice.ArtWork;
            PromptComponent.YesText.text = TalkingTo.NPC.LoveForItem(TalkingTo.QuestItem.ItemGameObject.GetComponent<Item>()).ToString();
            PromptComponent.NoText.text = "-7";
            PromptComponent.NoCarryOn = true;
        }
    }

    void Update()
    {
        if (TalkingTo != null)
        {     
            Movement.Instance.MousePos = TalkingTo.transform.position;
            Movement.Instance.InteractObj.enabled = false;
        }

        if (Movement.Instance != null)
        {
            if (DialogueHolder.SpeakingToObj && Caller != null)
            {
                Vector2 ObjectPos = ((Vector2)Caller.transform.position - (Vector2)Movement.Instance.transform.position).normalized;
                Movement.Instance.Anim.SetFloat("Horizontal", ObjectPos.x);
                Movement.Instance.Anim.SetFloat("Vertical", ObjectPos.y);
            }
        }

        if (StoryMovement.Instance != null)
        {
            if (DialogueHolder.SpeakingToObj && Caller != null)
            {
                Vector2 ObjectPos = ((Vector2)Caller.transform.position - (Vector2)StoryMovement.Instance.transform.position).normalized;
                StoryMovement.Instance.Anim.SetFloat("Horizontal", ObjectPos.x);
                StoryMovement.Instance.Anim.SetFloat("Vertical", ObjectPos.y);
            }
        }


        //Display the text
        if (TextToDisplay != null && !HasCompleted)
        {
            Timer -= Time.unscaledDeltaTime;
            while (Timer <= 0)
            {
                Timer += TimeBtwChar;
                CharIndex++;
                if(CharIndex != TextToDisplay.Length + 1)
                {
                    string text = "<color=#000000ff>" + TextToDisplay.Substring(0, CharIndex) + "</color>";
                    TextBox.text = text;
                }

            }

        }

        //if the dialogue is not done yet, dont allow the player to skip to the next line
        if (!IsClosing)
        {
            if (TextToDisplay != null)
            {
                if (CharIndex > TextToDisplay.Length)
                {
                    HasCompleted = true;
                    NextIndicator.SetActive(true);
                    //If done speaking show the button to progress
                    if(Movement.Instance != null)
                    {
                        if (Movement.Instance.Interact.triggered)
                        {
                            if (NewPrompt == null)
                            {
                                LoadNextText();
                            }
                            else if (NewPrompt != null)
                            {
                                if (!NewPrompt.GetComponent<Prompt>().NoCarryOn)
                                {
                                    LoadNextText();
                                }
                            }

                        }
                    }

                    if(StoryMovement.Instance != null)
                    {
                        if (StoryMovement.Instance.Interact.triggered)
                        {
                            if (NewPrompt == null)
                            {
                                LoadNextText();
                            }
                            else if (NewPrompt != null)
                            {
                                if (!NewPrompt.GetComponent<Prompt>().NoCarryOn)
                                {
                                    LoadNextText();
                                }
                            }

                        }
                    }

                }
                else if (CharIndex <= TextToDisplay.Length)
                {
                    if (Movement.Instance != null)
                    {
                        if (Movement.Instance.Interact.triggered)
                        {
                            HasCompleted = true;
                            TextBox.text = TextToDisplay;
                            CharIndex = TextToDisplay.Length;
                        }
                    }
                    if (StoryMovement.Instance != null)
                    {
                        if (StoryMovement.Instance.Interact.triggered)
                        {
                            HasCompleted = true;
                            TextBox.text = TextToDisplay;
                            CharIndex = TextToDisplay.Length;
                        }
                    }

                    HasCompleted = false;
                    NextIndicator.SetActive(false);
                }
            }
        }
        //Remove the blur effect

    }

    public void LoadText()
    {
        IsClosing = false;

        if (Speach != null)
        {
            if (CurrentlyShowing < Speach.Count - 1)
            {
                HasCompleted = false;
                TextToDisplay = Speach[CurrentlyShowing].Speak;
                NameBox.text = Speach[CurrentlyShowing].Name;
                CharIndex = 0;
                //Replace text refrences with the correct thing
                if (TextToDisplay.Contains("*"))
                {
                    ReplaceText(ref TextToDisplay, "*Person*", TalkingTo.NPC.Name);
                    ReplaceText(ref TextToDisplay, "*Place*", TalkingTo.ThisTarget.name);
                    ReplaceText(ref TextToDisplay, "*State*", System.Enum.GetName(typeof(NPCAI.State), TalkingTo.StateBefore));
                    ReplaceText(ref TextToDisplay, "*FavItem*", TalkingTo.NPC.FavItem.Name);
                    ReplaceText(ref TextToDisplay, "*Item*", TalkingTo.QuestItem.Name);
                    CallText(ref TextToDisplay, "*PromptItemGive*", PromptPlayer, "");
                }
                switch (Speach[CurrentlyShowing].Type)
                {
                    case TypeOfDialogue.Dialogue.TypeOfSpeach.Normal:
                        if (Movement.Instance != null)
                            FindObjectOfType<CinemachineVirtualCamera>().m_Follow = Movement.Instance.transform;
                        if (StoryMovement.Instance != null)
                            FindObjectOfType<CinemachineVirtualCamera>().m_Follow = StoryMovement.Instance.transform;
                        Destroy(Obj);
                        break;
                    case TypeOfDialogue.Dialogue.TypeOfSpeach.LookAt:
                        FindObjectOfType<CinemachineVirtualCamera>().m_Follow = LookTarget.transform;
                        Destroy(Obj);
                        break;
                    case TypeOfDialogue.Dialogue.TypeOfSpeach.Destroy:
                        Destroy(Obj);
                        break;
                    case TypeOfDialogue.Dialogue.TypeOfSpeach.CallOver:
                        break;
                    case TypeOfDialogue.Dialogue.TypeOfSpeach.SpawnEnemy:
                        PortalHealth.Instance.Spawner.SpawnEnemy(Speach[CurrentlyShowing].ActionObject, out Obj, new Vector2(-20f, 80f));
                        FindObjectOfType<CinemachineVirtualCamera>().m_Follow = Obj.transform;
                        Obj.GetComponent<Enemies>().DontSearch = true;
                        break;
                    case TypeOfDialogue.Dialogue.TypeOfSpeach.Spawn:
                        GameObject ToSpawn = Resources.Load(Speach[CurrentlyShowing].ActionObject, typeof(GameObject)) as GameObject;
                        break;
                }
            }
        }
    }

    void LoadNextText()
    {              
        {
            if(CurrentlyShowing < Speach.Count - 1)
            {
                HasCompleted = false;
                CurrentlyShowing++;
                if(Speach[CurrentlyShowing].Speak != null)
                {
                    TextToDisplay = Speach[CurrentlyShowing].Speak;
                    NameBox.text = Speach[CurrentlyShowing].Name;
                }
                CharIndex = 0;
                //Replace text refrences with the correct thing
                if (TextToDisplay.Contains("*"))
                {
                    ReplaceText(ref TextToDisplay, "*Person*", TalkingTo.NPC.Name);
                    ReplaceText(ref TextToDisplay, "*Place*", TalkingTo.ThisTarget.name);
                    ReplaceText(ref TextToDisplay, "*State*", System.Enum.GetName(typeof(NPCAI.State), TalkingTo.StateBefore));
                    ReplaceText(ref TextToDisplay, "*FavItem*", TalkingTo.NPC.FavItem.Name);
                    ReplaceText(ref TextToDisplay, "*Item*", TalkingTo.QuestItem.Name);
                    CallText(ref TextToDisplay, "*PromptItemGive*", PromptPlayer, "");
                }

            }
            else if (CurrentlyShowing >= Speach.Count - 1)
            {
                SetScale(new Vector3(0f, 0f), ExitSpeaking);
            }
            LookTarget = GameObject.Find(Speach[CurrentlyShowing].ActionObject);
            switch (Speach[CurrentlyShowing].Type)
            {
                case TypeOfDialogue.Dialogue.TypeOfSpeach.Normal:
                    if (Movement.Instance != null)
                        FindObjectOfType<CinemachineVirtualCamera>().m_Follow = Movement.Instance.transform;
                    if (StoryMovement.Instance != null)
                        FindObjectOfType<CinemachineVirtualCamera>().m_Follow = StoryMovement.Instance.transform;
                    Destroy(Obj);
                    break;
                case TypeOfDialogue.Dialogue.TypeOfSpeach.LookAt:
                    FindObjectOfType<CinemachineVirtualCamera>().m_Follow = LookTarget.transform;
                    Destroy(Obj);
                    break;
                case TypeOfDialogue.Dialogue.TypeOfSpeach.Destroy:
                    Destroy(Obj);
                    break;
                case TypeOfDialogue.Dialogue.TypeOfSpeach.CallOver:
                    break;
                case TypeOfDialogue.Dialogue.TypeOfSpeach.SpawnEnemy:
                    PortalHealth.Instance.Spawner.SpawnEnemy(Speach[CurrentlyShowing].ActionObject, out Obj, new Vector2(-20f,80f));
                    FindObjectOfType<CinemachineVirtualCamera>().m_Follow = Obj.transform;
                    Obj.GetComponent<Enemies>().DontSearch = true;
                    break;
            }
        }
    }

    void ReplaceText(ref string Text, string ThingToReplace, string ReplaceWith)
    {
        if (Text.Contains(ThingToReplace))
        {
            Text = Text.Replace(ThingToReplace, ReplaceWith);
        }
        else
        {
            return;
        }
    }

    void CallText(ref string Text, string Check, Action Call, string ReplaceWith)
    {
        if (Speach[CurrentlyShowing].Speak.Contains(Check))
        {
            if (HasCompleted)
            {
                Call.Invoke();
            }
            Text = Text.Replace(Check, ReplaceWith);
        }
    }

    void ReplaceText(ref string Text, string ThingToReplace, string ReplaceWith, string OpenCase, string CloseCase)
    {
        if (Speach[CurrentlyShowing].Speak.Contains(ThingToReplace))
            Text = Text.Replace(ThingToReplace,OpenCase + ReplaceWith + CloseCase);       
    }

    void WobbleText(string TextToWobble, float Amount)
    {
        TextBox.ForceMeshUpdate();
        var TextInfo = TextBox.GetTextInfo(TextToWobble);

        for (int i = 0; i < TextInfo.characterCount; i++)
        {
            var Charinfo = TextInfo.characterInfo[i];

            if (!Charinfo.isVisible)
            {
                continue;
            }

            var Verts = TextInfo.meshInfo[Charinfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; j++)
            {
                var orig = Verts[Charinfo.vertexIndex + j];
                Verts[Charinfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * Amount);

            }
        }
        for (int i = 0; i < TextInfo.meshInfo.Length; i++)
        {
            var MeshInfo = TextInfo.meshInfo[i];
            MeshInfo.mesh.vertices = MeshInfo.vertices;
            TextBox.UpdateGeometry(MeshInfo.mesh, i);
        }
    }
}
