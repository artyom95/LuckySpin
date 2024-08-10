using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField] public UnityEngine.UI.Image Icon { get; private set; }
    public string Name { get; private set; }

    public void Initialize(UnityEngine.UI.Image icon)
    {
        Icon.sprite = icon.sprite;
        var size = Icon.sprite.rect.size;
        size.x = GlobalConstant.X_SIZE_SPRITE;
        size.y = GlobalConstant.Y_SIZE_SPRITE;
        
        var spriteRect = Icon.sprite.rect;
        spriteRect.size = new Vector2(size.x, size.y);
        Name = icon.name;
    }
}