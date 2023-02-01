using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreChange : MonoBehaviour
{
    [HideInInspector] public int score;
    [SerializeField] Text scoreText;

    Animator animator;
    Collider2D collider2D;
    SpriteRenderer body;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        body = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            scoreText.text = score.ToString();

            if (PlayerPrefs.HasKey("PlayerScore"))
            {
                int currentScore = PlayerPrefs.GetInt("PlayerScore");
                currentScore += score;
                PlayerPrefs.SetInt("PlayerScore", currentScore);
            }
            else
            {
                PlayerPrefs.SetInt("PlayerScore", score);
            }

            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        animator.Play("scoreDestroy");
        collider2D.enabled = false;

        yield return new WaitForSeconds(0.7f);

        scoreText.gameObject.SetActive(false);
        body.color = new Color(1f,1f,1f,1f);

        gameObject.SetActive(false);
    }
}