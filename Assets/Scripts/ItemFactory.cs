using VContainer;
using VContainer.Unity;

public class ItemFactory
{
    private readonly IObjectResolver _container;

    public ItemFactory(IObjectResolver container)
    {
        _container = container;
    }

    public Item Create(Item itemPrefab)
    {
        var item = _container.Instantiate(itemPrefab);
        return item;
    }
}