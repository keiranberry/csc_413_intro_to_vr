using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class controls the score menu
/// </summary>
public class ScoreMenu : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject startButton;

    /// <summary>
    /// Called every frame to update the score
    /// menu with current score and time
    /// </summary>
    private void Update()
    {
        scoreText.text = "Score: " + GameManager.Instance.score;
        timerText.text = "Time: " + GameManager.Instance.GetFormattedTime();
    }

    public void StartButtonSelect()
    {
        Image buttonImage = startButton.GetComponent<Image>();
        buttonImage.color = Color.green;
        Invoke(nameof(ResetButtonColor), 0.7f);
        GameManager.Instance.EndGame();
        GameManager.Instance.StartGame();
    }

    public void ResetButtonColor()
    {
        startButton.GetComponent<Image>().color = Color.white;
    }
}
