using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool ShowConsole;

    public static DebugCommand START_GAME;
    public static DebugCommand END_GAME;
    public static DebugCommand KILL_ENEMIES;
    public static DebugCommand<int> SPAWN_NUMBER_OF_ENEMIES;
    public static DebugCommand<string> SPAWN_ENEMY;
    public static DebugCommand<string> MOVE_TO_SCENE;

    public List<object> CommandList;

    string ConsoleInput;

    private void Awake()
    {
        START_GAME = new DebugCommand("start_game", "Starts the game", "START_GAME", () => 
        {
            PortalHealth.Instance.enabled = true;
            PortalHealth.Instance.StartGame();             
        });
        END_GAME = new DebugCommand("end_game", "end the game", "END_GAME", () => 
        {   PortalHealth.Instance.EndGame();
            PortalHealth.Instance.enabled = false;
        });
        KILL_ENEMIES = new DebugCommand("kill_enemies", "kills all enemies alive", "KILL_ENEMIES", () => 
        { 
            FindObjectOfType<AmountOfEnemies>().DestroyAllEnemies();        
        });
        SPAWN_NUMBER_OF_ENEMIES = new DebugCommand<int>("spawn_number", "spawns a number of enemies", "SPAWN_NUMBER", (x) =>
        {
            PortalHealth.Instance.Spawner.NumberOfEnemies = x;
            StartCoroutine(PortalHealth.Instance.Spawner.SpawnEnemy());
        });
        SPAWN_ENEMY = new DebugCommand<string>("spawn_enemy", "spawn a specific enemy", "SPAWN_ENEMY", (n) =>
         {
             PortalHealth.Instance.Spawner.SpawnEnemy(n);            
         });
        MOVE_TO_SCENE = new DebugCommand<string>("move_scene", "move to a specific scene", "MOVE_TO", (s) =>
         {
             Camera.main.GetComponent<Exit>().LoadScene(s);
         });

        CommandList = new List<object>
        {
            START_GAME,
            END_GAME,
            KILL_ENEMIES,
            SPAWN_NUMBER_OF_ENEMIES,
            SPAWN_ENEMY,
            MOVE_TO_SCENE
        };
    }

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ShowConsole = !ShowConsole;
            Movement.Instance.InAction = !Movement.Instance.InAction;
        }
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            if (ShowConsole)
            {
                HandleInput();
                ConsoleInput = "";
            }
        }
    }

    private void OnGUI()
    {
        if(!ShowConsole) { return; }

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 70), "");
        GUI.backgroundColor = new Color(0, 0, 0, 255);
        ConsoleInput = GUI.TextArea(new Rect(10f, y + 70f, Screen.width - 20f, 30f), ConsoleInput);
    }

    public void HandleInput()
    {
        string[] Props = ConsoleInput.Split(' ');

        for (int i = 0; i < CommandList.Count; i++)
        {
            DebugCommandBase CommandBase = CommandList[i] as DebugCommandBase;

            if (ConsoleInput.Contains(CommandBase.CommandID))
            {
                if(CommandList[i] as DebugCommand != null)
                {
                    (CommandList[i] as DebugCommand).Invoke();
                }
                else if(CommandList[i] as DebugCommand<int> != null)
                {
                    (CommandList[i] as DebugCommand<int>).Invoke(int.Parse(Props[1]));
                }
                else if(CommandList[i] as DebugCommand<string> != null)
                {
                    (CommandList[i] as DebugCommand<string>).Invoke(Props[1]);
                }
            }
        }
    }
}
