using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAbility : MonoBehaviour {
    private Unit owner;
    private int ability_index;

    public void Initialize(Unit _owner, int _ability_index, string _name)
    {
        owner = _owner;
        ability_index = _ability_index;
        transform.Find("Text").GetComponent<Text>().text = _name;
    }

    public void OnClick()
    {
        owner.SelectAbility(ability_index);
    }
}
