using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerFade : MonoBehaviour
{
    public static ControllerFade instanceFade;
    public Image imageFade;
    public Color colorInitial;
    public Color colorFinal;
    public float durationFade;
    public bool isFade;
    private float _time;

    private void Awake()
    {
       instanceFade = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitialFade());
    }

    private IEnumerator InitialFade()
    {
        isFade = true;
        _time = 0f;
        while (_time <= durationFade)
        {
            imageFade.color = Color.Lerp(colorInitial, colorFinal, _time / durationFade);
            _time += Time.deltaTime;
            yield return null;
        }

        isFade = false;
    }
}
