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

    public GameObject ringZero_;
    public GameObject ringOne_;
    public GameObject ringTwo_;
    public GameObject ringThree_;
    public GameObject ringFour_;

    [SerializeField] private int[] OneSlotRing = new int[5];
    [SerializeField] private int[] TwoSlotRing = new int[4];

    int ringSelected;
    // Start is called before the first frame update
    void Start()
    {
        OneSlotRing[0] = 2;
        TwoSlotRing[0] = 6;

        OneSlotRing[1] = 0;
        TwoSlotRing[1] = 4;

        OneSlotRing[2] = 2;
        TwoSlotRing[2] = 6;

        OneSlotRing[3] = 4;
        TwoSlotRing[3] = 0;


        ringZero[OneSlotRing[0]] = ringFour[0];

        ringOne[OneSlotRing[1]] = ringFour[1];
        ringOne[TwoSlotRing[1]] = ringFour[2];

        ringTwo[OneSlotRing[2]] = ringFour[3];
        ringTwo[TwoSlotRing[2]] = ringFour[4];

        ringThree[OneSlotRing[3]] = ringFour[5];
        ringThree[TwoSlotRing[3]] = ringFour[6];

        ringZero[TwoSlotRing[0]] = ringFour[7];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ringSelected = 0;
            ringZero[OneSlotRing[0]] = ringFour[0];
            ringZero[TwoSlotRing[0]] = ringFour[7];

            ringFour[0].transform.parent = ringZero_.transform;
            ringFour[7].transform.parent = ringZero_.transform;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ringSelected = 1;
            ringOne[OneSlotRing[1]] = ringFour[1];
            ringOne[TwoSlotRing[1]] = ringFour[2];

            ringFour[1].transform.parent = ringOne_.transform;
            ringFour[2].transform.parent = ringOne_.transform;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ringSelected = 2;

            ringTwo[OneSlotRing[2]] = ringFour[3];
            ringTwo[TwoSlotRing[2]] = ringFour[4];

            ringFour[3].transform.parent = ringTwo_.transform;
            ringFour[4].transform.parent = ringTwo_.transform;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ringSelected = 3;
            ringThree[OneSlotRing[3]] = ringFour[5];
            ringThree[TwoSlotRing[3]] = ringFour[6];

            ringFour[5].transform.parent = ringThree_.transform;
            ringFour[6].transform.parent = ringThree_.transform;
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
            ringFour[0] = ringZero[OneSlotRing[0]];

            ringFour[1] = ringOne[OneSlotRing[1]];
            ringFour[2] = ringOne[TwoSlotRing[1]];

            ringFour[3] = ringTwo[OneSlotRing[2]];
            ringFour[4] = ringTwo[TwoSlotRing[2]];

            ringFour[5] = ringThree[OneSlotRing[3]];
            ringFour[6] = ringThree[TwoSlotRing[3]];

            ringFour[7] = ringZero[TwoSlotRing[0]];

            ////// MAKE CHIL OF THE 4 RING
            ringFour[0].transform.parent = ringFour_.transform;

            ringFour[1].transform.parent = ringFour_.transform;
            ringFour[2].transform.parent = ringFour_.transform;


            ringFour[3].transform.parent = ringFour_.transform;
            ringFour[4].transform.parent = ringFour_.transform;


            ringFour[5].transform.parent = ringFour_.transform;
            ringFour[6].transform.parent = ringFour_.transform;


            ringFour[7].transform.parent = ringFour_.transform;


            if (Input.GetAxis("Mouse ScrollWheel") > 0)  //Gira derecha
            {
                OneSlotRing[4]++;
                if (OneSlotRing[4] > 7) { OneSlotRing[4] = 0; }



                ringFour_.transform.Rotate(0, 0, -90, Space.Self);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                OneSlotRing[4]--;
                if (OneSlotRing[4] < 0) { OneSlotRing[4] = 7; }


                ringFour_.transform.Rotate(0, 0, 90, Space.Self);
            }
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Gira derecha
            {
                OneSlotRing[ring]++;
                if (OneSlotRing[ring] > 7) { OneSlotRing[ring] = 0; }

                TwoSlotRing[ring]++;
                if (TwoSlotRing[ring] > 7) { TwoSlotRing[ring] = 0; }

                gameObject.transform.GetChild(ring).Rotate(0, 0, -45, Space.Self);



            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                OneSlotRing[ring]--;
                if (OneSlotRing[ring] < 0) { OneSlotRing[ring] = 7; }

                TwoSlotRing[ring]--;
                if (TwoSlotRing[ring] < 0) { TwoSlotRing[ring] = 7; }

                gameObject.transform.GetChild(ring).Rotate(0, 0, 45, Space.Self);

                


            }
        }
        
    }
}
