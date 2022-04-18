using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public Vector2 cell;
    [SerializeField] private GameObject slotManagerObject;

    public GameObject[][] cells;

    private RectTransform rectTransform;
    private Image image;

    private bool disabled = false;
    private bool availableShow = true;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Start()
    {
        cells = slotManagerObject.GetComponent<SlotManager>().cells; // Must be later than Awake of SlotManager
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (disabled) return;
        //Debug.Log("OnDrop (ItemSlot)");
        if (eventData.pointerDrag != null)
        {
            if (CheckCanBeDropped(eventData.pointerDrag))
            {
                eventData.pointerDrag.GetComponent<ShipDragDrop>().Pin(rectTransform.position, this);
            }
        }
    }

    public bool CheckCanBeDropped(GameObject shipObject)
    {
        if (disabled) return false;
        bool canBeDropped = true;
        ShipDragDrop itemDragDrop = shipObject.GetComponent<ShipDragDrop>();
        switch (itemDragDrop.size)
        {
            case 1:
                // [p]
                //canBeDropped = true;
                break;
            case 2:
                // [p][ ]
                //
                // [p]
                // [ ]
                if (itemDragDrop.horizontally)
                {
                    if ((int)cell.x + 1 < 10)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 1].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                else
                {
                    if ((int)cell.y + 1 < 10)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                break;
            case 3:
                // [ ][p][ ]
                //
                // [ ]
                // [p]
                // [ ]
                if (itemDragDrop.horizontally)
                {
                    if ((int)cell.x + 1 < 10 && (int)cell.x - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 1].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x - 1].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                else
                {
                    if ((int)cell.y + 1 < 10 && (int)cell.y - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y - 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                break;
            case 4:
                // [ ][p][ ][ ]
                //
                // [ ]
                // [p]
                // [ ]
                // [ ]
                if (itemDragDrop.horizontally)
                {
                    if ((int)cell.x + 2 < 10 && (int)cell.x - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 1].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x + 2].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y][(int)cell.x - 1].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                else
                {
                    if ((int)cell.y + 2 < 10 && (int)cell.y - 1 > -1)
                    {
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y + 2][(int)cell.x].GetComponent<ShipSlot>().disabled;
                        canBeDropped = canBeDropped && !cells[(int)cell.y - 1][(int)cell.x].GetComponent<ShipSlot>().disabled;
                    }
                    else
                    { canBeDropped = false; break; }
                }
                break;
            default:
                canBeDropped = false;
                break;
        }
        return canBeDropped;
    }

    public void SetState(bool en)
    {
        disabled = !en;
        if (en || !availableShow)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = new Color(0.96f, 0.96f, 1.0f);
        }
    }

    public bool CheckEnabled()
    {
        return !disabled;
    }

    public void SetAvailableShow(bool en)
    {
        availableShow = en;
        SetState(!disabled); // Update color
    }
}