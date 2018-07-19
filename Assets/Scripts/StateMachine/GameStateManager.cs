using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateManager {

	public static void EndTurn()
    {
        if (UnitManager.RemainingUnitsWithMoves() == 0)
        {
            Global.GAMESTATEFSM.SetState(new RoundState());
        }
        else Global.GAMESTATEFSM.NextState();
    }

    public static void EndRound()
    {

    }
}
