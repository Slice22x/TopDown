using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [System.Serializable]
    public struct Node
    {
        public int LevelNumber;
        public string LevelName;
        public bool Locked;
        public Transform NextConnector;
        public Transform BackConnector;

    }
    public Node ThisNode;
    public Image LockImage;

    // Start is called before the first frame update
    void Start()
    {   

    }

    // Update is called once per frame
    void Update()
    {
        if (Movement.Instance.Info.Level >= ThisNode.LevelNumber)
        {
            ThisNode.Locked = false;
        }
        if (Movement.Instance.Info.Level < ThisNode.LevelNumber)
        {
            ThisNode.Locked = true;
        }

        if (!ThisNode.Locked)
        {
            LockImage.enabled = false;
        }
        if (ThisNode.Locked)
        {
            LockImage.enabled = true;
        }
    }
}
