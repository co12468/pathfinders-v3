using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Text playerNameText;
    public Text[] frameScoresText;
    public Text[] roundTotalText;

    private BowlingGameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<BowlingGameManager>();
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerNameText.text = gameManager.GetPlayerName(); // Update player name

        int[] scores = gameManager.GetScores(); // Get scores from the game manager

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == -1)
            {
                frameScoresText[i].text = " "; // Display empty spaces for top row
            }
            else if (scores[i] == 0)
            {
                frameScoresText[i].text = "-"; // Display dash for zero scores
            }
            else if (scores[i] == 10)
            {
                frameScoresText[i].text = "X"; // Display dash for zero scores
            }
            else
            {
                frameScoresText[i].text = scores[i].ToString(); // Display the score
            }

            // Calculate and display the round totals after every two frames
            int roundCounter = (i + 1) / 2; // Calculate the round number
            if ((i + 1) % 2 == 0 && scores[i] != -1) // Check if it's the end of a round
            {
                roundTotalText[roundCounter - 1].text = CalculateRoundTotal(scores, i).ToString(); // Update round total
            }
        }
    }

    private int CalculateRoundTotal(int[] scores, int currentIndex)
    {
        int total = 0;
        for (int i = 0; i <= currentIndex; i++)
        {
            if (scores[i] != -1)
            {
                total += scores[i];
            }
        }
        return total;
    }

    // Method to update the frame scores based on fallen pins
    public void UpdateFrameScores(int fallenPins)
    {
        gameManager.Roll(fallenPins); // Update game manager with the roll
        UpdateUI(); // Update the UI after each roll
    }
}
