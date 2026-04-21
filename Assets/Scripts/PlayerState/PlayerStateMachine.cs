using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private PlayerMovement2D playerMovement;
    public string LayerName;

    private IPlayerState _currentState;

    private void Awake()
    {
        // No singleton logic — allow multiple players
        if (!playerMovement)
            playerMovement = GetComponent<PlayerMovement2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeState(new NormalState());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ChangeState(new NormalState());
    }

    private void Update()
    {
        _currentState?.Update(this, Time.deltaTime);
    }

    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    public void OnDestroy()
    {
        _currentState?.Exit(this);
    }

    public PlayerMovement2D Player => playerMovement;
}