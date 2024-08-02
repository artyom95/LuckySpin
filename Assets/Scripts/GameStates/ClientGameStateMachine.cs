namespace GameStates
{
    public class ClientGameStateMachine : StateMachine.StateMachine
    {
        public ClientGameStateMachine(
            BootstrapState bootstrapState,
            ObserveState observeMapState,
          AnimationState animationState,
            GameOverState gameOverState)
            : base(bootstrapState, observeMapState, animationState, gameOverState)
        {
        }
    }
}