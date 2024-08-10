using Common.Extensions;
using Cysharp.Threading.Tasks;
using Events;
using StateMachine;
using UniTaskPubSub;
using UnityEngine;

namespace GameStates
{
    public class ObserveState : IState
    {
        private StateMachine.StateMachine _stateMachine;
        private readonly ScreenPresenter _screenPresenter;
        private CompositeDisposable _subscriptions;
        private readonly AsyncMessageBus _messageBus;

        public ObserveState(
            AsyncMessageBus messageBus,
            ScreenPresenter screenPresenter)
        {
            _messageBus = messageBus;
            _screenPresenter = screenPresenter;
            _screenPresenter.SubscribeSpinButton(OnSpinButtonPressed);
            _screenPresenter.SubscribeCalmButton();
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
            _messageBus.Publish(new UnLockedSpinButtonEvent());
            return UniTask.CompletedTask;
        }

        private async void OnSpinButtonPressed()
        {
            _messageBus.Publish(new LockedSpinButtonEvent());
            await _stateMachine.Enter<AnimationState>();
        }
    }
}