using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager {

    public static Player.Type CurrentPlayer;

    public static Player GetCurrentPlayer()
    {
        return GetPlayer(CurrentPlayer);
    }

    public static Player GetPlayer(Player.Type _type)
    {
        switch (_type)
        {
            case Player.Type.PLAYER1:
                return Global.PLAYER1;
            case Player.Type.PLAYER2:
                return Global.PLAYER2;
            default:
                return null;
        }
    }

    public static void SwitchPlayerTurn()
    {
        switch (CurrentPlayer)
        {
            case Player.Type.PLAYER1:
                CurrentPlayer = Player.Type.PLAYER2;
                break;
            case Player.Type.PLAYER2:
                CurrentPlayer = Player.Type.PLAYER1;
                break;
            default:
                break;
        }
    }

    public static void SetPlayerTurn(Player.Type _type)
    {
        CurrentPlayer = _type;
    }

    public static Player.Type GetHighestSPDPlayerType()
    {
        int player1_speed = 0;
        int player2_speed = 0;
        foreach(Unit unit in UnitManager.units.Values)
        {
            if (unit.owner == Player.Type.PLAYER1) player1_speed += unit.GetModifiedSPD();
            else if (unit.owner == Player.Type.PLAYER2) player2_speed += unit.GetModifiedSPD();
        }
        if (player1_speed > player2_speed) return Player.Type.PLAYER1;
        else if (player2_speed > player1_speed) return Player.Type.PLAYER2;
        else return Random.Range(0f, 1f) > 0.5f ? Player.Type.PLAYER1 : Player.Type.PLAYER2;
    }
}
