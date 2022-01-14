using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D grid;
    [SerializeField] GameObject pinkFood;
    [SerializeField] GameObject[] redFoods;
    [SerializeField] GameObject blueFood;

    [SerializeField] int minRandom = 3;
    [SerializeField] int maxRandom = 5;

    int foodEaten = 0;
    int foodToBeEaten = 0;

    int foodTypeCount = 4;

    private void Start() 
    {
        RandomizePosition();
        foodToBeEaten = Random.Range(minRandom, maxRandom + 1);
    }

    public void RandomizePosition()
    {
        Bounds bounds = this.grid.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            foodEaten++;

            if (foodEaten == foodToBeEaten)
            {
                foodEaten = 0;
                foodToBeEaten = Random.Range(minRandom, maxRandom);
                
                int type = Random.Range(0, foodTypeCount);
                Debug.Log(type);
                
                if (type < 2)
                {
                    pinkFood.SetActive(true);
                }
                else if (type == 3)
                {
                    for (int i = 0; i < redFoods.Length; i++)
                    {
                        redFoods[i].SetActive(true);
                    }
                }
                else if (type == 2)
                {
                    blueFood.SetActive(true);
                }
            }
        }
        RandomizePosition();
    }
}
