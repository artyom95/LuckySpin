using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Events;
using JetBrains.Annotations;
using UniTaskPubSub;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AnimationController
{
    private readonly GameSettings _gameSettings;
    private readonly IAsyncPublisher _publisher;
    private Action _onAchieveDetectedHandler;
    private GameObject _chest;
    private GameObject _rotateWheel;
    private List<ItemFinishCard> _createdFinishCards;
    private Action _publishEvent;

    public AnimationController(GameSettings gameSettings, IAsyncPublisher publisher)
    {
        _publisher = publisher;
        _gameSettings = gameSettings;
    }

    public void Initialize(GameObject chest, GameObject rotateWheel, Action onAchieveDetectedHandler)
    {
        _rotateWheel = rotateWheel;
        _chest = chest;
        _onAchieveDetectedHandler = onAchieveDetectedHandler;
    }

    public void ShowAnimationAttempt(GameObject movingObject)
    {
        var lastPosition = _gameSettings.LastMovingPosition;
        var jumpPower = _gameSettings.JumpPower;
        var numberJumps = _gameSettings.NumberJumps;
        var jumpDuration = _gameSettings.MoveDuration;
        var scaleDuration = _gameSettings.ScaleAttemptDuration;
        var scale = movingObject.gameObject.transform.localScale;

        var copyMovingObject = Object.Instantiate(movingObject, movingObject.transform.position,
            movingObject.transform.rotation, movingObject.transform.parent);
        copyMovingObject.transform.localScale = Vector3.zero;

        var sequence = DOTween.Sequence();


        sequence.Append(copyMovingObject.transform.DOScale(scale, scaleDuration));

        sequence.Append(copyMovingObject.transform.DOJump(lastPosition, jumpPower, numberJumps, jumpDuration));

        sequence.AppendCallback(RotateWheel);
        sequence.AppendCallback(() => Object.DestroyImmediate(copyMovingObject));
    }

    public async UniTask ShowSkullCardAnimation([NotNull] ItemCard card)
    {
        PrepareAnimationCard(card, out var currentScale, out var finishScale);


        var sequence = DOTween.Sequence();

       sequence.Append(card.transform.DOScale(currentScale, _gameSettings.ScaleDuration));
        sequence.Append(card.transform.DOShakePosition(_gameSettings.ShakeDuration));
        sequence.AppendInterval(_gameSettings.Delay);

        sequence.Append(card.transform.DOScale(finishScale, _gameSettings.ScaleDuration));
        sequence.AppendCallback(() => card.gameObject.SetActive(false));
        sequence.Append(card.transform.DOScale(currentScale, _gameSettings.ScaleDuration));

         sequence.AppendCallback(() => _publisher.PublishAsync(new BackGroundChangedEvent(false)));
    }

    public void ShowGameOverAnimation(List<ItemFinishCard> createdFinishCards)
    {
        ShowFinishChestAnimation(createdFinishCards);
    }

    public async UniTask ShowCardAnimation([NotNull] ItemCard card)
    {
        PrepareAnimationCard(card, out var currentScale, out var finishScale);
        var firstPosition = card.transform.position;
        var sequence = DOTween.Sequence();


        sequence.Join(card.transform.DOScale(currentScale, _gameSettings.ScaleDuration))
            .Join(card.transform.DORotate(_gameSettings.RotateAngle, _gameSettings.RotateDuration,
                RotateMode.FastBeyond360));

        sequence.AppendInterval(_gameSettings.Delay)
            .Append(card.transform.DOMoveX(_gameSettings.ChestХPosition, _gameSettings.MoveDuration))
            .AppendInterval(_gameSettings.Delay)
           
            .Append(card.transform.DOScale(finishScale, _gameSettings.ScaleDuration))
            .AppendCallback(() => _publisher.PublishAsync(new BackGroundChangedEvent(false)))
          .AppendCallback(ScaleChest)
            .AppendCallback(() => card.gameObject.SetActive(false))
            .Append(card.transform.DOMoveX(firstPosition.x, _gameSettings.MoveDuration))
            .Append(card.transform.DOScale(currentScale, _gameSettings.ScaleDuration));
    }

    public void HideChestWithAchievements()
    {
        foreach (var createdFinishCard in _createdFinishCards)
        {
            createdFinishCard.gameObject.SetActive(false);
        }

        _chest.SetActive(false);
        _publisher.PublishAsync(new BackGroundChangedEvent(false));
    }

    private void ShowFinishChestAnimation(List<ItemFinishCard> createdFinishCards)
    {
        _createdFinishCards = createdFinishCards;
        _publisher.PublishAsync(new BackGroundChangedEvent(true));
        var sequence = DOTween.Sequence();
        sequence.Append(_chest.transform.DOMoveX(_gameSettings.LastMovingPosition.x, _gameSettings.MoveDuration))
            .Append(_chest.transform.DOShakePosition(_gameSettings.ShakeDuration, 2f, 40))
            .AppendCallback(() => TurnOnFinishCardAnimation());
    }

    private async UniTask TurnOnFinishCardAnimation()
    {
        var sequenceNumber = 0;
        foreach (var finishCard in _createdFinishCards)
        {
            await ShowFinishCardAnimation(finishCard, sequenceNumber);
            sequenceNumber++;
        }

        _publisher.Publish(new TurnOnCalmButtonEvent());
    }

    private async UniTask ShowFinishCardAnimation(ItemFinishCard itemFinishCard, int sequenceNumber)
    {
        var movingPosition = new Vector3();
        var itemFinishCardPosition = _createdFinishCards.First().transform.position;
        switch (sequenceNumber)
        {
            case 0:
                movingPosition = new Vector3(itemFinishCardPosition.x - GlobalConstant.DELTA_X_Y_COORDINATE,
                    itemFinishCardPosition.y, itemFinishCardPosition.z);
                break;
            case 1:
                movingPosition = new Vector3(itemFinishCardPosition.x - GlobalConstant.DELTA_X_Y_COORDINATE,
                    itemFinishCardPosition.y + GlobalConstant.DELTA_X_Y_COORDINATE, itemFinishCardPosition.z);
                break;
            case 2:
                movingPosition = new Vector3(itemFinishCardPosition.x + GlobalConstant.DELTA_X_Y_COORDINATE,
                    itemFinishCardPosition.y, itemFinishCardPosition.z);
                break;
            case 3:
                movingPosition = new Vector3(itemFinishCardPosition.x + GlobalConstant.DELTA_X_Y_COORDINATE,
                    itemFinishCardPosition.y + GlobalConstant.DELTA_X_Y_COORDINATE, itemFinishCardPosition.z);
                break;
        }

        itemFinishCard.gameObject.SetActive(true);
        itemFinishCard.transform.DOMove(movingPosition, _gameSettings.MoveDuration);
    }

    private void PrepareAnimationCard(ItemCard card, out Vector3 currentScale, out Vector3 finishScale)
    {
        var transform = card.transform;
        currentScale = transform.localScale;
        finishScale = transform.localScale = Vector3.zero;
        card.gameObject.transform.localScale = finishScale;
    }

    private void ScaleChest()
    {
        var currentScale = _chest.transform.localScale;
        var sequence = DOTween.Sequence();
        sequence.WaitForCompletion();
        sequence.Append(_chest.transform.DOScale(currentScale * _gameSettings.ChestScale, _gameSettings.ScaleChestDuration))
            .Append(_chest.transform.DOScale(currentScale, _gameSettings.ScaleChestDuration))
            .AppendCallback(() => _publisher.PublishAsync(new AnimationCompletedEvent()));
    }

    private void InvokeDetectedEvent()
    {
        _onAchieveDetectedHandler?.Invoke();
    }

    private void RotateWheel()
    {
        var speedRotation = Random.Range(_gameSettings.MinimalSpeedRotation, _gameSettings.MaximalSpeedRotation);
        var angleRotation = Random.Range(_gameSettings.MinimalAngleRotation, _gameSettings.MaximalAngleRotation);
        var vectorRotation = new Vector3(0, 0, angleRotation);
        var sequence = DOTween.Sequence();

        sequence.Append(_rotateWheel.transform.DORotate(vectorRotation, speedRotation));
        sequence.AppendCallback(InvokeDetectedEvent);
    }
}