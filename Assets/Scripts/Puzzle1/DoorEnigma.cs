using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnigma : MonoBehaviour
{
    public GameObject[] ringZero =new GameObject[7];
    public GameObject[] ringOne = new GameObject[7];
    public GameObject[] ringTwo = new GameObject[7];
    public GameObject[] ringThree = new GameObject[7];

    public GameObject[] ringFour = new GameObject[7];


    [SerializeField] private int[] slotRing = new int[4];

    int ringSelected;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ringSelected = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ringSelected = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ringSelected = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ringSelected = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ringSelected = 4;
        }

        Rotate(ringSelected);
    }

    private void Rotate(int ring)
    {
        if (ring == 4)
        {
            if (slotRing[0] > 4)
            {
                ringFour[0] = ringZero[slotRing[0] +2];

                ringFour[1] = ringOne[slotRing[1]];
                ringFour[2] = ringOne[slotRing[1] + 4];

                ringFour[2] = ringTwo[slotRing[2] + 2];
                ringFour[3] = ringTwo[slotRing[2] + 4];

                ringFour[5] = ringThree[slotRing[3] + 6];
                ringFour[6] = ringThree[slotRing[3]];

                ringFour[7] = ringZero[slotRing[0] + 6];
            }
            ringFour[0] = ringZero[slotRing[0] + 2];
            ringFour[1] = ringOne[slotRing[1]];
            ringFour[2] = ringOne[slotRing[1] + 4];
            ringFour[3] = ringTwo[slotRing[2] + 2];
            ringFour[4] = ringTwo[slotRing[2] + 6];
            ringFour[5] = ringThree[slotRing[3] + 4];
            ringFour[6] = ringThree[slotRing[3]];
            ringFour[7] = ringZero[slotRing[0] + 6];

            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Gira derecha
            {
                //ringFour[0] = ringZero[2 - slotRing[0]];
                //ringFour[1] = ringOne[0 - slotRing[1]];
                //ringFour[2] = ringOne[4 - slotRing[1]];
                //ringFour[3] = ringTwo[2 - slotRing[2]];
                //ringFour[4] = ringTwo[6 - slotRing[2]];
                //ringFour[5] = ringThree[4 - slotRing[3]];
                //ringFour[6] = ringThree[0 - slotRing[3]];
                //ringFour[7] = ringZero[6 - slotRing[0]];


                //ringZero[slotRing[0] + 2].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringZero[slotRing[0] + 6].transform.parent = gameObject.transform.GetChild(4).transform;

                //ringOne[slotRing[1]].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringOne[slotRing[1] + 4].transform.parent = gameObject.transform.GetChild(4).transform;

                //ringTwo[slotRing[2] + 2].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringTwo[slotRing[2] + 6].transform.parent = gameObject.transform.GetChild(4).transform;

                //ringThree[slotRing[3]].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringThree[slotRing[3] + 4].transform.parent = gameObject.transform.GetChild(4).transform;

                //gameObject.transform.GetChild(ring).Rotate(0, 0, 90, Space.Self);
                //slotRing[ring]++;
                //if (slotRing[ring] > 7) { slotRing[ring] = 0; }

                //ringFour[-slotRing[4]].transform.parent = ringZero[2]

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                //ringZero[slotRing[0] + 2].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringZero[slotRing[0] + 6].transform.parent = gameObject.transform.GetChild(4).transform;

                //ringOne[slotRing[1]].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringOne[slotRing[1] + 4].transform.parent = gameObject.transform.GetChild(4).transform;

                //ringTwo[slotRing[2] + 2].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringTwo[slotRing[2] + 6].transform.parent = gameObject.transform.GetChild(4).transform;

                //ringThree[slotRing[3]].transform.parent = gameObject.transform.GetChild(4).transform;
                //ringThree[slotRing[3] + 4].transform.parent = gameObject.transform.GetChild(4).transform;

                //gameObject.transform.GetChild(ring).Rotate(0, 0, -90, Space.Self);
                //slotRing[ring]++;
                //if (slotRing[ring] > 7) { slotRing[ring] = 0; }

                

                
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Gira derecha
            {
                slotRing[ring]++;
                if (slotRing[ring] > 7) { slotRing[ring] = 0; }
                gameObject.transform.GetChild(ring).Rotate(0, 0, 45, Space.Self);


            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                slotRing[ring]--;
                if (slotRing[ring] < 0) { slotRing[ring] = 7; }
                gameObject.transform.GetChild(ring).Rotate(0, 0, -45, Space.Self);


            }
        }
        
    }
}
