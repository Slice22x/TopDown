using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using Cinemachine;

public class Inventory
{
    static List<ItemScriptableObject> ItemInventory = new List<ItemScriptableObject>();

    public static List<ItemScriptableObject> CurrentInv()
    {
        return ItemInventory;
    }

    public static void AddItem(ItemScriptableObject Item)
    {
        ItemInventory.Add(Item);
    }

    public static ItemScriptableObject GetItem(ItemScriptableObject ToFind)
    {
        var ReturnItem = ItemInventory.Find(i => i == ToFind);

        if(ReturnItem != null)
        {
            return ReturnItem;
        }
        else
        {
            return null;
        }
    }

    public static void LoadNewInventory(List<ItemScriptableObject> NewInv)
    {
        ItemInventory = NewInv;
    }

    public static List<string> ConvertToStrArray()
    {
        List<string> ItemIntArray = new List<string>();

        if(ItemInventory.Count > 0)
        {
            foreach (ItemScriptableObject item in ItemInventory)
            {
                ItemIntArray.Add(item.ID);
            }
            return ItemIntArray;
        }
        else
        {
            return null;
        }
    }
}

public class Movement : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D Body;

    public InputAction Mover;
    [HideInInspector] public Vector2 NewMover;

    public InputAction Interact;

    public InputAction Run;

    public float runSpeed;
    public float HomeRunSpeed;

    public SpriteRenderer Rend;

    [HideInInspector]public Vector2 MousePos;

    public bool JumpState;

    public bool InAttack;

    public List<GameObject> ItemsInHand;

    public Transform Hand;

    public static Movement Instance { get; private set; }
    public bool InAction;

    public Detection NPC_Checker;

    public float DisableTimer = 3f;
    private float FixedTimer;

    public Shooting GunInHand;

    public bool InShopArea;

    public TypeOfDialogue TutorialDialogue;
    public TypeOfDialogue ShootingTutorial;
    public SpriteRenderer InteractObj;
    public bool ActionPrompt;

    public int Level;

    public PlayerInfo Info;

    public CinemachineVirtualCamera VCam;

    public ParticleSystem HealthSystem;
    public ParticleSystem AmmoSystem;

    [HideInInspector]public Animator Anim;

    public RenderPipelineAsset OriginalAsset;

    public Level LevelBefore;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        ScreenTrans.TransDone += Done;
    }

    void Done()
    {
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
        {
            if (Info.Level <= 0)
            {
                SceneType.Instance.GetComponent<Exit>().CancelTut = false;
            }
            if (Info.Level > 0)
            {
                SceneType.Instance.GetComponent<Exit>().CancelTut = true;
            }
        }
        LoadTut();
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            PortalHealth.PortalDeath += PortalHealth_PortalDeath;
        }

        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level || SceneType.Instance.ThisScene == SceneType.TypeOfScene.Boss)
        {
            GetComponent<Health>().enabled = true;
            GetComponent<ThowingKnife>().enabled = true;
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            gameObject.SetActive(true);
        }
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village || SceneType.Instance.ThisScene == SceneType.TypeOfScene.House)
        {
            GetComponent<Health>().enabled = false;
            GetComponent<ThowingKnife>().enabled = false;
            InteractObj = GameObject.Find("Interact").GetComponent<SpriteRenderer>();
        }
        Body = GetComponent<Rigidbody2D>();
        VCam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
        GetComponent<QueueingUp>().Speed = runSpeed;
        FixedTimer = DisableTimer;
        GraphicsSettings.renderPipelineAsset = OriginalAsset;
    }

    private void PortalHealth_PortalDeath()
    {
        VCam.m_Follow = PortalHealth.Instance.transform;
        VCam.m_Lens.OrthographicSize = 10f;
        PortalHealth.PortalDeath -= PortalHealth_PortalDeath;
    }

    public void LoadTut()
    {
        if (SceneType.Instance.GetComponent<Exit>().CancelTut)
            return;
        if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
            Dialogue.MakeNewBox(TutorialDialogue);
        else if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
            Dialogue.MakeNewBox(ShootingTutorial);
        ScreenTrans.TransDone -= Done;
    }

    void Start()
    {
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level)
        {
            PortalHealth.PortalDeath += PortalHealth_PortalDeath;
        }
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level || SceneType.Instance.ThisScene == SceneType.TypeOfScene.Boss)
        {
            GetComponent<Health>().enabled = true;
            GetComponent<ThowingKnife>().enabled = true;
        }
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village || SceneType.Instance.ThisScene == SceneType.TypeOfScene.House)
        {
            SoundManager.Play("Village");
            GetComponent<Health>().enabled = false;
            GetComponent<ThowingKnife>().enabled = false;
            InteractObj = GameObject.Find("Interact").GetComponent<SpriteRenderer>();
        }
        Body = GetComponent<Rigidbody2D>();
        VCam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
        GetComponent<QueueingUp>().Speed = runSpeed;
        FixedTimer = DisableTimer;
    }

    public void LeaveQueue(QueueHandeler Handle)
    {
        GetComponent<QueueingUp>().QueueDown(out InAction, Handle);
    }

    void Update()
    {
        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame)
        {
            Cursor.visible = !Cursor.visible;
        }

        if(SceneType.Instance.ThisScene == SceneType.TypeOfScene.Village)
        {
            InteractObj.enabled = ActionPrompt;
            InteractObj.GetComponent<PositionConstraint>().translationOffset = new Vector3(0f,(Mathf.Sin(Time.time * 5f) + 4f) * 0.4f);
            VCam.m_Lens.OrthographicSize = 8f;
        }
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.House)
        {
            InteractObj.enabled = ActionPrompt;
            InteractObj.GetComponent<PositionConstraint>().translationOffset = new Vector3(0f, (Mathf.Sin(Time.time * 5f) + 4f) * 0.4f);
            VCam.m_Lens.OrthographicSize = 5.6f;
        }
        if (SceneType.Instance.ThisScene == SceneType.TypeOfScene.Level )
        {
            if(Dialogue.Instance != null)
            {
                if (!Dialogue.Instance.Finished)
                {
                    Health.Instance.Invis = true;
                }
            }

        }


        NewMover.x = Mover.ReadValue<Vector2>().x;
        NewMover.y = Mover.ReadValue<Vector2>().y;

        if(NPC_Checker.enabled == false)
        {
            DisableTimer -= Time.deltaTime;
            NPC_Checker.Detected = false;
            if (DisableTimer <= 0)
            {
                NPC_Checker.enabled = true;
                DisableTimer = FixedTimer;
            }
        }

        if (NPC_Checker.Detected)
        {
            ActionPrompt = true;
            if (NPC_Checker.ObjectsInArea.CompareTag("Villager"))
            {
                if (Interact.triggered)
                {
                    if (!InAction)
                    {
                        if (!InShopArea)
                        {
                            if (!NPC_Checker.ObjectsInArea.GetComponent<NPCAI>().InShop)
                            {
                                if (NPC_Checker.ObjectsInArea.GetComponent<NPCAI>().ThisState == NPCAI.State.WantToSpeakToNPC)
                                    NPC_Checker.ObjectsInArea.GetComponent<NPCAI>().SwitchState(NPCAI.State.Speaking);
                            }
                        }

                    }
                }


            }
        }

        runSpeed = Mathf.Clamp(runSpeed, 7f, 15f);

        if (Keyboard.current.leftShiftKey.isPressed)
        {
            runSpeed += 0.25f;
        }
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            runSpeed -= 0.25f;
        }
        MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if(GetComponentInChildren<Shooting>() != null)
        {
            GetComponentInChildren<Shooting>().enabled = true;
        }
        if(ActionPrompt != true)
        {
            ActionPrompt = false;
        }

    }

    private void FixedUpdate()
    {
        Vector2 MsPos = (MousePos - (Vector2)transform.position).normalized;

        //Movement
        if (!InAction)
        {
            Body.MovePosition(Body.position + NewMover * runSpeed * 1.5f * Time.fixedDeltaTime);
            Anim.SetFloat("Horizontal", MsPos.x);
            Anim.SetFloat("Vertical", MsPos.y);
            Anim.SetFloat("Speed", NewMover.SqrMagnitude());

        }

        if (JumpState == true)
        { 
            CinemachineShake.Instance.ShakeCamera(5f, 2f);
        }
    }

    private void OnDisable()
    {
        InAttack = true;
        Mover.Disable();
        Interact.Disable();
    }

    private void OnEnable()
    {
        InAttack = false;
        Mover.Enable();
        Interact.Enable();
    }
}