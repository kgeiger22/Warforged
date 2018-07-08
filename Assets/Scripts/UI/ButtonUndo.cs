using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUndo : MonoBehaviour {

    public void OnClick()
    {
        SelectionManager.GetSelectedUnit().UndoMove();
    }
}
