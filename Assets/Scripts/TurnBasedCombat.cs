using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnBasedCombat : MonoBehaviour
{
    private Character hero;
    private Character enemy;
    private TurnManager turnManager;

    private bool heroTurn = true;

    [Header("Config")]
    public float turnDelay = 2f; // Tiempo entre turnos

    [Header("UI Vida")]
    public Slider heroHealthBar;
    public Slider enemyHealthBar;

    [Header("UI Vida - Texto")]
    public TextMeshProUGUI heroHealthText;
    public TextMeshProUGUI enemyHealthText;

    [Header("UI Vida - Colores")]
    public Image heroFill;
    public Image enemyFill;
    public Color highHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    [Header("UI - Botones de acción")]
    public Button attackButton;
    public Button defendButton;
    public Button healButton;
    public Button fortifyButton;
    public Button stunButton;

    [Header("UI - Estado")]
    public TextMeshProUGUI statusText;

    [Header("UI - Pantalla final")]
    public GameObject endScreen;
    public TextMeshProUGUI endMessageText;
    public Button restartButton;
    public Button quitButton;
    public CharacterBehaviour heroObject;
    public CharacterBehaviour enemyObject;

    [Header("Audio - Música")]
    public AudioSource backgroundMusic;   // música de fondo
    public AudioClip victoryMusic;        // clip de victoria
    public AudioClip defeatMusic;         // clip de derrota
    public AudioSource sfxSource;         // fuente para efectos de sonido

    [Header("Audio - Efectos")]
    public AudioClip attackSFX;
    public AudioClip defendSFX;
    public AudioClip healSFX;
    public AudioClip fortifySFX;
    public AudioClip stunSFX;
    public AudioClip poisonSFX;
    public AudioClip heroHitSFX;
    public AudioClip enemyHitSFX;

    void Start()
    {
        hero = heroObject.Data;
        enemy = enemyObject.Data;

        turnManager = new TurnManager();

        // Config sliders
        heroHealthBar.maxValue = hero.MaxHealth;
        enemyHealthBar.maxValue = enemy.MaxHealth;

        heroHealthBar.value = hero.Health;
        enemyHealthBar.value = enemy.Health;
        UpdateHealthBars();

        // Mensaje inicial
        ShowStatus(hero, "comienza la batalla. Elige una acción.");

        // Listeners botones héroe
        attackButton.onClick.AddListener(() => OnHeroAction("attack"));
        defendButton.onClick.AddListener(() => OnHeroAction("defend"));
        healButton.onClick.AddListener(() => OnHeroAction("heal"));
        if (fortifyButton != null)
            fortifyButton.onClick.AddListener(() => OnHeroAction("fortify"));
        if (stunButton != null)
            stunButton.onClick.AddListener(() => OnHeroAction("stun"));

        // Botones pantalla final
        restartButton.onClick.AddListener(RestartBattle);
        quitButton.onClick.AddListener(QuitGame);

        endScreen.SetActive(false);

        // Reproducir música de fondo
        if (backgroundMusic != null)
            backgroundMusic.Play();
    }

    void UpdateHealthBars()
    {
        heroHealthBar.value = hero.Health;
        enemyHealthBar.value = enemy.Health;

        heroHealthText.text = $"{hero.Health} / {hero.MaxHealth}";
        enemyHealthText.text = $"{enemy.Health} / {enemy.MaxHealth}";

        UpdateHealthColor(heroFill, hero.Health, hero.MaxHealth);
        UpdateHealthColor(enemyFill, enemy.Health, enemy.MaxHealth);
    }

    void UpdateHealthColor(Image fill, int current, int max)
    {
        float ratio = (float)current / max;

        if (ratio > 0.6f)
            fill.color = highHealthColor;
        else if (ratio > 0.3f)
            fill.color = midHealthColor;
        else
            fill.color = lowHealthColor;
    }

    void OnHeroAction(string action)
    {
        if (!heroTurn) return;

        DisableButtons(); // Bloquear acciones

        if (hero.IsStunned)
        {
            ShowStatus(hero, "está aturdido y pierde el turno");
            PlaySFX(stunSFX);
            hero.IsStunned = false;
            heroTurn = false;

            hero.UpdateEffects();
            enemy.UpdateEffects();

            if (enemy.Health > 0)
                StartCoroutine(EnemyTurnCoroutine());
            return;
        }

        switch (action)
        {
            case "attack":
                turnManager.AddCommand(new AttackCommand(hero, enemy, 20));
                ShowStatus(hero, $"ataca a {enemy.Name}");
                PlaySFX(attackSFX);
                PlaySFX(enemyHitSFX);
                break;
            case "defend":
                turnManager.AddCommand(new DefendCommand(hero));
                ShowStatus(hero, "se defiende");
                PlaySFX(defendSFX);
                break;
            case "heal":
                turnManager.AddCommand(new HealCommand(hero, 15));
                ShowStatus(hero, "se cura");
                PlaySFX(healSFX);
                break;
            case "fortify":
                hero.ApplyStatus(new FortifyEffect(2, 5));
                ShowStatus(hero, "se fortifica");
                PlaySFX(fortifySFX);
                break;
            case "stun":
                enemy.ApplyStatus(new StunEffect(1));
                ShowStatus(hero, $"aturde a {enemy.Name}");
                PlaySFX(stunSFX);
                PlaySFX(enemyHitSFX);
                break;
        }

        turnManager.ExecuteTurn();
        hero.UpdateEffects();
        enemy.UpdateEffects();

        CheckBattleState();
        UpdateHealthBars();

        if (enemy.Health > 0)
            StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        yield return new WaitForSeconds(turnDelay);
        EnemyTurn();
    }

    void EnemyTurn()
    {
        if (enemy.IsStunned)
        {
            ShowStatus(enemy, "está aturdido y pierde el turno");
            PlaySFX(stunSFX);
            enemy.IsStunned = false;
            hero.UpdateEffects();
            enemy.UpdateEffects();
            StartCoroutine(HeroTurnCoroutine());
            return;
        }

        heroTurn = false;
        ShowStatus(enemy, "toma su turno...");

        float decision = Random.value;

        if (enemy.Health <= 20 && decision < 0.2f)
        {
            turnManager.AddCommand(new HealCommand(enemy, 10));
            ShowStatus(enemy, "se cura");
            PlaySFX(healSFX);
        }
        else if (decision < 0.4f)
        {
            turnManager.AddCommand(new AttackCommand(enemy, hero, 15));
            ShowStatus(enemy, $"ataca a {hero.Name}");
            PlaySFX(attackSFX);
            PlaySFX(heroHitSFX);
        }
        else if (decision < 0.7f)
        {
            hero.ApplyStatus(new PoisonEffect(3, 5));
            ShowStatus(enemy, $"envenena a {hero.Name}");
            PlaySFX(poisonSFX);
            PlaySFX(heroHitSFX);
        }
        else
        {
            hero.ApplyStatus(new StunEffect(1));
            ShowStatus(enemy, $"aturde a {hero.Name}");
            PlaySFX(stunSFX);
            PlaySFX(heroHitSFX);
        }

        turnManager.ExecuteTurn();
        hero.UpdateEffects();
        enemy.UpdateEffects();

        CheckBattleState();
        UpdateHealthBars();

        StartCoroutine(HeroTurnCoroutine());
    }

    private IEnumerator HeroTurnCoroutine()
    {
        yield return new WaitForSeconds(turnDelay);
        if (hero.Health > 0)
        {
            ShowStatus(hero, "elige una acción");
            heroTurn = true;
            EnableButtons();
        }
    }

    void CheckBattleState()
    {
        if (hero.Health <= 0)
        {
            EndBattle("¡Derrota!");
        }
        else if (enemy.Health <= 0)
        {
            EndBattle("¡Victoria!");
        }

        UpdateHealthBars();
    }

    void EndBattle(string message)
    {
        statusText.text = message;
        DisableButtons();

        endMessageText.text = message;
        endScreen.SetActive(true);

        // Detener música de fondo
        if (backgroundMusic != null && backgroundMusic.isPlaying)
            backgroundMusic.Stop();

        // Música de victoria o derrota
        if (sfxSource != null)
        {
            if (message.Contains("Victoria") && victoryMusic != null)
            {
                sfxSource.clip = victoryMusic;
                sfxSource.Play();
            }
            else if (message.Contains("Derrota") && defeatMusic != null)
            {
                sfxSource.clip = defeatMusic;
                sfxSource.Play();
            }
        }
    }

    void DisableButtons()
    {
        attackButton.interactable = false;
        defendButton.interactable = false;
        healButton.interactable = false;
        if (fortifyButton != null) fortifyButton.interactable = false;
        if (stunButton != null) stunButton.interactable = false;
    }

    void EnableButtons()
    {
        attackButton.interactable = true;
        defendButton.interactable = true;
        healButton.interactable = true;
        if (fortifyButton != null) fortifyButton.interactable = true;
        if (stunButton != null) stunButton.interactable = true;
    }

    void RestartBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        EnableButtons();
    }

    void QuitGame()
    {
        SceneManager.LoadScene(1);
    }

    void ShowStatus(Character character, string message)
    {
        statusText.text = $"¡{character.Name} {message}!";
    }

    void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
