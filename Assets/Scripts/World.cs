using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class World : MonoBehaviour {

    static public World G_WORLD;

    public bool generate = false;

    //Set the width and height of the board
    public int width;
    public int height;
    public int previous_width { get; private set; }
    public int previous_height{ get; private set; }
    public void SetPreviousWidthHeight(int _width, int _height)
    {
        previous_width = _width;
        previous_height = _height;
    }

    //Adjusts the probability of each tile type
    //[SerializeField]
    //float flatland_factor;
    //[SerializeField]
    //float forest_factor;
    //[SerializeField]
    //float mountain_factor;
    //[SerializeField]
    //GameObject grid_object;
    //[SerializeField]
    //GameObject highlight_object;

    //Stores tile data

    void Awake () {
        G_WORLD = this;

        //if (generate)
        //{
        //    CreateBoard(width, height);
        //}

	}
	

	void Update () {
        if (generate)
        {
            if (width != previous_width || height != previous_height)
            {
                CreateBoard(width, height);
            }
            previous_width = width;
            previous_height = height;
        }
    }

    public void CreateBoard(int _width, int _height)
    {
        ResetBoardImmediate();

        // //Normalize tile factors
        // float total = flatland_factor + forest_factor + mountain_factor;
        // flatland_factor = flatland_factor / total;
        // forest_factor = forest_factor / total;
        // mountain_factor = mountain_factor / total;

        //Use perlin noise to generate map
        for (int x = 0; x < _width; ++x)
        {
            GameObject column = new GameObject();
            column.name = "Column" + (x + 1).ToString();
            column.transform.position = new Vector3(x * 4, 0, 0);
            column.transform.parent = transform;
            for (int y = 0; y < _height; ++y)
            {
                // float r = Mathf.PerlinNoise((float)x * 4 / width, (float)y * 4 / height);
                //
                // if (r < flatland_factor)
                // {
                //     CreateTile(x, y, Tile.Tile_Type.FLATLAND);
                // }
                // else if (r < forest_factor + flatland_factor)
                // {
                //     CreateTile(x, y, Tile.Tile_Type.FOREST);
                // }
                // else
                // {
                //     CreateTile(x, y, Tile.Tile_Type.MOUNTAIN);
                // }
                CreateTile(x, y, Tile.Tile_Type.FLATLAND, column);
            }
        }
    }

    void ResetBoardImmediate()
    {
        for (int j = transform.childCount - 1; j >= 0; --j)
        {
            DestroyImmediate(transform.GetChild(j).gameObject);
        }
    }

    void CreateTile(int x, int y, Tile.Tile_Type type, GameObject column)
    {
        Tile new_tile = Instantiate(Resources.Load<Tile>("Prefabs/Tile"));
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

    public void InitalizeTiles()
    {
        for (int x = 0; x < transform.childCount; ++x)
        {
            Transform column = transform.GetChild(x);
            for (int y = 0; y < column.transform.childCount; ++y)
            {
                Tile tile = column.GetChild(y).GetComponent<Tile>();
                tile.SetCoordinates(x, y);
                tile.Init();
            }
        }
    }
}

//[CustomEditor(typeof(World))]
//public class LevelScriptEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        World script = (World)target;
//        if (script.width != script.previous_width || script.height != script.previous_height)
//        {
//            script.CreateBoard(script.width, script.height);
//        }
//        script.SetPreviousWidthHeight(script.width, script.height);
//
//    }
//}