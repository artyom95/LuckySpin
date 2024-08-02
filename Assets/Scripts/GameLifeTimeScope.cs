using System.Collections.Generic;
using DefaultNamespace;
using GameStates;
using UniTaskPubSub;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using AnimationState = GameStates.AnimationState;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] private ScreenView _screenView;
    [SerializeField] private ButtonController _buttonController;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private List<ItemCard> _cards;
    [SerializeField] private List<ItemFinishCard> _finishards;

    protected override void Configure(IContainerBuilder builder)
    {
        base.Configure(builder);
        RegisterGameStateMachine(builder);
        builder.RegisterInstance(_screenView);
        builder.RegisterInstance(_buttonController);
        builder.RegisterInstance(_gameSettings);
        builder.RegisterInstance(_cards);
        builder.RegisterInstance(_finishards);
        
        builder.Register<ScreenPresenter>(Lifetime.Singleton);
        builder.Register<ScreenModel>(Lifetime.Singleton);
        builder.Register<ItemCard>(Lifetime.Singleton);
        builder.Register<ItemFinishCard>(Lifetime.Singleton);
      
        builder.Register<ItemFactory>(Lifetime.Singleton);
        builder.Register<ItemCardFactory>(Lifetime.Singleton);
        builder.Register<ItemFinishCardFactory>(Lifetime.Singleton);

        builder.Register<AttemptController>(Lifetime.Singleton);
        builder.Register<AnimationController>(Lifetime.Singleton);
        builder.Register<AchieveDetecter>(Lifetime.Singleton);

        builder.Register<CardInformationHandler>(Lifetime.Singleton);


        builder.Register<AsyncMessageBus>(Lifetime.Singleton)
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterEntryPoint<GameController>();
    }

    private void RegisterGameStateMachine(IContainerBuilder builder)
    {
        builder.Register<BootstrapState>(Lifetime.Singleton);
        builder.Register<ObserveState>(Lifetime.Singleton);
        builder.Register<AnimationState>(Lifetime.Singleton);
        builder.Register<ClientGameStateMachine>(Lifetime.Singleton);
        builder.Register<GameOverState>(Lifetime.Singleton);
    }
}