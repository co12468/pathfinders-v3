using UnityEngine;
using System.Collections.Generic;

public class BowlingGameManager : MonoBehaviour
{
    public string playerName = "Player"; // Default player name
    private List<int> rolls = new List<int>(); // Store pin counts for each roll

    // Method to set player name
    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    // Method to add a roll to the list
    public void Roll(int pins)
    {
        rolls.Add(pins);
        // Add logic to calculate scores and update UI here
    }

    // Method to get player name
    public string GetPlayerName()
    {
        return playerName;
    }

    // Method to get scores for frames
    public int[] GetScores()
    {
        List<int> frameScores = new List<int>();
        int currentScore = 0;
        int frameIndex = 0;

        for (int i = 0; i < rolls.Count && frameIndex < 21; i++)
        {
            currentScore = rolls[i];

            // Add the current score to the frame scores for each roll
            if (i % 2 != 0)
            {
                currentScore -= rolls[i - 1];
            }
            frameScores.Add(currentScore);

            if (IsStrike(i))
            {
                // If it's a strike, calculate and add the bonus
                // currentScore += StrikeBonus(i);
                if (frameIndex < 20) // For the last frame, no extra rolls
                    frameIndex++;
            }
            else if (!IsStrike(i) && i % 2 == 1)
            {
                if (IsSpare(i))
                {
                    // If it's a spare, calculate and add the bonus
                    // currentScore += SpareBonus(i);
                }
                currentScore = 0;
                frameIndex++;
            }
            else if (!IsStrike(i) && i % 2 == 0 && i > 0)
            {
                if (IsSpare(i - 1))
                {
                    // If it's a spare, calculate and add the bonus
                    // currentScore += SpareBonus(i - 1);
                }
            }
        }

        // Fill remaining frames with 0 scores if the game hasn't ended
        while (frameScores.Count < 21)
        {
            frameScores.Add(-1);
        }

        return frameScores.ToArray();
    }

    // Method to get the total score
    public int GetTotalScore()
    {
        int total = 0;
        foreach (int score in rolls)
        {
            total += score;
        }
        return total;
    }

    // Helper method to check for a strike
    private bool IsStrike(int rollIndex)
    {
        return rolls[rollIndex] == 10;
    }

    // Helper method to check for a spare
    private bool IsSpare(int rollIndex)
    {
        return rollIndex % 2 == 1 && rolls[rollIndex - 1] + rolls[rollIndex] == 10;
    }

    // Calculate bonus for a strike
    private int StrikeBonus(int rollIndex)
    {
        return rolls[rollIndex + 1] + rolls[rollIndex + 2];
    }

    // Calculate bonus for a spare
    private int SpareBonus(int rollIndex)
    {
        return rolls[rollIndex + 1];
    }
}
