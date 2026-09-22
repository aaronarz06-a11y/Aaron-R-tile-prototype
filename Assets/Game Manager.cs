using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;

    //ends game when the player reaches the exit
    public void WinGame()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        Debug.Log("YOU WIN!");
    }

    //ends the game when the player loses all health
    public void LoseGame()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        Debug.Log("GAME OVER!");
    }
}