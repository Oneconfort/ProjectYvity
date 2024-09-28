using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cheats
{
    public static int campFireIndex;
    public static CampFire[] campFires;

    public static void GoToNextCampFire()
    {
        campFireIndex++;
        if (!MaybeLoadCampFires())
        {
            return;
        }
        
        if (campFireIndex >= campFires.Length)
        {
            campFireIndex = 0;
        }

        GameController.controller.Player.Respawn(campFires[campFireIndex]);
    }

    public static void GoToPreviousCampFire()
    {
        if (!MaybeLoadCampFires())
        {
            return;
        }

        campFireIndex--;
        if (campFireIndex < 0)
        {
            campFireIndex = campFires.Length - 1;
        }

        GameController.controller.Player.Respawn(campFires[campFireIndex]);
    }

    static bool MaybeLoadCampFires()
    {
        if (campFires == null || campFires.Length == 0)
        {
            campFires = Object.FindObjectsOfType<CampFire>();
            if (campFires == null || campFires.Length == 0)
            {
                Debug.Assert(false, "There is no camp fires in this level.");
                return false;
            }

            campFireIndex = 0;
        }

        return true;
    }
}
