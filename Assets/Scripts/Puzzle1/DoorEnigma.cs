using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DoorEnigma : MonoBehaviour, IInteractable
{
    /*
    public GameObject[] ringZero = new GameObject[7];
    public GameObject[] ringOne = new GameObject[7];
    public GameObject[] ringTwo = new GameObject[7];
    public GameObject[] ringThree = new GameObject[7];
    public GameObject[] ringFour = new GameObject[7];

    public GameObject ringZero_;
    public GameObject ringOne_;
    public GameObject ringTwo_;
    public GameObject ringThree_;
    public GameObject ringFour_;

    [SerializeField] private int[] OneSlotRing = new int[4];
    [SerializeField] private int[] TwoSlotRing = new int[4];

    [SerializeField] private int[] SlotRingFour = new int[8];
*/

    /*   int ringSelected;
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
   
           SlotRingFour[0] = 0;
           SlotRingFour[1] = 1;
           SlotRingFour[2] = 2;
           SlotRingFour[3] = 3;
           SlotRingFour[4] = 4;
           SlotRingFour[5] = 5;
           SlotRingFour[6] = 6;
           SlotRingFour[7] = 7;
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
                   
                   
   
                   for (int i = 0; i < SlotRingFour.Length; i++)
                   {
                       SlotRingFour[i]++;
                       if (SlotRingFour[i] > 7) { SlotRingFour[i] = 0; }
                   }
   
                   ringFour_.transform.Rotate(0, 0, -90, Space.Self);
               }
               else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
               {
   
                   for (int i = 0; i < SlotRingFour.Length; i++)
                   {
                       SlotRingFour[i]--;
                       if (SlotRingFour[i] < 0) { SlotRingFour[i] = 7; }
                   }
   
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
          
       } */

    private enum State
    {
        selecting,
        circleSelected
    }

    private State currentState = State.selecting;

    [SerializeField] private Transform[] rings;

    private Circle[] _circles;

    private int selectedRing = 0;

    private bool active = false;

    private PlayerController _playerController;

    private bool waitForNewInput = false;

    private void Awake()
    {
        _circles = new Circle[4];

        AddNewCircle(0, 6, 2);
        AddNewCircle(1, 0, 4);
        AddNewCircle(2, 2, 6);
        AddNewCircle(3, 4, 0);

        _playerController = FindObjectOfType<PlayerController>();
    }

    private void AddNewCircle(int index, int initialSlot1, int initialSlot2)
    {
        Transform[] spheres = new Transform[rings[index].transform.childCount];

        for (int i = 0; i < rings[index].transform.childCount; i++)
        {
            spheres[i] = rings[index].transform.GetChild(i);
        }

        _circles[index] = new Circle(initialSlot1, initialSlot2, spheres);
    }

    private void Update()
    {
        if (!active)
            return;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (waitForNewInput)
        {
            waitForNewInput = CheckInput(input);
            return;
        }

        bool interact = Input.GetButtonDown("Interact");

        if (currentState == State.selecting)
        {
            Selecting(input, interact);
        }
        else if (currentState == State.circleSelected)
        {
            RotatingSelectedRing(input, interact);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Exit();
        }

        waitForNewInput = CheckInput(input);
    }

    private bool CheckInput(Vector2 input)
    {
        return input.x > 0.1 || input.x < -0.1 || input.y > 0.1 || input.y < -0.1;
    }

    public void Interact()
    {
        active = true;
        _playerController.DisableController();
    }

    private void Selecting(Vector2 input, bool interact)
    {
        if (interact)
        {
            currentState = State.circleSelected;
        }

        if (input.x >= 0.3f)
        {
            selectedRing = 1;
        }
        else if (input.x <= -0.3f)
        {
            selectedRing = 3;
        }
        else if (input.y >= 0.3f)
        {
            selectedRing = 0;
        }
        else if (input.y <= -0.3f)
        {
            selectedRing = 2;
        }
    }

    private void RotatingSelectedRing(Vector2 input, bool interact)
    {
        if (interact)
        {
            currentState = State.selecting;
        }

        if (input.x >= 0.3)
        {
            Transform[] spheres = new Transform[8];

            spheres[0] = _circles[0].GetSphereSlot1();
            spheres[1] = _circles[0].GetSphereSlot2();
            spheres[2] = _circles[1].GetSphereSlot1();
            spheres[3] = _circles[1].GetSphereSlot2();
            spheres[4] = _circles[2].GetSphereSlot1();
            spheres[5] = _circles[2].GetSphereSlot2();
            spheres[6] = _circles[3].GetSphereSlot1();
            spheres[7] = _circles[3].GetSphereSlot2();
            
          
            _circles[0].SetSphereOnSlot1(spheres[7]);
            _circles[0].SetSphereOnSlot2(spheres[0]);
            _circles[1].SetSphereOnSlot1(spheres[1]);
            _circles[1].SetSphereOnSlot2(spheres[2]);
            _circles[2].SetSphereOnSlot1(spheres[3]);
            _circles[2].SetSphereOnSlot2(spheres[4]);
            _circles[3].SetSphereOnSlot1(spheres[5]); 
            _circles[3].SetSphereOnSlot2(spheres[6]);
           
            
            Vector3 savedPosition = spheres[0].position;
            Transform savedParent = spheres[0].parent;

            for (int i = 0; i < spheres.Length - 1; i++)
            {
                spheres[i].position = spheres[i + 1].position;
                spheres[i].parent = spheres[i + 1].parent;
            }

            spheres[spheres.Length - 1].position = savedPosition;
            spheres[spheres.Length - 1].parent = savedParent;
        }
        else if (input.x <= -0.3f)
        {
            Transform[] spheres = new Transform[8];

            spheres[0] = _circles[0].GetSphereSlot1();
            spheres[1] = _circles[0].GetSphereSlot2();
            spheres[2] = _circles[1].GetSphereSlot1();
            spheres[3] = _circles[1].GetSphereSlot2();
            spheres[4] = _circles[2].GetSphereSlot1();
            spheres[5] = _circles[2].GetSphereSlot2();
            spheres[6] = _circles[3].GetSphereSlot1();
            spheres[7] = _circles[3].GetSphereSlot2();

            _circles[0].SetSphereOnSlot1(spheres[1]);
            _circles[0].SetSphereOnSlot2(spheres[2]);
            _circles[1].SetSphereOnSlot1(spheres[3]);
            _circles[1].SetSphereOnSlot2(spheres[4]);
            _circles[2].SetSphereOnSlot1(spheres[5]);
            _circles[2].SetSphereOnSlot2(spheres[6]);
            _circles[3].SetSphereOnSlot1(spheres[7]);
            _circles[3].SetSphereOnSlot2(spheres[0]);
            
            Vector3 savedPosition = spheres[spheres.Length - 1].position;
            Transform savedParent = spheres[spheres.Length - 1].parent;

            for (int i = spheres.Length - 1; i > 0; i--)
            {
                spheres[i].position = spheres[i - 1].position;
                spheres[i].parent = spheres[i - 1].parent;
            }

            spheres[0].position = savedPosition;
            spheres[0].parent = savedParent;
        }

        if (input.y >= 0.3)
        {
            _circles[selectedRing].TurnRight();
        }
        else if (input.y <= -0.3f)
        {
            _circles[selectedRing].TurnLeft();
        }
    }

    private void Exit()
    {
        _playerController.EnableController();
        active = false;
    }
}


public class Circle
{
    private int slot1;
    private int slot2;

    private Transform[] spheres;

    public Transform[] GetBoles()
    {
        return spheres;
    }

    public Circle(int slot1, int slot2, Transform[] spheres)
    {
        this.slot1 = slot1;
        this.slot2 = slot2;

        this.spheres = spheres;
    }

    public Transform GetSphereSlot1()
    {
        return spheres[slot1];
    }

    public Transform GetSphereSlot2()
    {
        return spheres[slot2];
    }

    public void SetSphereOnSlot1(Transform sphere)
    {
        spheres[slot1] = sphere;
    }
    
    public void SetSphereOnSlot2(Transform sphere)
    {
        spheres[slot2] = sphere;
    }

    public void TurnRight()
    {
        Vector3 savedPosition = spheres[0].position;

        for (int i = 0; i < spheres.Length - 1; i++)
        {
            spheres[i].position = spheres[i + 1].position;
        }

        spheres[spheres.Length - 1].position = savedPosition;

        slot1--;
        if (slot1 < 0)
        {
            slot1 = spheres.Length - 1;
        }

        slot2--;
        if (slot2 < 0)
        {
            slot2 = spheres.Length - 1;
        }
    }

    public void TurnLeft()
    {
        Vector3 savedPosition = spheres[spheres.Length - 1].position;

        for (int i = spheres.Length - 1; i > 0; i--)
        {
            spheres[i].position = spheres[i - 1].position;
        }

        spheres[0].position = savedPosition;

        slot1++;
        if (slot1 > spheres.Length -1 )
        {
            slot1 = 0;
        }

        slot2++;
        if (slot2 > spheres.Length - 1)
        {
            slot2 = 0;
        }
    }
}