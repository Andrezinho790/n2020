using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AuraType { CURE, FORTIFY, BERSERK }

public class AuraEffect : MonoBehaviour
{
    public AuraType auraType;
    public float timeOfAura;
    public RFX4_EffectSettings effect;

    [Header("CURE PROPERTIES")]
    public int amountOfCure;

    [Header("FORTIFY PROPERTIES")]
    public int amountOfLifeToAdd;

    [Header("BERSERK PROPERTIES")]
    public int amountOfStrengthToAdd;

    private List<TroopController> troopsAffected = new List<TroopController>();
    private bool isActive;

    private void Start()
    {
        isActive = false;        
    }

    private void Update()
    {
        if(GameManager.Instance.gamePhase == 1)
        {
            if(!isActive)
            {
                Invoke("FinishVFXEffect", timeOfAura - 2.5f);
                Invoke("FinishThisEffect", timeOfAura);
            }

            isActive = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ally"))
        {
            TroopController ally = other.GetComponent<TroopController>();
            if (!troopsAffected.Find(x => x == ally))
            {
                troopsAffected.Add(ally);

                switch (auraType)
                {
                    case AuraType.FORTIFY:
                        ally.Fortify(amountOfLifeToAdd);
                        break;
                    case AuraType.BERSERK:
                        ally.Berserk(amountOfStrengthToAdd);
                        break;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ally"))
        {
            TroopController ally = other.GetComponent<TroopController>();

            switch (auraType)
            {
                case AuraType.CURE:
                    ally.Cure(amountOfCure);
                    break;
            }
        }
    }

    void FinishVFXEffect()
    {
        effect.IsVisible = false;
    }

    void FinishThisEffect()
    {
        foreach (TroopController ally in troopsAffected)
        {
            switch (auraType)
            {
                case AuraType.FORTIFY:
                    ally.Fortify(amountOfLifeToAdd * -1);
                    break;
                case AuraType.BERSERK:
                    ally.Berserk(amountOfStrengthToAdd * -1);
                    break;
            }
        }

        Destroy(this.gameObject);
    }
}
