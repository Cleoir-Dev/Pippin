using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class ControllerGame : MonoBehaviour
{
    private int score;
    public int scoreStar;
    public Text txtScore;
    public Text txtStar;
    public Sprite[] imagesLifes;
    public Image barLife;
    public GameObject hitPrefab;
    

    public void Points(int qtdPoint)
    {
        score += qtdPoint;
        txtScore.text = score.ToString();      
    }

    public int GetScore()
    {
        return score;
    }

    public void BarLifes(int healthLife)
    {
        barLife.sprite = imagesLifes[healthLife];
    }

    public void BarStars(int star)
    {
        scoreStar += star;
        txtStar.text = scoreStar.ToString();
    }
}
