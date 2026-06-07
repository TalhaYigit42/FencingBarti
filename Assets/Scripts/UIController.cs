using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public int winsToWin = 3;
    public AudioClip battleMusic;

    private AudioSource musicSource;
    private int p1Score;
    private int p2Score;
    private Label p1Label;
    private Label p2Label;
    private VisualElement winScreen;
    private Label winLabel;
    private bool gameOver;

    void Awake()
    {
        Instance = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = battleMusic;
        musicSource.loop = true;
        float vol = AudioManager.Instance != null ? AudioManager.Instance.MusicVolume : 1f;
        musicSource.volume = vol * 0.6f;
        if (battleMusic != null)
            musicSource.Play();
    }

    void Start()
    {
        var doc = GetComponent<UIDocument>();
        if (doc == null) return;

        var root = doc.rootVisualElement;
        if (root == null) return;

        p1Label = root.Q<Label>("P1Label");
        p2Label = root.Q<Label>("P2Label");
        winScreen = root.Q<VisualElement>("WinScreen");
        winLabel = root.Q<Label>("WinLabel");

        if (winScreen != null)
            winScreen.style.display = DisplayStyle.None;

        UpdateLabels();
    }

    public void AddScore(int player)
    {
        if (gameOver) return;

        if (player == 1)
            p1Score++;
        else
            p2Score++;

        UpdateLabels();

        if (p1Score >= winsToWin || p2Score >= winsToWin)
            StartCoroutine(ShowWinner(player));
    }

    void UpdateLabels()
    {
        if (p1Label != null) p1Label.text = $"Player 1: {p1Score}";
        if (p2Label != null) p2Label.text = $"Player 2: {p2Score}";
    }

    IEnumerator ShowWinner(int player)
    {
        gameOver = true;

        if (winScreen != null)
        {
            winScreen.style.display = DisplayStyle.Flex;
            if (winLabel != null)
                winLabel.text = $"Player {player} Wins!";
        }

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SampleScene");
    }
}
