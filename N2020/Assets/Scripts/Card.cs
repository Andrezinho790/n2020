using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardObject cardData;
    public bool spawn;
    public bool isUsed;
    public Vector3 initialPos;
    public Vector3 handPos;
    public int handId;

    private SpriteRenderer sr;
    private Collider col;

    [HideInInspector]public Animator cardAnim;

    [SerializeField]private GameManager gameManager;

    
    private void Start()
    {
        cardAnim = transform.GetComponentInParent<Animator>();
        initialPos = transform.position;
    }

    public void Refresh()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider>();
        col.enabled = true;
        sr.enabled = true;
        sr.sprite = cardData.sprite;
        spawn = true;
        isUsed = false;        
    }

    public void Update()
    {
        if(spawn)
        {
            GoToHandPoint();
        }
        if(gameManager.gamePhase == 1)
        {
            cardAnim.SetBool("isOpen", false);   
        }
    }

    public void ReturnToInitialSprite()
    {
        sr.color = new Color(255,255,255);
    }

    public void HighlightSprite()
    {
        sr.color = Color.green;
    }

    public void ChangeAlpha()
    {
        Color color = sr.color;
        color.a = 0.2f;
        sr.color = color;
    }

    public void SpawnActor(Vector3 position)
    {
        switch (cardData.type)
        {
            case CardType.WARRIOR:
                Instantiate(Resources.Load("Allies/Warrior"), position, Quaternion.identity);
                break;
            case CardType.VALKYRIES:
                Instantiate(Resources.Load("Allies/Valkyrie"), position, Quaternion.identity);
                break;
            case CardType.ARCHER:
                Instantiate(Resources.Load("Allies/Archer"), position, Quaternion.identity);
                break;
            case CardType.CURE:
                Instantiate(Resources.Load("Auras/CureAura"), position, Quaternion.identity);
                break;
            case CardType.FORTIFY:
                Instantiate(Resources.Load("Auras/FortifyAura"), position, Quaternion.identity);
                break;
            case CardType.BERSERK:
                Instantiate(Resources.Load("Auras/BerserkAura"), position, Quaternion.identity);
                break;
        }
    }

    public void GoToHandPoint()
    {
        cardAnim.SetBool("isOpen", true);        
        spawn = false;
    }

    public void GoToSpawnPoint()
    {
        if (isUsed)
        {
            sr.enabled = false;
            col.enabled = false;
        }
    }

    public void SetHandPosition()
    {
        handPos = transform.position;
    }
}
