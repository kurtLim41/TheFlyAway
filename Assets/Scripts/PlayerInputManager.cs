using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    
    public TitleScreenUI titleScreenUI;
    
    private bool wasdJoined = false;
    private bool arrowsJoined = false;
    private bool player1Joined = false;

    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "TitleScreen")
        {
            return;
        }
        
        if (Keyboard.current == null) return;

        if (!wasdJoined && Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (!player1Joined)
            {
                player1Joined = true;
                var player = PlayerInput.Instantiate(player1Prefab,
                    controlScheme: "WASD", pairWithDevice: Keyboard.current);
                titleScreenUI.ShowText(1);
            }
            else
            {
                var player = PlayerInput.Instantiate(player2Prefab,
                    controlScheme: "WASD", pairWithDevice: Keyboard.current);
                titleScreenUI.ShowText(2);
            }
            wasdJoined = true;
        }
        
        if (!arrowsJoined && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (!player1Joined)
            {
                player1Joined = true;
                var player = PlayerInput.Instantiate(player1Prefab,
                    controlScheme: "Arrows", pairWithDevice: Keyboard.current);
                titleScreenUI.ShowText(1);
            }
            else
            {
                var player = PlayerInput.Instantiate(player2Prefab,
                    controlScheme: "Arrows", pairWithDevice: Keyboard.current);
                titleScreenUI.ShowText(2);
            }
            arrowsJoined = true;
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamepad))
            {
                if (!player1Joined)
                {
                    player1Joined = true;
                    joinedGamepads.Add(gamepad);
                    var player = PlayerInput.Instantiate(player1Prefab,
                        controlScheme: "Gamepad", pairWithDevice: gamepad);
                    titleScreenUI.ShowText(1);
                }
                else
                {
                    joinedGamepads.Add(gamepad);
                    var player = PlayerInput.Instantiate(player2Prefab,
                        controlScheme: "Gamepad", pairWithDevice: gamepad);
                    titleScreenUI.ShowText(2);
                }
            }
        }
    }
}
