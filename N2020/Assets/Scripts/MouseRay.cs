using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    public GameObject pointToSpawn;

    private Transform lastOutlineTransform;
    private Transform lastCardSpriteRenderer;
    private Vector3 initialPos;
    private bool isHoldingDown;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if (lastCardSpriteRenderer)
            {
                CardSystem(hit);
            }

            if (lastOutlineTransform != null)
            {
                if (hit.transform != lastOutlineTransform)
                {
                    lastOutlineTransform.GetComponent<Outline>().enabled = false;
                }                
            }

            if (hit.transform.CompareTag("TowerSlot") && gameManager.gamePhase == 0)
            {
                hit.transform.GetComponent<Outline>().enabled = true;
                lastOutlineTransform = hit.transform;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    hit.transform.GetChild(0).gameObject.SetActive(true);
                }
            }

            if (hit.transform.CompareTag("Card") && !isHoldingDown)
            {
                lastCardSpriteRenderer = hit.transform;             
            }
            else if(!hit.transform.CompareTag("Card") && !isHoldingDown)
            {
                lastCardSpriteRenderer = null;
            }
        }        
    }

    void CardSystem(RaycastHit hit)
    {
        Card card = lastCardSpriteRenderer.GetComponent<Card>();
        if(card)
        {
            card.HighlightSprite();

            if (lastCardSpriteRenderer != hit.transform)
            {
                card.ReturnToInitialSprite();
            }

            if(!card.isUsed)
            {
                if (Input.GetMouseButton(0))
                {
                    isHoldingDown = true;
                    Vector3 temp = Input.mousePosition;
                    temp.z = 60f;
                    lastCardSpriteRenderer.position = Camera.main.ScreenToWorldPoint(temp);

                    Ray cardRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit cardHit;

                    if (Physics.Raycast(cardRay, out cardHit, Mathf.Infinity, LayerMask.NameToLayer("Card")))
                    {
                        if (cardHit.transform.CompareTag("Scenario"))
                        {
                            card.ChangeAlpha();
                            pointToSpawn.SetActive(true);
                            pointToSpawn.transform.position = new Vector3(cardHit.point.x, cardHit.point.y, cardHit.point.z);
                        }
                        else
                        {
                            pointToSpawn.SetActive(false);
                        }
                    }
                    else
                    {
                        pointToSpawn.SetActive(false);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (pointToSpawn.activeInHierarchy && GameManager.Instance.elixir >= card.cardData.elixirCost)
                    {
                        card.SpawnActor(pointToSpawn.transform.position);
                        Deck.Instance.AddToCemetery(card.handId);
                        pointToSpawn.SetActive(false);
                    }
                    else
                    {
                        pointToSpawn.SetActive(false);
                        card.ReturnToInitialSprite();
                    }

                    isHoldingDown = false;
                    lastCardSpriteRenderer.position = card.handPos;
                    lastCardSpriteRenderer = null;                    
                }
            }
        }
    }
}
