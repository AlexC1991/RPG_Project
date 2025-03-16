using RPGGame;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities & Spells DataBase Container", menuName = "RPG Game SO/Abilities & Spells DataBase", order = 1)]
public class AbilityContainer : ScriptableObject
{
    public PowerSelectionScript[] powerSelectionScript;
}
