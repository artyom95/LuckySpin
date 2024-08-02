using DefaultNamespace;
using UnityEngine;


public class Item : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer Icon { get; private set; }
    public string Name { get; private set; }

    public void Initialize(SpriteRenderer icon)
    {
        Icon.sprite = icon.sprite;
        var size = Icon.size;
        size.x = GlobalConstant.X_SIZE_SPRITE;
        size.y = GlobalConstant.Y_SIZE_SPRITE;
        Icon.size = size;
        Name = icon.gameObject.name;
    }
}