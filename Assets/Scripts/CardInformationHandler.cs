using System;
using System.Collections.Generic;

public class CardInformationHandler
{
    private readonly Dictionary<string, string> _cardsInformation;

    public CardInformationHandler()
    {
        _cardsInformation = new();
    }

    public void AddCardInformation(ItemCard currentCard)
    {
        if (IsCardInformation(currentCard))
        {
            _cardsInformation.Remove(currentCard.Name);

            var currentReward = CountNewAmountRewarding(currentCard);
            _cardsInformation.Add(currentCard.Name, currentReward.ToString());
        }
        else
        {
            _cardsInformation.Add(currentCard.Name, currentCard.AmountRewarding.text.ToString());
        }
    }

    public string GetRewardInformation(ItemFinishCard card)
    {
        if (_cardsInformation.ContainsKey(card.Name))
        {
            return _cardsInformation[card.Name];
        }

        return default;
    }

    private bool IsCardInformation(ItemCard currentCard)
    {
        if (_cardsInformation.ContainsKey(currentCard.Name))
        {
            return true;
        }

        return false;
    }

    private int CountNewAmountRewarding(ItemCard currentCard)
    {
        var previousReward = Convert.ToInt32(_cardsInformation[currentCard.Name]);
        var currentReward = Convert.ToInt32(currentCard.AmountRewarding.textInfo.materialCount);
        return previousReward + currentReward;
    }
}