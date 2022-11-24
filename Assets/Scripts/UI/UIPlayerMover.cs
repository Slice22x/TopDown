using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class UIPlayerMover : MonoBehaviour
{
    public Transform Player;
    public List<LevelNode> Nodes;
    [Range(0f, 1f)]
    public float MovementValue;
    public LevelNode CurrentNode;
    public LevelNode NextNode;
    bool Moving;
    bool Loading;
    bool Backwards;

    public static GameObject Instance;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = transform.parent.gameObject;
            Close();
        }
    }

    private void Update()
    {
        if (!Loading)
        {
            Movement.Instance.InAction = true;
        }
        if (Loading)
        {
            Movement.Instance.InAction = false;
        }

        if (Moving)
        {
            if (!Backwards)
            {
                MovementValue += Time.smoothDeltaTime * CurrentNode.ThisNode.NextConnector.GetComponent<Connecter>().Speed;
            }
            if (Backwards)
            {
                MovementValue += Time.smoothDeltaTime * CurrentNode.ThisNode.BackConnector.GetComponent<Connecter>().Speed;
            }
            if (MovementValue < 1f)
            {
                Player.position = Vector3.Lerp(CurrentNode.transform.position, NextNode.transform.position, MovementValue);
            }
            if (MovementValue >= 1f)
            {
                Moving = false;
                CurrentNode = NextNode;
            }
        }

        if (!Moving)
        {
            MovementValue = 0f;
        }

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && !Moving)
        {
            if(CurrentNode.ThisNode.NextConnector != null)
            {
                if(!Nodes[Nodes.IndexOf(CurrentNode) + 1].ThisNode.Locked)
                {
                    NextNode = Nodes[Nodes.IndexOf(CurrentNode) + 1];
                    Moving = true;
                    Backwards = false;
                }
            }
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame && !Moving)
        {
            if (CurrentNode.ThisNode.BackConnector != null)
            {
                NextNode = Nodes[Nodes.IndexOf(CurrentNode) - 1];
                Moving = true;
                Backwards = true;
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (CurrentNode.ThisNode.LevelName != null)
            {
                ScreenTrans.CallLevel(CurrentNode.ThisNode.LevelName);
                Movement.Instance.InAction = true;
                Loading = true;
            }
        }

    }

    public void Close()
    {
        Movement.Instance.InAction = false;
        transform.parent.gameObject.SetActive(false);
    }
}
