using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Config Relative Player")]
    public float offSetX = 3f;
    public float offSetY = 3f;
    public float smooth = 0.5f;

    [Header("Config Limite Camera")]
    public float limitedUp = 2f;
    public float limitedDown = 0f;
    public float limitedLeft = 0f;
    public float limitedRight = 100f;

    [Header("Config Duration Teleport")]
    public float elapsedTime = 0f;
    public float duration = 0.5f;
    public float speedTeleport = 5f;

    private Transform player;
    private float playerX;
    private float playerY;

    private float defaultSmooth = 0f;
    private float totalMultiply = 1.02f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>()?.transform;
        defaultSmooth = smooth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player != null)
        {
            playerX = Mathf.Clamp(player.position.x + offSetX, limitedLeft, limitedRight);
            playerY = Mathf.Clamp(player.position.y + offSetY, limitedDown, limitedUp);

            transform.position = Vector3.Lerp(transform.position, new Vector3(playerX, playerY, transform.position.z), smooth);
        }
    }

    public void SmoothMoveTo()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothMoveCoroutine());    
    }

    private IEnumerator SmoothMoveCoroutine()
    {
        float initialSmooth = speedTeleport / 1000;
        smooth = initialSmooth;
        while (elapsedTime < duration)
        {       
            elapsedTime += Time.deltaTime;
            smooth *= totalMultiply;
            yield return null;
        }
        smooth = defaultSmooth;
        elapsedTime = 0f;
    }
}
