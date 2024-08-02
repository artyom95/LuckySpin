using Common.Extensions;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using Events;
using UniTaskPubSub;
using IState = StateMachine.IState;


namespace GameStates
{
    public class BootstrapState : IState
    {
        private StateMachine.StateMachine _stateMachine;
        private readonly ScreenPresenter _screenPresenter;
        private readonly ButtonController _buttonController;
        private CompositeDisposable _subscriptions;
        private readonly IAsyncSubscriber _subscriber;

        public BootstrapState(ScreenPresenter screenPresenter, ButtonController buttonController,
            IAsyncSubscriber subscriber)
        {
            _subscriber = subscriber;
            _buttonController = buttonController;
            _screenPresenter = screenPresenter;
        }
        public void Initialize(StateMachine.StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public UniTask Exit()
        {
            _subscriptions?.Dispose();
            return UniTask.CompletedTask;
        }

        public UniTask Enter()
        {
            _screenPresenter.Initialize();
            _subscriptions = new CompositeDisposable()
            {
                _subscriber.Subscribe<LockedSpinButtonEvent>(_=> _buttonController.LockSpinButton()),
                _subscriber.Subscribe<UnLockedSpinButtonEvent>(_=> _buttonController.UnLockSpinButton()),
                _subscriber.Subscribe<TurnOffCalmButtonEvent>( _=> OnTurnOffButtonHandler())
            };
            
            return _stateMachine.Enter<ObserveState>();
        }

        private async UniTask OnTurnOffButtonHandler()
        {
            _buttonController.TurnOffCalmButton();
        }
    }
}