using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        SetCardsList();
        cardIndex = GetRandomCardIndex();
        CardsInit();
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(cardsList.Count);
            Instantiate<GameObject>(cardsList[0]);
        }
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
            Instantiate(cardsList[cardIndex[i]], positions[i], Quaternion.FromToRotation(Vector3.up, Vector3.down));
            //GameObject currCard = Instantiate<GameObject>(cardsList[cardIndex[i]]);
            //currCard.transform.SetPositionAndRotation(positions[i], Quaternion.FromToRotation(Vector3.up, Vector3.down));
        }


    }
}
