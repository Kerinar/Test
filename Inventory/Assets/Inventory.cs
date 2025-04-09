using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Cell _cellPrefab;
    private Cell[,] _cells;

    public Cell GetCell(int x, int y)
    {
        return _cells[x, y];
    }

    private void Start()
    {
        _cells = new Cell[3,3];

        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var newCell = Instantiate(_cellPrefab, transform);
                newCell._positionX = i;
                newCell._positionY = j;
                _cells[i,j] = newCell;
            }
        }
    }
}
