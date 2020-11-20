using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private void Awake()
    {
        if (instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public Transform enemy1SpawnPoint;
    public Transform enemy2SpawnPoint;
    public Transform enemy3SpawnPoint;
    public Transform bossSpawnPoint;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject boss;
    
    public List<Transform> pathTargets;

    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI elixirText;
    public Animator phaseTextAnim;

    public Button finishPlanningPhaseBtn;
    public Animator finishPlanningPhaseAnim;

    private bool isSpawning;

    public int gamePhase;
    int gameWave;

    public List<Wave> waves;

    public float playerHealth = 10;

    public Animator GameOverAnim;

    public int elixir;
    public int maxElixir;
    private int minElixir;

    public float timeToSpawnBoss;

    public List<AudioClip> GruntsAudios;
    public List<AudioClip> GruntsValkAudios;

    public List<AudioClip> DieAudios;
    public List<AudioClip> ValkDieAudios;

    public List<AudioClip> Enemy1Grunts;
    public List<AudioClip> Enemy2Grunts;
    public List<AudioClip> Enemy3Grunts;

    public List<AudioClip> StartFightAudios;

    AudioSource audioSr;
    void Start()
    {
        audioSr = GetComponent<AudioSource>();
        elixir = minElixir = 3;
        gameWave = -1;
        finishPlanningPhaseBtn.interactable = false;
        StartCoroutine(StartPlanningPhase());
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy1SpawnPoint.childCount <= 0 && enemy2SpawnPoint.childCount <= 0 && enemy3SpawnPoint.childCount <= 0 &&gamePhase == 1 && !isSpawning)
        {
            StartCoroutine(StartPlanningPhase());
        }
    }

    IEnumerator StartPlanningPhase()
    {
        gamePhase = 0;
        elixirText.enabled = true;
        elixirText.text = "Elixir: " + elixir;
        phaseText.text = "Fase de Planejamento";
        phaseTextAnim.SetTrigger("isOpen");
        yield return new WaitForSeconds(3f);
        Deck.Instance.DrawCards();
        finishPlanningPhaseAnim.SetBool("isOpen", true);
        finishPlanningPhaseBtn.interactable = true;   
    }

    public void StartWavePhase()
    {

        foreach (Card card in Deck.Instance.hand)
        {
            if (!card.isUsed)
            {
                Deck.Instance.AddToCemetery(card.handId);
            }
        }

        gameWave++;

        audioSr.PlayOneShot(StartFightAudios[gameWave]);

        if (elixir < maxElixir)
        {
            elixir = minElixir + (gameWave + 1);
        }

        gamePhase = 1;
        finishPlanningPhaseAnim.SetBool("isOpen", false);
        finishPlanningPhaseBtn.interactable = false;
        elixirText.enabled = false;
        phaseText.text = "Fase de Combate";
        phaseTextAnim.SetTrigger("isOpen");
        isSpawning = true;
        StartCoroutine(SpawnEnemiesTypeOne());
        StartCoroutine(SpawnEnemiesTypeTwo());
        StartCoroutine(SpawnEnemiesTypeThree());
        if (waves[gameWave].spawnBoss)
        {
            StartCoroutine(SpawnBoss());
        }
    }

    IEnumerator SpawnEnemiesTypeOne()
    {
        yield return new WaitForSeconds(1.5f);
        while(enemy1SpawnPoint.childCount < waves[gameWave].enemy1Amount)
        {
            yield return new WaitForSeconds(waves[gameWave].enemy1SpawnRate);
            Instantiate(enemy1, enemy1SpawnPoint);
        }
    }
    IEnumerator SpawnEnemiesTypeTwo()
    {
        yield return new WaitForSeconds(1.5f);
        while (enemy2SpawnPoint.childCount < waves[gameWave].enemy2Amount)
        {
            yield return new WaitForSeconds(waves[gameWave].enemy2SpawnRate);
            Instantiate(enemy2, enemy2SpawnPoint);
        }
    }
    IEnumerator SpawnEnemiesTypeThree()
    {
        yield return new WaitForSeconds(1.5f);
        while (enemy3SpawnPoint.childCount < waves[gameWave].enemy3Amount)
        {
            yield return new WaitForSeconds(waves[gameWave].enemy3SpawnRate);
            Instantiate(enemy3, enemy3SpawnPoint);
        }
        yield return new WaitForSeconds(1f);
        isSpawning = false;
    }
    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(1.5f);
        yield return new WaitForSeconds(timeToSpawnBoss);

        Instantiate(boss, bossSpawnPoint);

        yield return new WaitForSeconds(1f);
        isSpawning = false;

    }
    public void TakeDamage()
    {
        playerHealth--;
        if (playerHealth <= 0) 
        {
            GameOver();
        }
    }

    void GameOver()
    {
        GameOverAnim.SetBool("isOpen", true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeElixir(int elixir)
    {
        this.elixir += elixir;
        elixirText.text = "Elixir: " + this.elixir;
    }
}
