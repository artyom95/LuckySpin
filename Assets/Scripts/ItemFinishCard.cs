using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ItemFinishCard : MonoBehaviour

{
    [SerializeField] private SpriteRenderer _image;
    [field: SerializeField] public TextMeshProUGUI AmountRewarding;
    public string Name { get; private set; }

    public void SetName()
    {
        Name = _image.name;
    }
    public void SetAmountRewarding(string amountRewarding)
    {
        AmountRewarding.text = amountRewarding;
    }
}