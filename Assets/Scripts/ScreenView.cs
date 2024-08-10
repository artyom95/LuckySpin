using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ScreenView : MonoBehaviour
{
    [field: SerializeField] public List<RectTransform> TransformItemsList { get; private set; }

    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [field:SerializeField] public GameObject AttemptImage { get; private set; }
    [field:SerializeField] public GameObject RotateWheel { get; private set; }
    [field:SerializeField] public GameObject Arrow { get; private set; }
    [SerializeField] private GameObject _panel;
    [field:SerializeField] public GameObject Chest { get; private set; }


    public void Initialize([NotNull] List<Item> itemsList, int amountAttempts)
    {
        gameObject.SetActive(true);
        for (var i = 0; i < TransformItemsList.Count; i++)
        {
            var parentTransform = TransformItemsList[i] ;
            if (parentTransform != null)
            {
                itemsList[i].gameObject.transform.SetParent( parentTransform,false);
                itemsList[i].gameObject.transform.localPosition= Vector3.zero;
            }
        }
        SetAttempts(amountAttempts);
    }

    public void SetAttempts(int amountAttempts)
    {
        _textMeshPro.text = amountAttempts.ToString();
    }

    public void ChangeBackGround(bool shouldChangeBackGround)
    {
        _panel.SetActive(shouldChangeBackGround);
    }
}