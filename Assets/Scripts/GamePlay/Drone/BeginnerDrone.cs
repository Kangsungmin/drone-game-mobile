using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerDrone : Drone
{
    public ParticleSystem grabEffect, GoalInEffect, ElecEffect;
    float endX, endY, endZ;

    void Awake()
    {
        Max_Fuel = 100.0f;
        Fuel = 100.0f;
        Speed = 120;
        MaxThrust = 200.0f;
        thisRB = GetComponent<Rigidbody>();
        GoalInEffect.Stop();
        grabEffect.Stop();
        ElecEffect.Stop();
        AnimatorState = true;//드론 에니메이션 상태
        bodyDir = Vector3.zero;
        int StageLevel;
        if (SceneData.SceneLevelName != null) StageLevel = int.Parse(SceneData.SceneLevelName);
        else StageLevel = 1;

    }
    public void SetReference(GameObject[] Ref)
    {
        playEnvironment = Ref[0];//Environment
        ui_Manager = Ref[1].GetComponent<UIManager>();
        moveJoystickLeft = Ref[2].GetComponent<VirtualJS_Left>();
        moveJoystickRight = Ref[3].GetComponent<VirtualJS_Right>();
        //파티클은 수동 할당요함.
        //드론의 하위 스크립트에도 레퍼런스 할당
        GameObject[] tempRef = new GameObject[3];
        tempRef[0] = ui_Manager.LiftButton.gameObject;
        tempRef[1] = ui_Manager.LiftButtonAni;
        tempRef[2] = ui_Manager.GaugeUI;
        //Claw.SendMessage("SetReference", tempRef);
    }
    void Start()
    {
        /*
        playEnvironment = GameObject.Find("PlayEnvironment");
        UIManager = GameObject.Find("UI");
        moveJoystickLeft = UIManager.transform.Find("VirtualJoystickLeft").GetComponent<VirtualJS_Left>();
        moveJoystickRight = UIManager.transform.Find("VirtualJoystickRight").GetComponent<VirtualJS_Right>();
        Claw = transform.Find("Claw");
        grabEffect = transform.Find("ParticlePack").Find("GrabEffect").Find("Particle System").GetComponent<ParticleSystem>();
        GoalInEffect = transform.Find("ParticlePack").Find("GoalInEffect").Find("Particle System").GetComponent<ParticleSystem>();
        ElecEffect = transform.Find("ParticlePack").Find("ElectronicEffect").Find("Particle System").GetComponent<ParticleSystem>();
        GoalInEffect.Stop();
        grabEffect.Stop();
        ElecEffect.Stop();
        AnimatorState = true;//드론 에니메이션 상태
        wingDir = new Vector3(270, 180, 0);
        bodyDir = Vector3.zero;
        int StageLevel = int.Parse(SceneData.SceneLevelName);
        */
    }
    //=============================Update함수(프레임마다 호출) [시작]=============================
    void Update()
    {
        bodyDir = Vector3.zero;
        //=============================드론 에니메이션[시작]=============================
        Animations();
        //=============================드론 에니메이션[끝]===============================
        //=============================드론 시동[시작]=============================
        //*초기 비행 시에만 동작한다.
        ////추력을 사용자가 위로 올리기 시작하면 시동이 켜진다.
        //<<05.26수정>>
        if (!droneStarting)
        {
            if (moveJoystickRight.Vertical() > 0.0f)
            {
                DronePowerOn = true;
                droneStarting = true;
            }
        }
        //=============================드론 시동[끝]===============================

        //=============================드론 조작[시작]=============================
        //*드론 작동 가능 시 동작한다.
        //*조이스틱 조작에따른 날개와 몸체 회전을 시킨다.

        if (DronePowerOn)//드론 작동 가능 시,
        {
            currentY = transform.eulerAngles.y;
            bodyDir.z = -40.0f * moveJoystickLeft.Horizontal();//몸체 좌우 회전
            bodyDir.x = 30.0f * moveJoystickLeft.Vertical();
            bodyDir.y = currentY;
            bodyDir.y += 4.0f * moveJoystickRight.Horizontal();
            transform.eulerAngles = bodyDir;//Drone 오브젝트 좌우 회전 적용

            if (flyDelay) StartCoroutine("AddCtrlToDrone", moveJoystickRight.Vertical()); //상하강 버튼을 누를시
            if (moveJoystickLeft.Horizontal() + moveJoystickLeft.Vertical() + moveJoystickRight.Vertical() != 0.0f) AnimatorState = false;
            else AnimatorState = true;




            transform.eulerAngles = bodyDir;//Drone 오브젝트 좌우 회전 적용
                                            //if ((moveJoystickLeft.Vertical() != 0.0f) || (moveJoystickRight.Vertical() != 0.0f)) ; //Arming.AttackMode = false;
                                            //좌측 조이스틱에 따른 날개 회전 애니메이션은 DroneAnim스크립트에서 처리한다.



            //애니메이션

        }
        //=============================드론 조작[끝]===============================
    }
    //=============================Update함수(프레임마다 호출) [끝]===============================

    //드론 자체적으로 안정성 향상을 위해 주기적으로 자세제어를 호출한다.

    void FixedUpdate()//일정간격으로 호출한다.
    {
        float sinz = Mathf.Sin(bodyDir.z * Mathf.Deg2Rad);
        sinz = Mathf.Abs(sinz);
        float sinx = Mathf.Sin(bodyDir.x * Mathf.Deg2Rad);
        sinx = Mathf.Abs(sinx);

        if (DronePowerOn)
        {
            F_Force = Speed * moveJoystickLeft.Vertical();
            R_Force = Speed * moveJoystickLeft.Horizontal();
            Forward = Vector3.forward * F_Force;
            Right = Vector3.right * R_Force;
            thisRB.AddRelativeForce(Forward);
            thisRB.AddRelativeForce(Right);
            Vector3 ExtraT = new Vector3(0.0f, Mathf.Abs(F_Force) * sinx + Mathf.Abs(R_Force) * sinz, 0.0f);

            thisRB.AddForce(Vector3.up * Thrust + ExtraT); // world의 위(y축)으로 추력만큼 힘을 가한다.
            if (fuelDelay) StartCoroutine("fuelControl");
        }

    }

    void Animations()
    {
        if (DroneAnimator != null)
        {
            DroneAnimator.SetBool("idle", AnimatorState);
        }
    }


    //=============================드론 추력 조작[시작]=============================
    //*파라미터로는 1 ~ -1이 넘어온다.
    IEnumerator AddCtrlToDrone(float Up)
    {
        flyDelay = false;
        if (DronePowerOn == false && Up > 0.0f) { Thrust = 40; DronePowerOn = true; }//드론 첫 동작시 초기 모터속도 1650

        if (Thrust > MaxThrust && Up > 0) {; }
        else if (DronePowerOn && Up == 0.0f) { Thrust = hovering_Thrust; }
        else//드론에 추력을 가할 때,
        {
            if (Thrust >= 0.0f)
            {
                if (Up < 0.0f) Thrust = 30 * Up;
                else Thrust += 20 * Up;
            }
            else
            {
                if (Up > 0.0f) Thrust += 10 * Up;
            }
        }
        yield return new WaitForSeconds(0.07f);//해당 메소드에 0.07초 지연을 시킨다.
        //이곳의 코드는 지연 후 이루어 진다.
        flyDelay = true;
    }
    //=============================드론 추력 조작[끝]===============================
    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "charger")
        {
            col.gameObject.GetComponentInParent<Charger>().chargeOn = false;
            getFuel();
        }
        else if (col.tag == "Item")//피자아이템을 획득
        {
            Destroy(col.gameObject);
        }

    }
    
    //=============================드론 충돌 판정[시작]=============================
    //=============================드론 충돌 판정[끝]===============================

    



    //=============================드론 연료함수[시작]=============================
    IEnumerator fuelControl()//연료 감소 메소드
    {
        fuelDelay = false;
        if (Fuel <= 0)
        {
            DronePowerOn = false;
            DroneAnimator.enabled = false;
            GameOver = true;
            /*
            Playenv.GameOver = true;
            playEnvironment.GetComponent<Playenv>().GameEnd();//게임종료시킴
            */
            Environment.GameOver = true;
            playEnvironment.GetComponent<Environment>().GameEnd();


        }
        else if (Thrust > 20 && DronePowerOn) Fuel -= 1.0f;//연료가 남아있을 때 감소시킨다.

        yield return new WaitForSeconds(1.0f);//해당 메소드에 1초 지연을 시킨다.
        fuelDelay = true;
    }
    //=============================드론 연료함수[끝]===============================

    //=============================연료 충전함수[시작]=============================
    public override void getFuel()
    {
        if (Fuel < 100)
        {
            if (Fuel > 80)
            {
                Fuel = 100;
            }
            else
            {
                Fuel += 15;
            }
        }
        ElecEffect.Play();
    }
    //=============================연료 충전함수[끝]===============================

    public void SpawnItem(string ItemName)
    {
        //랜덤하게 넘겨받은 아이템 스폰
        GameObject Item = Resources.Load("Prefabs/Items/"+ ItemName) as GameObject;
        Item = Instantiate(Item, Claw.transform.position, Claw.transform.rotation);
    }


    public override void GrabSomthing(GameObject targetObject)//물건을 집는 메소드
    {
        //수정후
        //인자로 받아온 오브젝트의 사이즈를 구한다.
        //claw + 오브젝트 높이 의 위치 아래 오브젝트를 가져온다
        playEnvironment.GetComponent<Playenv>().ActiveArrowToDestination();
        targetObject.transform.GetComponent<BoxCollider>().enabled = false;
        targetObject.transform.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 size = targetObject.transform.GetComponent<Renderer>().bounds.size;

        transform.position = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + (size.y * 0.6f), targetObject.transform.position.z);
        targetObject.transform.SetParent(transform);
        Claw.GetComponent<BoxCollider>().size = size;
        Claw.transform.position = targetObject.transform.position;
        OnGrab.grabState = "Using";
        GetComponent<Rigidbody>().mass += targetObject.GetComponent<Rigidbody>().mass;//드론의 무게를 증가시킨다.
        //파티클 실행
        GrabParticlePlay();

    }
    public override void DropSomthing()
    {
        if (transform.childCount >= 8)
        {
            //가이드 화살표 off
            playEnvironment.GetComponent<Playenv>().ArrowOff(false);
            GetComponent<Rigidbody>().mass -= transform.GetChild(7).GetComponent<Rigidbody>().mass;
            transform.GetChild(7).GetComponent<BoxCollider>().enabled = true;
            transform.GetChild(7).GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(7).parent = null;//물건 부모해제
        }
        //Claw.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
        //Claw.transform.localPosition = new Vector3(0, 0, 0);
    }


    public void GrabParticlePlay()
    {
        grabEffect.Play();

    }
    public void GoalInParticlePlay()
    {
        GoalInEffect.Play();
    }



}