using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { WARRIOR, ARCHER, VALKYRIES, CURE, FORTIFY, BERSERK, TOWER_MELEE, TOWER_TRAP }

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/SpawnCard", order = 1)]
public class CardObject : ScriptableObject
{
    public CardType type;
    public string cardName;
    public Sprite sprite;
    public int elixirCost;
}
