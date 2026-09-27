using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RunHistorySortHeader : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onDoubleClick = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (eventData.clickCount == 2)
        {
            onDoubleClick.Invoke();
        }
    }
}