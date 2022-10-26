using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour
{
    public string number;  // from A, 2, 3... J, Q, K
    public string color;   // sp: spade , he: heart, di: diamond, cl: club
    //public Dictionary<string, int> numberDir = new Dictionary<string, int>
    //{
    //    {"01", 1},
    //    {"02", 2 },
    //    {"03", 3},
    //    {"04", 4},
    //    {"05", 5},
    //    {"06", 6},
    //    {"07", 7 },
    //    {"08", 8 },
    //      ...
    //};
    
    // Start is called before the first frame update
    void Start()
    {
        WriteCardInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void WriteCardInfo()
    {
        string cardInfo = gameObject.name;
        number = cardInfo.Substring(cardInfo.Length - 12, 2);
        color = cardInfo.Substring(18, 2);
        
        Debug.Log("The card information is: " + number + " " + color);
    }
}
