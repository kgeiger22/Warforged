﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasManager {

    public enum Menu
    {
        STATE = 0,
        STORE,
        UNITPLACE,
        TILEINFO,
        UNITINFO,
        UNITACTION,
        UNITABILITY,
        COUNT
    }

    static List<GameObject> menus = new List<GameObject>();

	// Use this for initialization
	static public void Init () {
		for(int i = 0; i < (int)Menu.COUNT; ++i)
        {
            menus.Add(CreateCanvas((Menu)i));
        }
	}

    static GameObject CreateCanvas(Menu _menu)
    {
        GameObject result = null;
        switch (_menu)
        {
            case Menu.STATE:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/StateCanvas"));
                break;
            case Menu.STORE:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/StoreCanvas"));
                break;
            case Menu.UNITPLACE:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UnitPlaceCanvas"));
                break;
            case Menu.TILEINFO:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/TileInfoCanvas"));
                break;
            case Menu.UNITINFO:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UnitInfoCanvas"));
                break;
            case Menu.UNITACTION:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UnitActionCanvas"));
                break;
            case Menu.UNITABILITY:
                result = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UnitAbilityCanvas"));
                break;
            default:
                break;
        }
        if (result) result.SetActive(false);
        return result;
    }

    static public void EnableCanvas(Menu _menu)
    {
        menus[(int)_menu].SetActive(true);
    }

    static public void DisableCanvas(Menu _menu)
    {
        menus[(int)_menu].SetActive(false);
    }

    static public GameObject GetCanvas(Menu _menu)
    {
        return menus[(int)_menu];
    }

    static public UnitInfoCanvas GetUnitInfoCanvas()
    {
        return GetCanvas(Menu.UNITINFO).GetComponent<UnitInfoCanvas>();
    }
}
