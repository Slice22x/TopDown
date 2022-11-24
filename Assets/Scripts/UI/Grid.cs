using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int Width;
    private int Height;
    private float CellSize;
    public Dictionary<int[,], int> Count;
    public Item[,] GridArray;


    public Grid(int Width, int Height, float CellSize, GameObject CellGridUnit, Transform Parent)
    {
        this.Width = Width;
        this.Height = Height;
        this.CellSize = CellSize;
        Count = new Dictionary<int[,], int>(Width * Height);
        GridArray = new Item[Width, Height];

        for (int x = 0; x < GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < GridArray.GetLength(1); y++)
            {
                Count.Add(new int[x, y], 0);
                GameObject NewObject = MonoBehaviour.Instantiate(CellGridUnit, GetWorldPosition(x, y), Quaternion.identity);
                NewObject.transform.SetParent(Parent, false);
            }
        }
    }

    public Grid(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;
        Count = new Dictionary<int[,], int>(Width * Height);
        GridArray = new Item[Width, Height];

        for (int x = 0; x < GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < GridArray.GetLength(1); y++)
            {
                Count.Add(new int[x, y], 0);
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * CellSize;
    }

    public void SetValue(int x, int y, Item Items)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            if (GetValue(x, y) == null)
            {
                GridArray[x, y] = Items;
            }
        }
    }

    public Item GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return GridArray[x, y];
        }
        else
        {
            return null;
        }
    }
}
