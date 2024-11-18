using System.Collections;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField]
    private Card cardTemplate;

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
            int randomCardCategory = Random.Range(0, numCardCategories);
            int randomCard = Random.Range(1, numCardCategories + 1);
            Vector3 cardSpawnPosition = new Vector3(randXPosition, transform.parent.position.y + 10,transform.parent.position.z);
            transform.position = cardSpawnPosition;
            Card spawnedCard = Instantiate(cardTemplate, transform.parent);
            spawnedCard.transform.position = cardSpawnPosition;
            spawnedCard.Setup(sprite: null, category: (Category)randomCardCategory, value: randomCard);
            yield return new WaitForSeconds(timeBewteenSpawnSec);
        }
    }

    void Start()
    {
        StartCoroutine(spawnCardLoop());
    }

}
