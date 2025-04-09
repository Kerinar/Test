using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image _image;
    public bool _isFree = true;
    private bool _isAllowedToDrop = true;

    public int _positionX, _positionY;

    [SerializeField]private Inventory _inventory;

    //private List<Cell> _selectedCells = new List<Cell>();

    private void Awake()
    {
        _image = GetComponent<Image>();
        _inventory = GetComponentInParent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && _isAllowedToDrop == true)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                = GetComponent<RectTransform>().anchoredPosition - eventData.pointerDrag.GetComponent<Item>()._Offset;

            eventData.pointerDrag.GetComponent<Item>()._occupiedCells = eventData.pointerDrag.GetComponent<Item>()._selectedCells.ToList();
            eventData.pointerDrag.GetComponent<Item>()._selectedCells.Clear();

            var occupiedCells = eventData.pointerDrag.GetComponent<Item>()._occupiedCells;

            foreach(Cell cell in occupiedCells)
            {
                cell._isFree = false;
            }
        }
        else
        {
            eventData.pointerDrag.GetComponent<Item>().ReturnToSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //Поиск занимаемых клеток
            var objectSizeX = eventData.pointerDrag.GetComponent<Item>()._sizeX;
            var objectSizeY = eventData.pointerDrag.GetComponent<Item>()._sizeY;

            int offsetX = _positionX - eventData.pointerDrag.GetComponent<Item>().currentSelectedCellX;
            int offsetY = _positionY - eventData.pointerDrag.GetComponent<Item>().currentSelectedCellY;

            var outFlag = true;
            for (int i = offsetX; outFlag && (i < offsetX + objectSizeX); i++)
            {
                for(int j = offsetY; outFlag && (j < offsetY + objectSizeY); j++)
                {
                    try
                    {
                        eventData.pointerDrag.GetComponent<Item>()._selectedCells.Add(_inventory.GetCell(i, j));
                    }
                    catch
                    {
                        eventData.pointerDrag.GetComponent<Item>()._selectedCells.Clear();
                        outFlag = false;
                    }
                }
            }

            _isAllowedToDrop = true;

            var selectedCells = eventData.pointerDrag.GetComponent<Item>()._selectedCells;
            foreach (Cell cell in selectedCells)
            {
                if (cell._isFree == false)
                {
                    _isAllowedToDrop = false;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<Item>()._selectedCells.Clear();
        }
    }

    public void ColorCell()
    {
        if (_image.color == Color.red)
        {
            _image.color = new Color32(255, 255, 255, 100);
        }
        else
        {
            _image.color = Color.red;
        }
    }
}
