using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeScript : MonoBehaviour
{

    [SerializeField] Transform segmentObject;
    [SerializeField] BoxCollider2D grid;
    [SerializeField] int snakeLength = 1;
    [SerializeField] float invisibleTime = 10f;

    [SerializeField] TextMeshProUGUI green;
    [SerializeField] TextMeshProUGUI red;
    [SerializeField] TextMeshProUGUI pink;
    [SerializeField] TextMeshProUGUI blue;
    [SerializeField] TextMeshProUGUI highScoreText;

    int redCount = 0;
    int pinkCount = 0;
    int blueCount = 0;
    int highScore = 0;

    Vector2 direction = Vector2.right;
    Vector2 previousDirection = Vector2.right;
    private List<Transform> segmentList;
    float time = 0;
    bool isInvisible = false;
    bool madeVisible = false;

    private void Start() 
    {
        segmentList = new List<Transform>();
        segmentList.Add(this.transform);
        initiliazeSnake();
        redCount = 0;
        pinkCount = 0;
        blueCount = 0;
        highScore = snakeLength;
        UpdateGreen();
        UpdateRed();
        UpdatePink();
        UpdateBlue();
        UpdateHighScore();
    }

    private void Update() 
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && previousDirection != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && previousDirection != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && previousDirection != Vector2.left)
        {
            direction = Vector2.right;
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && previousDirection != Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (isInvisible)
        {
            time += Time.deltaTime;
        }

        if (time > invisibleTime - 1 && !madeVisible)
        {
            SetInvisibility(false);
            madeVisible = true;
        }

        if (time > invisibleTime)
        {
            time = 0;
            isInvisible = false;
            madeVisible = false;
        }
        
    }

    private void FixedUpdate() 
    {
        for (int i = segmentList.Count - 1; i > 0; i--)
        {
            segmentList[i].position = segmentList[i - 1].position;           
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y,
            0f
        );

        Bounds bounds = this.grid.bounds;

        if (Mathf.Round(transform.position.x  - bounds.min.x) < 0)
        {
            this.transform.position = new Vector3(
                bounds.max.x,
                this.transform.position.y,
                0f
            );
        }
        if (Mathf.Round(transform.position.x - bounds.max.x) > 0)
        {
            this.transform.position = new Vector3(
                bounds.min.x,
                this.transform.position.y,
                0f
            );
        }
        if (Mathf.Round(transform.position.y - bounds.min.y) < 0)
        {
            this.transform.position = new Vector3(
                this.transform.position.x,
                bounds.max.y,
                0f
            );
        }
        if (Mathf.Round(transform.position.y - bounds.max.y) > 0)
        {
            this.transform.position = new Vector3(
                this.transform.position.x,
                bounds.min.y,
                0f
            );
        }

        previousDirection = direction;
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentObject);
        segment.position = segmentList[segmentList.Count - 1].position;
        if (isInvisible && !madeVisible)
        {
            Color tempColor = GetComponent<SpriteRenderer>().color;
            tempColor.a = 0.3f;
            segment.gameObject.GetComponent<SpriteRenderer>().color = tempColor;
        }
        segmentList.Add(segment);
        UpdateGreen();
    }

    private void RestartGame()
    {
        for (int i = 1; i < segmentList.Count; i++)
        {
            Destroy(segmentList[i].gameObject);
        }

        segmentList.Clear();
        segmentList.Add(this.transform);

        this.transform.position = new Vector3 (-14, 0, 0);
        initiliazeSnake();
        time = 0;

        redCount = 0;
        pinkCount = 0;
        blueCount = 0;
        UpdateGreen();
        UpdateRed();
        UpdatePink();
        UpdateBlue();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Food")
        {
            Grow();
            redCount++;
            UpdateRed();
        }
        else if (other.tag == "PinkFood")
        {
            Grow(); Grow(); Grow(); Grow(); Grow();
            pinkCount++;
            UpdatePink();
        }
        else if (other.tag == "BlueFood")
        {
            isInvisible = true;
            SetInvisibility(true);
            time = 0;
            blueCount++;
            UpdateBlue();
        }
        else if (other.tag == "Segment" && !isInvisible)
        {
            RestartGame();
        }
    }

    private void SetInvisibility(bool isInvisible)
    {
        float transparency;
        if (isInvisible)
        {
            transparency = 0.3f;
        }
        else 
        {
            transparency = 1f;
        }

        Color tempColor = GetComponent<SpriteRenderer>().color;
        tempColor.a = transparency;

        for (int i = 0; i < segmentList.Count; i++)
        {
            segmentList[i].gameObject.GetComponent<SpriteRenderer>().color = tempColor;
        }
    }

    private void initiliazeSnake()
    {
        for (int i = 1; i < snakeLength; i++)
        {
            segmentList.Add(Instantiate(this.segmentObject));
        }
    }

    private void UpdateGreen()
    {
        int greenCount = segmentList.Count;
        green.text = greenCount.ToString();
        if (greenCount > highScore)
        {
            highScore = greenCount;
            UpdateHighScore();
        }
    }

    private void UpdateRed()
    {
        red.text = redCount.ToString();
    }

    private void UpdatePink()
    {
        pink.text = pinkCount.ToString();
    }

    private void UpdateBlue()
    {
        blue.text = blueCount.ToString();
    }

    private void UpdateHighScore()
    {
        highScoreText.text = "High Score:\n" + highScore.ToString();
    }
}
