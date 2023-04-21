using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGridLayout_MatchHeigh : GridLayoutGroup {
    RectTransform parentRect, rect;
    protected override void Awake() {
        rect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(parentRect.sizeDelta.x, rect.sizeDelta.y);
    }
    public override void CalculateLayoutInputHorizontal() {
        if (!gameObject.activeInHierarchy) return;

        if (constraint == Constraint.FixedColumnCount) {
            float width = rectTransform.rect.width - padding.left - padding.right - spacing.x * (constraintCount - 1);
            float size = width / constraintCount;
            float rate = size / m_CellSize.x;
            Vector2 newCellSize = m_CellSize * rate;
            m_CellSize = new Vector3(newCellSize.x, newCellSize.x, 1);
            int newRow = transform.childCount / constraintCount;
            while ((cellSize.y * newRow + spacing.y * (newRow - 1)) > rectTransform.rect.height) {
                m_CellSize -= Vector2.one;
            }
        }
        base.CalculateLayoutInputHorizontal();
    }

}
