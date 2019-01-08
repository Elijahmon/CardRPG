using System.Collections.Generic;
using System;

public class CombatDeck {

    public CombatDeck()
    {
        randomShuffler = new Random();
        cards = new List<Card>();
    }

    public CombatDeck(CombatDeck deck)
    {
        randomShuffler = new Random();
        cards = new List<Card>();
        deck.cards.ForEach((x) => { cards.Add(x); } );
    }

    enum DeckType {Combat, Adventure, Equipment }

    protected List<Card> cards;
    protected Random randomShuffler;

    // Use this for initialization
    public virtual void Init()
    {

	}

    public void Init(List<Enums.Card> cardTypes)
    {
        foreach(Enums.Card type in cardTypes)
        {
            cards.Add(new Card(type));
        }
    }
	
    public void Shuffle()
    {
        
        for(int i = cards.Count - 1; i > 0; --i)
        {
            int rand = randomShuffler.Next(i + 1);
            Card temp = cards[i];
            cards[i] = cards[rand];
            cards[rand] = temp; 
        }
    }

    public Card RemoveTopCard()
    {
        Card cardToRemove = cards[0];
        cards.RemoveAt(0);
        return cardToRemove;
    }

    public void AddCardToBottom(Card card)
    {
        cards.Add(card);
    }

    public int DeckCount()
    {
        return cards.Count;
    }

    public List<Card> Clear()
    {
        List<Card> cardsToRemove = new List<Card>();

        for(int i = 0; i < cards.Count; i++)
        {
            cardsToRemove.Add(cards[i]);
        }
        cards.Clear();

        return cardsToRemove;
    }

    public Card Discard(Card c)
    {
        cards.Remove(c);
        return c;
    }
}
