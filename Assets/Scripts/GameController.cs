using System;
using GameStates;
using UniTaskPubSub;
using VContainer.Unity;

public class GameController : IStartable, ITickable, IDisposable
{
    private readonly ClientGameStateMachine _gameStateMachine;
    private AsyncMessageBus _messageBus;

    public GameController(ClientGameStateMachine gameStateMachine,
        AsyncMessageBus messageBus)
    {
        _messageBus = messageBus;
        _gameStateMachine = gameStateMachine;
    }

    public async void Start()
    {
        await _gameStateMachine.Enter<BootstrapState>();
    }
    
    public void Tick()
    {
    }

    public void Dispose()
    {
        _gameStateMachine?.Dispose();
    }
}