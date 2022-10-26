using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public GameObject cardSet;
    //public GameObject[] cards;
    public List<GameObject> cardsList;

    // for table size is  -0.2 < X < 0.2; Y = 1.3; -0.1 < Z < 0.1;
    // for camera <Projection = Orthographic> ; <size = 0.2>
    public int col = 2;     // for stage 1
    public int row = 2;     // for stage 1
    private List<int> cardIndex = new List<int>();

    public List<GameObject> currCards = new List<GameObject>();

    public GameObject previousCard;

    public int totalTime;

    public int score;

    public TMP_Text scoreUI;
    public TMP_Text time;

    // Start is called before the first frame update
    void Start()
    {
        SetCardsList();
        cardIndex = GetRandomCardIndex();
        CardsInit();

        StartCoroutine(TimeCount());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCardsList()
    {
        //Debug.Log("Setting Card List.");
        for (int i=0; i<52; i++)
        {
            cardsList.Add(cardSet.transform.GetChild(i).gameObject);
        }
        //Debug.Log("Card List has been set"+ "total " + cardsList.Count + " cards, and the last card is " + cardsList[cardsList.Count-1].name);
    }

    private List<int> GetRandomCardIndex()
    {
        Debug.Log("Choosing cards randomly...");
        int numCards = col * row / 2;
        List<int> res = new List<int>();
        for(int i = 0; i<numCards; i++)
        {
            int currNumber = Random.Range(0, 52);
            if(res.IndexOf(currNumber) == -1)
            {
                res.Add(currNumber);
            }
            else
            {
                i--;
            }
        }
        Debug.Log(res.Count);
        return res;        
    }

    private void CardsInit()
    {
        Debug.Log("Setting the transform of the cards. ");
        // Every card is used twice.
        cardIndex.AddRange(cardIndex);
        // Randomly sort the list.
        // ...

        // Get all positions of each card.
        List<Vector3> positions = new List<Vector3>();
        for(int i=0; i<col; i++)
        {
            for(int j=0; j<row; j++)
            {
                positions.Add(new Vector3(-0.2f + 0.4f / (col - 1) * i, 1.3f, -0.1f + 0.2f / (row - 1) * j));
            }
        }

        // Instantiate all cards and set position and rotation
        for (int i = 0; i<cardIndex.Count; i++)
        {
            currCards.Add(Instantiate(cardsList[cardIndex[i]], positions[i], Quaternion.FromToRotation(Vector3.up, Vector3.down)));
            //GameObject currCard = Instantiate<GameObject>(cardsList[cardIndex[i]]);
            //currCard.transform.SetPositionAndRotation(positions[i], Quaternion.FromToRotation(Vector3.up, Vector3.down));
        }


    }

    public void CheckCurrentState(GameObject currentCard)
    {
        Debug.Log("Checking Current State...");
        // check if it is the second click.
        int notOpen = 0;
        bool isSecondClick = true;
        foreach(GameObject card in currCards)
        {
            if (card.GetComponent<CardControl>().open)
                isSecondClick = !isSecondClick;
            else
                notOpen++;
        }
        if (notOpen == 0)
            StartCoroutine(NextStage());

        // GameObject previousCard = new GameObject();

        

        if(!isSecondClick)
        {
            previousCard = currentCard;
            //Debug.Log("Get information of previous Card: " + previousCard.name);
        }
        else
        {
            // Check if this two cards are the same one. (same number and same color.)
            if(previousCard.GetComponent<CardControl>().number == currentCard.GetComponent<CardControl>().number &&
                previousCard.GetComponent<CardControl>().color == currentCard.GetComponent<CardControl>().color)
            {
                // don't do anything.
                score += 5;
                scoreUI.SetText("Score: " + score);
            }
            else
            {
                Debug.Log("Wrong!");
                StartCoroutine(Wait(1, currentCard));
                //previousCard.transform.Rotate(Vector3.left, 180);
                //currentCard.transform.Rotate(Vector3.left, 180);
            }
        }

    }
    IEnumerator Wait(int seconds, GameObject currentCard)
    {
        yield return new WaitForSeconds(seconds);
        previousCard.transform.Rotate(Vector3.left, 180);
        previousCard.GetComponent<CardControl>().open = false;
        currentCard.transform.Rotate(Vector3.left, 180);
        currentCard.GetComponent<CardControl>().open = false;
    }

    IEnumerator TimeCount()
    {
        // set Time text to Total Time
        while (totalTime > 0)
        {
            time.SetText("Time: " + totalTime);
            yield return new WaitForSeconds(1);
            totalTime--;
        }
        EndGame();
        yield break;
    }
    public void EndGame()
    {

    }
    IEnumerator NextStage()
    {
        yield return new WaitForSeconds(1);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < 2)
            SceneManager.LoadScene(sceneIndex + 1);
        else
            EndGame();
    }
}
