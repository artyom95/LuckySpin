using Cysharp.Threading.Tasks;
using IState = StateMachine.IState;


namespace GameStates
{
    public class BootstrapState : IState
    {
        private StateMachine.StateMachine _stateMachine;
        private readonly ScreenPresenter _screenPresenter;

        public BootstrapState(ScreenPresenter screenPresenter)
        {
            _screenPresenter = screenPresenter;
        }
        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Exit()
        {
            return UniTask.CompletedTask;
        }

        public UniTask Enter()
        {
            _screenPresenter.Initialize();
            return _stateMachine.Enter<ObserveState>();
        }
    }
}