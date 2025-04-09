using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] public int _sizeX;
    [SerializeField] public int _sizeY;
    [SerializeField] private Vector2 _slot;

    private RectTransform _rectTransform;
    private Image _currentSelectedCellImage;
    private Canvas _canvas;

    public int currentSelectedCellX;
    public int currentSelectedCellY;

    private Vector2 _offset;
    public Vector2 _Offset { get { return _offset; } }

    public List<Cell> _occupiedCells = new List<Cell>();
    public List<Cell> _selectedCells = new List<Cell>();

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void ReturnToSlot()
    {
        _rectTransform.anchoredPosition = _slot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentSelectedCellImage = eventData.pointerEnter.GetComponent<Image>();
        _offset = eventData.pointerEnter.GetComponent<RectTransform>().anchoredPosition;

        currentSelectedCellX = eventData.pointerEnter.GetComponent<ItemCell>().positionX;
        currentSelectedCellY = eventData.pointerEnter.GetComponent<ItemCell>().positionY;    

        _currentSelectedCellImage.raycastTarget = false;

        //Освобождение занятых клеток
        foreach (Cell cell in _occupiedCells)
        {
            cell._isFree = true;
        }

        _occupiedCells.Clear();
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _currentSelectedCellImage.raycastTarget = true;

        if (_occupiedCells.Count == 0)
        {
            ReturnToSlot();
        }
    }
}
