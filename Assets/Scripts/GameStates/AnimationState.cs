using System.Threading.Tasks;
using Common.Extensions;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
using UniTaskPubSub;
using IState = StateMachine.IState;

namespace GameStates
{
    public class AnimationState: IState
    {
        private StateMachine.StateMachine _stateMachine;
        private readonly ScreenPresenter _screenPresenter;
        private readonly IAsyncSubscriber _subscriber;
        private CompositeDisposable _subscriptions;

        public AnimationState(ScreenPresenter screenPresenter, IAsyncSubscriber subscriber)
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
                _subscriber.Subscribe<AnimationCompletedEvent>(_ => OnAnimationCompletedHandler())
            };
            _screenPresenter.ShowAnimationAttempts();
            return UniTask.CompletedTask;
        }

        private async UniTask OnAnimationCompletedHandler()
        {
            await _stateMachine.Enter<GameOverState>();
        }
    }
}