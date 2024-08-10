using System;
using Common.Extensions;
using Events;
using GameStates;
using UniTaskPubSub;
using VContainer.Unity;

public class GameController : IStartable, ITickable, IDisposable
{
    private readonly ClientGameStateMachine _gameStateMachine;
    private readonly ButtonController _buttonController;
    private readonly IAsyncSubscriber _subscriber;
    private CompositeDisposable _subscriptions;

    public GameController(ClientGameStateMachine gameStateMachine,
        ButtonController buttonController,
        IAsyncSubscriber subscriber)
    {
        _subscriber = subscriber;
        _buttonController = buttonController;
        _gameStateMachine = gameStateMachine;
    }

    public async void Start()
    {
        _subscriptions = new CompositeDisposable()
        {
            _subscriber.Subscribe<LockedSpinButtonEvent>(_=> _buttonController.LockSpinButton()),
            _subscriber.Subscribe<UnLockedSpinButtonEvent>(_=> _buttonController.UnLockSpinButton()),
            _subscriber.Subscribe<TurnOnCalmButtonEvent>( _=> OnTurnOnButtonHandler())
        };
        await _gameStateMachine.Enter<BootstrapState>();
    }
    
    public void Tick()
    {
    }

    public void Dispose()
    {
        _subscriptions.Dispose();
        _gameStateMachine?.Dispose();
    }
    private void OnTurnOnButtonHandler()
    {
      _buttonController.TurnOnCalmButton();
    }
}