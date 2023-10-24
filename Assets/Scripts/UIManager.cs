using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI gameOver;
    [SerializeField] private TextMeshProUGUI restart;
    
    [SerializeField] private Sprite[] liveSprites;

    [SerializeField] private Image livesImage;

    private GameManager _gameManager;
    
    void Start() {
        score.text = "Score: " + 0;
        gameOver.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!_gameManager) 
            Debug.Log("The Game Manager is Null!");
    }

    public void UpdateScore(int playerScore) {
        score.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives) {
        livesImage.sprite = liveSprites[currentLives];
        if (currentLives == 0) {
            GameOverSequence();
        }
    }

    void GameOverSequence() {
        _gameManager.GameOver();
        gameOver.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine() {
        while (true) {
            gameOver.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            gameOver.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
