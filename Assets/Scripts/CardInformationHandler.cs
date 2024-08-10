using System;
using System.Collections.Generic;
using Enum;

public class CardInformationHandler
{
    private readonly Dictionary<string, string> _cardsInformation;

    public CardInformationHandler()
    {
        _cardsInformation = new();
    }

    public void AddCardInformation(ItemCard currentCard)
    {
        if (currentCard.Name.Equals(AchieveType.Skull.ToString()))
        {
            return;
        }

        if (IsCardInformation(currentCard))
        {
            var currentReward = CountNewAmountRewarding(currentCard);
            _cardsInformation[currentCard.Name] = currentReward.ToString();
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

        return GlobalConstant.DEFAULT_AMOUNT_REWARDS;
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