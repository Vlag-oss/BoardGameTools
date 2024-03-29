﻿using BoardGameTools.Shared.Models;

namespace BoardGameTools.Server.Models;

public class ParryModel
{
    public bool Result { get; }
    public List<Card> CardsUsed { get; }
    public int TotalParry { get; }

    public ParryModel(bool result, List<Card> cardsUsed, int totalParry)
    {
        Result = result;
        CardsUsed = cardsUsed;
        TotalParry = totalParry;
    }
}