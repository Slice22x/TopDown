using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class StoryMovement : MonoBehaviour
{
    public static StoryMovement Instance;

    public InputAction Mover;
    public InputAction Interact;

    Vector2 NewMover;
    Rigidbody2D Body;
    public float runSpeed;

    Detection Detect;

    public GameObject InteractObj;

    public bool InAction;

    [SerializeField] TypeOfDialogue Intro;
    [SerializeField] TypeOfDialogue EnterKitchen;

    public PlayableDirector Cutscene;

    public PlayerInfo Info;

    public Animator Anim;

    void Awake()
    {
        Instance = this;
        Body = GetComponent<Rigidbody2D>();
        Mover.Enable();
        Interact.Enable();
        Detect = GetComponent<Detection>();
        SoundManager.Play("Night_Theme");
    }

    private void Start()
    {
        ScreenTrans.TransDone += ScreenTrans_TransDone;
        Triggers.Triggered += Triggers_Triggered;
    }

    private void Triggers_Triggered(string Name)
    {
        if(Name == "Kitchen")
        {
            SoundManager.Stop("Night_Theme");
            Dialogue.MakeNewBox(EnterKitchen, gameObject);
        }
    }

    private void ScreenTrans_TransDone()
    {
        Dialogue.MakeNewBox(Intro);
        ScreenTrans.TransDone -= ScreenTrans_TransDone;
    }

    void FixedUpdate()
    {
        NewMover.x = Mover.ReadValue<Vector2>().x;
        NewMover.y = Mover.ReadValue<Vector2>().y;
        if (!InAction)
        {
            Body.MovePosition(Body.position + NewMover * runSpeed * 1.5f * Time.fixedDeltaTime);
            Anim.SetFloat("Horizontal", NewMover.x);
            Anim.SetFloat("Vertical", NewMover.y);
            Anim.SetFloat("Speed", NewMover.SqrMagnitude());
        }


        InteractObj.SetActive(Detect.Detected);

        if (Detect.Detected)
        {
            InteractObj.GetComponent<PositionConstraint>().translationOffset = new Vector3(0f, (Mathf.Sin(Time.time * 5f) + 4f) * 0.4f);
            if (Interact.triggered && Dialogue.Instance.Finished)
            {
                Detect.ObjectsInArea.GetComponent<FurInteractions>().Interact();
            }
        }

        if(GameObject.Find("Fridge").GetComponent<FurInteractions>().Interacted && Dialogue.Instance.Finished)
        {
            Cutscene.Play();
        }
    }

    public void PlayParticle()
    {
        SoundManager.StopAll();
        GameObject.Find("Fridge").GetComponent<ParticleSystem>().Play();
    }

    public void LoadVillage()
    {
        ScreenTrans.CallLevel("Village");
        Info.HasSeenStory = true;
        ScreenTrans.TransDone -= ScreenTrans_TransDone;
    }
}
