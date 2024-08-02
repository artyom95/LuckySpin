using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.Events;
using Enum;
using UniTaskPubSub;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// make deal with right finding ItemFinishCard (FindCard method)
/// fix issue with Item Collider
/// </summary>
public class ScreenModel
{
    public int Attempts { get; }

    private readonly AnimationController _animationController;
    private readonly AttemptController _attemptController;
    private IAsyncSubscriber _subscriber;
    private readonly AchieveDetecter _achieveDetector;
    private List<ItemCard> _cards;
    private readonly IAsyncPublisher _publisher;
    private readonly CardInformationHandler _cardInformationHandler;


    public ScreenModel(AttemptController attemptController,
        AnimationController animationController,
        AchieveDetecter achieveDetector,
        IAsyncSubscriber subscriber,
        IAsyncPublisher publisher,
        CardInformationHandler cardInformationHandler)
    {
        _cardInformationHandler = cardInformationHandler;
        _publisher = publisher;
        _achieveDetector = achieveDetector;
        _subscriber = subscriber;
        _attemptController = attemptController;
        _animationController = animationController;
        Attempts = attemptController.Attempts;
    }

    public void Initialize(GameObject arrow, List<ItemCard> cards, GameObject chest, GameObject rotateWheel)
    {
        _cards = cards;
        _achieveDetector.Initialize(arrow);
        _animationController.Initialize(chest, rotateWheel, OnAchieveDetectHandler);
    }

    public void ShowAnimationAttempts(GameObject movingObject)
    {
        _animationController.ShowAnimationAttempt(movingObject);
    }

    public int DecreaseAttempt()
    {
        _attemptController.DecreaseAttempts();
        return _attemptController.Attempts;
    }

    public bool IsItGameOver()
    {
        if (_attemptController.Attempts > 0)
        {
            return false;
        }

        return true;
    }

    public void ShowGameOverAnimation(List<ItemFinishCard> createdFinishCards)
    {
        InitializeFinishCards(createdFinishCards);

        _animationController.ShowGameOverAnimation(createdFinishCards);
    }

    public UnityAction GetSubscribingAnimation()
    {
        return _animationController.HideChestWithAchievements;
    }

    /// <summary>
    /// it should be personal class something like AmountAchieveManager which check
    /// attempt in any case and if it is not null this class have to add value to
    /// previous value, if it is null class could be replace null to value
    /// </summary>
    /// <param name="createdFinishCards"></param>
    private void InitializeFinishCards(List<ItemFinishCard> createdFinishCards)
    {
        foreach (var finishCard in createdFinishCards)
        {
            var amountRewarding = _cardInformationHandler.GetRewardInformation(finishCard);
            finishCard.SetAmountRewarding(amountRewarding);
        }
    }

    private void OnAchieveDetectHandler()
    {
        var currentItem = _achieveDetector.FindAchieve();

        Debug.Log(currentItem.Name);

        _publisher.PublishAsync(new BackGroundChangedEvent(true));
        var currentCard = FindCard(currentItem);

        ShowAnimation(currentCard);
    }

    private ItemCard FindCard(Item detectedItem)
    {
        ItemCard findingCard = null;
        foreach (var itemCard in _cards)
        {
            if (itemCard.Name.Equals(detectedItem.Name))
            {
                findingCard = itemCard;
            }
        }

        SetActive(findingCard);
        return findingCard;
    }

    private async UniTask ShowAnimation(ItemCard currentCard)
    {
        if (currentCard.Name.Equals(AchieveType.Skull.ToString()))
        {
            await _animationController.ShowSkullCardAnimation(currentCard);
        }
        else
        {
            await _animationController.ShowCardAnimation(currentCard);
        }

        await CollectCardInformation(currentCard);
    }

    private void SetActive(ItemCard currentCard)
    {
        currentCard.gameObject.SetActive(true);
    }

    private async UniTask CollectCardInformation(ItemCard currentCard)
    {
        _cardInformationHandler.AddCardInformation(currentCard);
    }
}