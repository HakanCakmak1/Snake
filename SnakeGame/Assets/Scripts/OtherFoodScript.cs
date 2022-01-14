using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherFoodScript : MonoBehaviour
{
    [SerializeField] BoxCollider2D grid;

    private void Awake() 
    {
        RandomizePosition();
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
            gameObject.SetActive(false);
        }
        else
        {
            RandomizePosition();
        }
    }

    private void OnEnable() 
    {
        RandomizePosition();
    }
}
