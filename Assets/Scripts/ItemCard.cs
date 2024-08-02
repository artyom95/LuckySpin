using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ItemCard : MonoBehaviour

{
    [SerializeField] private SpriteRenderer _image;
    [field:SerializeField] public TextMeshProUGUI AmountRewarding { get; private set; }
    public string Name { get; private set; }

    public void SetName()
    {
        Name = _image.name;
    }
}