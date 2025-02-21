using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected bool hasClicked;
    [SerializeField] protected Image highlight;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (hasClicked) return;
        highlight.gameObject.SetActive(true);
        highlight.color = Color.cyan;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (hasClicked) return;
        highlight.gameObject.SetActive(false);
    }

    protected virtual void OnDisable()
    {
        highlight.gameObject.SetActive(false);
    }
}
