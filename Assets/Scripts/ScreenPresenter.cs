using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Events;
using UniTaskPubSub;
using UnityEngine;
using UnityEngine.Events;

public class ScreenPresenter
{
    private readonly ScreenView _screenView;
    private readonly ButtonController _buttonController;
    private readonly ScreenModel _screenModel;
    private readonly GameSettings _gameSettings;
    private readonly ItemFactory _itemFactory;
    private readonly IAsyncSubscriber _subscriber;
    private readonly List<ItemCard> _cards;
    private readonly ItemCardFactory _itemCardFactory;
    private readonly IAsyncPublisher _publisher;
    private readonly List<ItemFinishCard> _finishCards;
   private List<ItemFinishCard> _createdFinishCards;
    private readonly ItemFinishCardFactory _itemFinishCardFactory;


    public ScreenPresenter(ScreenModel screenModel,
        ScreenView screenView,
        ButtonController buttonController,
        GameSettings gameSettings,
        ItemFactory itemFactory,
        IAsyncSubscriber subscriber,
        List<ItemCard> cards,
        List<ItemFinishCard> finishCards,
        ItemCardFactory itemCardFactory, 
        IAsyncPublisher publisher,
        ItemFinishCardFactory itemFinishCardFactory)
    {
        _itemFinishCardFactory = itemFinishCardFactory;
        _finishCards = finishCards;
        _publisher = publisher;
        _itemCardFactory = itemCardFactory;
        _cards = cards;
        _subscriber = subscriber;
        _itemFactory = itemFactory;
        _gameSettings = gameSettings;
        _screenModel = screenModel;
        _buttonController = buttonController;
        _screenView = screenView;
    }


    public void Initialize()
    {
        var itemList = InitializeItems(out var arrow);
        _subscriber.Subscribe<BackGroundChangedEvent>(OnBackGroundChangedHandler);
        _screenView.Initialize(itemList, _screenModel.Attempts);
        
        var cardList = _itemCardFactory.Create(_cards, _gameSettings.LastMovingPosition);
       
        var chest = _screenView.Chest;
        var rotateWheel= _screenView.RotateWheel;

        _createdFinishCards = _itemFinishCardFactory.Create(_finishCards, _gameSettings.LastMovingPosition);
        
        _screenModel.Initialize(arrow, cardList, chest, rotateWheel);
    }


    private List<Item> InitializeItems(out GameObject arrow)
    {
        var prefab = _gameSettings.ItemPrefab;
        var itemList = new List<Item>();
        arrow = _screenView.Arrow;
      
        foreach (var sprite in _gameSettings.Sprites)
        {
            var item = _itemFactory.Create(prefab);
            item.Initialize(sprite);
            itemList.Add(item);
        }

        return itemList;
    }

    public void SubscribeSpinButton(UnityAction subscribeAction)
    {
        _buttonController.SubscribeSpinButton(subscribeAction);
    }

    public void SubscribeCalmButton()
    {
        var subscribeAction = _screenModel.GetSubscribingAnimation();
        _buttonController.SubscribeCalmButton(subscribeAction);
    }
    public void ShowAnimationAttempts()
    {
        Debug.Log("ShowAnimationAttempts is called");
        var attemptImage = _screenView.AttemptImage;
      
        _screenModel.ShowAnimationAttempts(attemptImage);

        var attempts = _screenModel.DecreaseAttempt();
        _screenView.SetAttempts(attempts);
    }
/// <summary>
/// It necessary improve the CheckGameOverState
/// must check is it GameOverState (need to calling method in ScreenModel or in GameOverController thinking about it
/// need to initialize GameOver screen with GameOver achieve, collected information
/// need to create gameOver achieve fabric where I will be able create gameOver achieve
/// need to collect (especially in dictionary where I have  a key - name achieve, and value - quantity achieve) information about approve achievements
/// if it is not GameOver have to publish GameContinued Event
/// </summary>
    public void CheckGameOver()
    {
        if (!_screenModel.IsItGameOver())
        {
            _publisher.PublishAsync(new GameContinuedEvent());
        }
        else
        {
            _screenModel.ShowGameOverAnimation(_createdFinishCards);
        }
        
    }
    private void OnBackGroundChangedHandler(BackGroundChangedEvent eventData)
    {
        _screenView.ChangeBackGround(eventData.ShouldChangeBackGround);
    }
}