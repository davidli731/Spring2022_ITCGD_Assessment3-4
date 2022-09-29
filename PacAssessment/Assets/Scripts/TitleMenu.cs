using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    private IEnumerator swapSpriteCoroutine;
    private int currentSpriteValue;
    [SerializeField] private Image ghostImage;
    [SerializeField] private Sprite[] ghostSprites;

    // Start is called before the first frame update
    void Start()
    {
        currentSpriteValue = 0;

        swapSpriteCoroutine = swapSprite();
        StartCoroutine(swapSpriteCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (swapSpriteCoroutine == null)
        {
            swapSpriteCoroutine = swapSprite();
            StartCoroutine(swapSpriteCoroutine);
        }
    }

    private IEnumerator swapSprite()
    {
        yield return new WaitForSeconds(1.5f);

        currentSpriteValue++;

        if (currentSpriteValue > 2)
        {
            currentSpriteValue = 0;
        }

        ghostImage.sprite = ghostSprites[currentSpriteValue];

        StopCoroutine(swapSpriteCoroutine);
        swapSpriteCoroutine = null;
    }
}
