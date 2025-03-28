using System;
using RPGGame;
using UnityEngine;

public class SpellCardDetails : MonoBehaviour
{
    public PowerSelectionScript.PowerSelection powerForThisSpell;
    private PlayerPowers playerP;
    private void Start()
    {
        playerP = FindObjectOfType<PlayerPowers>();
    }

    public void LearnPower()
    {
        if (playerP.playersPowers.Count < 5)
        {
            playerP.playersPowers.Add(powerForThisSpell);
            playerP.UpdateInventoryBar();
            Debug.Log("Added this power to the player's powers" + powerForThisSpell.name);
        }
        else
        {
            Debug.Log("You can only learn 5 powers");
        }
    }
}
