using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Board : WarforgedMonoBehaviour {

    static public Board G_BOARD;

    //Set the width and height of the board
    public int width;
    public int height;

    protected override void OnGamePreInit()
    {
        G_BOARD = this;
        //initialize tiles
        for (int x = 0; x < transform.childCount; ++x)
        {
            Transform column = transform.GetChild(x);
            for (int y = 0; y < column.transform.childCount; ++y)
            {
                Tile tile = column.GetChild(y).GetComponent<Tile>();
                tile.SetCoordinates(x, y);
            }
        }
    }

    public void CreateBoard(int _width, int _height)
    {
        ResetBoard();

        for (int x = 0; x < _width; ++x)
        {
            GameObject column = new GameObject();
            column.name = "Column" + (x + 1).ToString();
            column.transform.position = new Vector3(x * 4, 0, 0);
            column.transform.parent = transform;
            for (int y = 0; y < _height; ++y)
            {
                CreateTile(x, y, Tile.Tile_Type.FLATLAND, column);
            }
        }
    }

    void ResetBoard()
    {
        for (int j = transform.childCount - 1; j >= 0; --j)
        {
            DestroyImmediate(transform.GetChild(j).gameObject);
        }
    }

    void CreateTile(int x, int y, Tile.Tile_Type type, GameObject column)
    {
        Tile new_tile = Instantiate(Resources.Load<Tile>("Prefabs/Flatland"));
        new_tile.SetCoordinates(x, y);
        new_tile.transform.position = new Vector3(x * 4, -0.5F, y * 4);
        new_tile.transform.parent = column.transform;
        new_tile.name = "Tile " + (x + 1).ToString() + "x" + (y + 1).ToString();
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height) return transform.GetChild(x).GetChild(y).GetComponent<Tile>();
        else return null;
    }

}

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board board = (Board)target;
        if (GUILayout.Button("Generate Board"))
        {
            board.CreateBoard(board.width, board.height);
        }
    }
}