using TMPro;
using UnityEngine;

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