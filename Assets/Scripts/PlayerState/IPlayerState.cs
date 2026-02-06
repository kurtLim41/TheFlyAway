public interface IPlayerState
{
    //defined state interface 
    void Enter(PlayerStateMachine ctx);
    void Exit(PlayerStateMachine ctx);
    void Update(PlayerStateMachine ctx, float deltaTime);
}