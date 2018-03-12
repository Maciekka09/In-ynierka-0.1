using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LayoutAnchor : MonoBehaviour
{
    RectTransform ownRectTransform;
    RectTransform parentRectTranform;

    void Awake()
    {
        ownRectTransform = transform as RectTransform;
        parentRectTranform = transform.parent as RectTransform;
        if (parentRectTranform == null)
            Debug.LogError("This component requires a RectTransform parent to work.", gameObject);
    }

    Vector2 GetPosition(RectTransform rt, TextAnchor anchor)
    {
        Vector2 returnValue = Vector2.zero;
        switch (anchor)
        {
            case TextAnchor.LowerCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.UpperCenter:
                returnValue.x += rt.rect.width * 0.5f;
                break;
            case TextAnchor.LowerRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.UpperRight:
                returnValue.x += rt.rect.width;
                break;
        }
        switch (anchor)
        {
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                returnValue.y += rt.rect.height * 0.5f;
                break;
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                returnValue.y += rt.rect.height;
                break;
        }
        return returnValue;
    }

    public Vector2 AnchorPosition(TextAnchor ownAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        Vector2 ownOffset = GetPosition(ownRectTransform, ownAnchor);
        Vector2 parentOffset = GetPosition(parentRectTranform, parentAnchor);
        Vector2 anchorCenter = new Vector2(Mathf.Lerp(ownRectTransform.anchorMin.x, ownRectTransform.anchorMax.x, ownRectTransform.pivot.x), Mathf.Lerp(ownRectTransform.anchorMin.y, ownRectTransform.anchorMax.y, ownRectTransform.pivot.y));
        Vector2 ownAnchorOffset = new Vector2(parentRectTranform.rect.width * anchorCenter.x, parentRectTranform.rect.height * anchorCenter.y);
        Vector2 ownPivotOffset = new Vector2(ownRectTransform.rect.width * ownRectTransform.pivot.x, ownRectTransform.rect.height * ownRectTransform.pivot.y);
        Vector2 pos = parentOffset - ownAnchorOffset - ownOffset + ownPivotOffset + offset;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        return pos;
    }

    public void SnapToAnchorPosition(TextAnchor ownAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        ownRectTransform.anchoredPosition = AnchorPosition(ownAnchor, parentAnchor, offset);
    }

    public Tweener MoveToAnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        return ownRectTransform.AnchorTo(AnchorPosition(myAnchor, parentAnchor, offset));
    }

}