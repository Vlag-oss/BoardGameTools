﻿namespace BoardGameTools.Shared.Models;

public class FightModel
{
    public bool Result { get; set; }
    public int Wound { get; set; }
    public List<Card> CardsUsed { get; set; } = new();
}