using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance { get; private set; }

    [SerializeField] private PlayerMovement2D playerMovement;

    private IPlayerState _currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (!playerMovement)
            playerMovement = GetComponent<PlayerMovement2D>();
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

    public PlayerMovement2D Player => playerMovement;
}