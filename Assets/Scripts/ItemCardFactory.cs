using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ItemCardFactory
{
    private readonly IObjectResolver _container;

    public ItemCardFactory(IObjectResolver container)
    {
        _container = container;
    }

    public List<ItemCard> Create(List<ItemCard> itemPrefabs, Vector3 position)
    {
        var itemCardsList = new List<ItemCard>();
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