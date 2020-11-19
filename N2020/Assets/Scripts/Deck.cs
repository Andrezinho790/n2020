using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    #region SINGLETON
    private static Deck instance;

    public static Deck Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private void Awake()
    {
        if(instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public List<CardObject> cards = new List<CardObject>(12);
    public List<CardObject> cemetery = new List<CardObject>();
    public List<Card> hand = new List<Card>();
    
    public void DrawCards()
    {
        if(cards.Count > 0)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                CardObject card = cards[Random.Range(0, cards.Count)];
                hand[i].cardData = card;
                hand[i].Refresh();
                cards.Remove(card);
            }
        }
        else
        {
            foreach (CardObject card in cemetery)
            {
                cards.Add(card);
            }

            cemetery.Clear();
        }
    }

    public void AddToCemetery(int handId)
    {
        Card card = hand[handId];

        cemetery.Add(card.cardData);
        card.isUsed = true;
        card.GoToSpawnPoint();
        GameManager.Instance.ChangeElixir(-card.cardData.elixirCost);
    }    
}
