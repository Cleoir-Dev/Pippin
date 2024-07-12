using UnityEngine;
using UnityEngine.UI;

public class ControllerGame : MonoBehaviour
{
    private int score;
    public Text txtScore;
    public Sprite[] imagesLifes;
    public Image barLife;
    public GameObject hitPrefab;
    

    public void Points(int qtdPoint)
    {
        score += qtdPoint;
        txtScore.text = score.ToString();      
    }

    public void BarLifes(int healthLife)
    {
        barLife.sprite = imagesLifes[healthLife];
    }
}
