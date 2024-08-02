using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ItemFinishCardFactory
{
    private readonly IObjectResolver _container;

    public ItemFinishCardFactory(IObjectResolver container)
    {
        _container = container;
    }

    public List<ItemFinishCard> Create(List<ItemFinishCard> itemPrefabs, Vector3 position)
    {
        var itemCardsList = new List<ItemFinishCard>();
        foreach (var itemPrefab in itemPrefabs)
        {
            var itemCard = _container.Instantiate(itemPrefab);
            itemCard.SetName();
            itemCard.gameObject.transform.position = position;
            itemCard.gameObject.SetActive(false);
            itemCardsList.Add(itemCard);
        }

        return itemCardsList;
    }
}