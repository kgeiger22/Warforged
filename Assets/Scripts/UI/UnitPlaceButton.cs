using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlaceButton : MonoBehaviour {

    Unit unit;
    [HideInInspector]
    public int index;

    public void SetUnit(Unit _unit)
    {
        unit = _unit;
        transform.Find("Text").GetComponent<Text>().text = unit.name;
    }
    
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        unit.gameObject.SetActive(true);
        unit.ResetWithDraggable();
        transform.parent.GetComponent<UnitPlaceCanvas>().RemovePlaceButton(index);
    }
}
