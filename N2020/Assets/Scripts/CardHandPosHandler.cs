using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandPosHandler : MonoBehaviour
{
    Card card;

    // Start is called before the first frame update
    void Start()
    {
        card = GetComponentInChildren<Card>();
    }

    public void SetHandCardPos()
    {
        card.SetHandPosition();
    }
}
