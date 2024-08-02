using Common.Extensions;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using StateMachine;
using UniTaskPubSub;

namespace GameStates
{
    public class GameOverState : IState
    {
        private StateMachine.StateMachine _stateMachine;
        private ScreenPresenter _screenPresenter;
        private IAsyncSubscriber _subscriber;
        private CompositeDisposable _subscriptions;

        public GameOverState(ScreenPresenter screenPresenter,  IAsyncSubscriber subscriber)
        {
            _subscriber = subscriber;
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
            _subscriptions = new CompositeDisposable()
            {
                _subscriber.Subscribe<GameContinuedEvent>(_ => OnGameContinuedHandler())
            };
            _screenPresenter.CheckGameOver();
            return UniTask.CompletedTask;
        }

        private async UniTask OnGameContinuedHandler()
        {
            await _stateMachine.Enter<ObserveState>();
        }
    }
}