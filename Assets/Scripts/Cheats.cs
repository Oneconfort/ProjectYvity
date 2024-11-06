using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cheats
{
    public static int campFireIndex;

    public static void GoToNextCampFire()
    {
        campFireIndex++;
        if (campFireIndex >= GameController.controller.campFires.Length)
        {
            campFireIndex = 0;
        }

        GameController.controller.Player.Respawn(GameController.controller.campFires[campFireIndex]);
    }

    public static void GoToPreviousCampFire()
    {
        campFireIndex--;
        if (campFireIndex < 0)
        {
            campFireIndex = GameController.controller.campFires.Length - 1;
        }

        GameController.controller.Player.Respawn(GameController.controller.campFires[campFireIndex]);
    }
}
