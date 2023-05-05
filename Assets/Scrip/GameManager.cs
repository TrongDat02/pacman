using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Ghost[] ghost;
	public Pacman pacman;
	public Transform pellets;

	public AudioSource opening;
	public AudioSource powerEat;
	public AudioSource victory;

	public Text scoreText;
	public Text highScoreText;
	public Text livesText;

	public Score gameOverScreen;

	public int ghostMuti { get; private set; } = 1;
	public int score { get; private set; }
	public int highScore = 0;
	public int lives { get; private set; }
	private void Start()
	{
		highScore = PlayerPrefs.GetInt("HighScore", 0);
		highScoreText.text = "HighScore: " + this.highScore.ToString();
		scoreText.text = "Score: " + this.score.ToString();
		livesText.text = "Lives: " + this.lives.ToString();
		NewGame();
	}
    private void Update()
    {
        /*if (this.lives <=0 && Input.anyKeyDown)
        {
			NewGame();
        }*/
		scoreText.text = "Score: " + this.score.ToString();
		livesText.text = "Lives: " + this.lives.ToString();
		if (this.highScore < this.score)
        {
			PlayerPrefs.SetInt("HighScore", score);
        }
	}
    public void NewGame()
	{
		SetScore(0);
		SetLives(3);
		NewRound();
	}
	private void NewRound()
	{
		foreach (Transform pellet in this.pellets)
		{
			pellet.gameObject.SetActive(true);
		}

		ResetState();
	}
	private void ResetState()
	{
		ResetGhostMuti();
		for(int i=0; i<this.ghost.Length; i++)
		{
			this.ghost[i].ResetState();
		}
		this.pacman.ResetState();
		opening.Play();
	}
	private void GameOver()
    {
		gameOverScreen.Setup(this.score);
		for (int i = 0; i < this.ghost.Length; i++)
		{
			this.ghost[i].gameObject.SetActive(false);
		}
		this.pacman.gameObject.SetActive(false);
	}
	private void SetScore(int score)
	{
		this.score = score;
	}
	private void SetLives(int lives)
	{
		this.lives = lives;
	}
	public void GhostEaten(Ghost ghost)
    {
		SetScore(this.score + (ghost.points * this.ghostMuti));
		this.ghostMuti++;
    }
	public void PacmanEaten()
    {
		this.pacman.gameObject.SetActive(false);
		SetLives(this.lives - 1);

		if(this.lives > 0)
        {
			Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
			GameOver();
        }
    }
	public void PelletEaten(Pellet pellet)
    {
		pellet.gameObject.SetActive(false);
		SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
			victory.Play();
			this.pacman.gameObject.SetActive(false);
			Invoke(nameof(NewRound), 6.0f);
        }
    }
	public void PowerPelletEaten(PowerPellet pellet)
    {
		powerEat.Play();
		for(int i=0; i < this.ghost.Length; i++)
        {
			this.ghost[i].frightened.Enable(pellet.duration);
        }

		PelletEaten(pellet);
		CancelInvoke();
		Invoke(nameof(ResetGhostMuti), pellet.duration);
	}
	private bool HasRemainingPellets()
    {
		foreach(Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
				return true;
            }
        }
		return false;
    }
    private void ResetGhostMuti()
    {
		this.ghostMuti = 1;
    }

/*    private void Ui()
    {
		
	}*/
}
