using System.Collections;
using System.Collections.Generic;
using System;

public class CombatHand : CombatDeck {

	public CombatHand()
    {
		Init();
    }

	public override void Init()
	{
		randomShuffler = new Random();
        cards = new List<Card>();
	}
	public List<Card> GetContents()
	{
		return cards;
	}
}
