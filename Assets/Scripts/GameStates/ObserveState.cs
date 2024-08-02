using Common.Extensions;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Events;
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
        private readonly IAsyncPublisher _publisher;

        public ObserveState(
            IAsyncPublisher publisher,
            ScreenPresenter screenPresenter)
        {
            _publisher = publisher;
            _screenPresenter = screenPresenter;
            _screenPresenter.SubscribeSpinButton(OnMousePressed);
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
            _publisher.PublishAsync(new UnLockedSpinButtonEvent());
          
            return UniTask.CompletedTask;
        }

        private async void OnMousePressed()
        {
            Debug.Log("Spin Button was pressed");
            await _stateMachine.Enter<AnimationState>();
           await _publisher.PublishAsync(new LockedSpinButtonEvent());
           
        }
    }
}