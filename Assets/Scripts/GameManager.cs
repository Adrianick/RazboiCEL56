using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int cardsNumber = 52;

    public Text score1;
    public Text score2;
    public Text card1;
    public Text card2;

    private int player1Score;
    private int player2Score;

    void Start()
    {
        player1Score = cardsNumber / 2;
        player2Score = cardsNumber / 2;

        card1.text = Random.Range(1, 13).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            player1Score++;
            score1.text = player1Score.ToString();
            score2.text = player2Score.ToString();


            print("Am apasat Space!");
        }
    }
}