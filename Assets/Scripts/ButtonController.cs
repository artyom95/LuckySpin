using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button _spinButton;
    
    [field:SerializeField] private Button _calmButton;

    private UnityAction _subscribeSpinAction;
    private UnityAction _subscribeCalmAction;

    public void SubscribeSpinButton(UnityAction subscribeAction)
    {
        _subscribeSpinAction = subscribeAction;
        _spinButton.onClick.AddListener(subscribeAction);
    }
    public void SubscribeCalmButton(UnityAction subscribeAction)
    {
        _subscribeCalmAction = subscribeAction;
        _calmButton.onClick.AddListener(subscribeAction);
    }
    
    public void LockSpinButton()
    {
        _spinButton.interactable = false;
    }
    
    public void UnLockSpinButton()
    {
        _spinButton.interactable =true;
    }

    public void  TurnOnCalmButton()
    {
         _calmButton.gameObject.SetActive(true);
    }
    private void OnDestroy()
    {
        _spinButton.onClick.RemoveListener(_subscribeSpinAction);
        _calmButton.onClick.RemoveListener(_subscribeCalmAction);
    }
}