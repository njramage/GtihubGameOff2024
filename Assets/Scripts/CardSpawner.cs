using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cardTemplate;

    [SerializeField]
    private float timeBewteenSpawnSec;
    [SerializeField]
    private int numCardCategories;

    [SerializeField]
    private const float MAGIC_LEFT_NUMBER = 142;
    [SerializeField]
    private const float MAGIC_RIGHT_NUMBER = 135;
    private const float SCREEN_WIDTH = 1920;

    private IEnumerator spawnCardLoop()
    {
        while (true)
        {
            Vector3 maxPointBounds = Camera.main.ScreenToWorldPoint(new Vector3(SCREEN_WIDTH - MAGIC_RIGHT_NUMBER, 0, 0));
            Vector3 minPointBounds = Camera.main.ScreenToWorldPoint(new Vector3(MAGIC_LEFT_NUMBER, 0, 0));
            float randXPosition = Random.Range(minPointBounds.x, maxPointBounds.x);
            int randomCardCategory = Random.Range(1, numCardCategories);
            int randomCard = Random.Range(1, numCardCategories);
            Vector3 cardSpawnPosition = new Vector3(randXPosition, transform.parent.position.y + 10,transform.parent.position.z);
            transform.position = cardSpawnPosition;
            GameObject spawnedCard = Instantiate(cardTemplate, transform.parent);
            spawnedCard.transform.position = cardSpawnPosition;
            Card card = spawnedCard.GetComponent<Card>();
            card.Setup(sprite: null, category: (Category)randomCardCategory, value: randomCard);
            yield return new WaitForSeconds(timeBewteenSpawnSec);
        }
    }

    void Start()
    {
        StartCoroutine(spawnCardLoop());
    }

}
