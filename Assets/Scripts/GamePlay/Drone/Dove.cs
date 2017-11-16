using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class Dove : Drone
{
    public ParticleSystem grabEffect, GoalInEffect;
    float endX, endZ;
    void Awake()
    {
        thisRB = GetComponent<Rigidbody>();
    }
    void Start()
    {
        playEnvironment = GameObject.Find("PlayEnvironment");
        InventoryManager = GameObject.Find("Inventory");
        UIManager = GameObject.Find("UI");
        moveJoystickLeft = UIManager.transform.Find("VirtualJoystickLeft").GetComponent<VirtualJS_Left>();
        moveJoystickRight = UIManager.transform.Find("VirtualJoystickRight").GetComponent<VirtualJS_Right>();
        Claw = transform.Find("Claw");
        grabEffect = transform.Find("FixRotation").Find("GrabEffect").Find("Particle System").GetComponent<ParticleSystem>();
        grabEffect.Stop();
        GoalInEffect = transform.Find("FixRotation").Find("GoalInEffect").Find("Particle System").GetComponent<ParticleSystem>();
        GoalInEffect.Stop();
        AnimatorState = true;//드론 에니메이션 상태
        wingDir = new Vector3(270, 180, 0);
        bodyDir = Vector3.zero;
        int StageLevel = int.Parse(SceneData.SceneLevelName);
    }
    /*
    bool getbooltime() { return this.bool_time; }
    void setbooltime(bool time) { this.bool_time = time; }
    */
    //=============================Update함수(프레임마다 호출) [시작]=============================
    void Update()
    {
        bodyDir = Vector3.zero;

        float amtMove = Speed * Time.smoothDeltaTime;//프레임당 이동 거리
        //float amtRot = RotSpeed * Time.smoothDeltaTime;//드론 z축기준 회전 속도
        float keyUp = Input.GetAxis("Up");
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
            currentY += moveJoystickLeft.Horizontal() * 2;//left조이스틱 회전 누적
            bodyDir.z = -50.0f * moveJoystickLeft.Horizontal();//몸체 좌우 회전
            bodyDir.y = currentY;

            transform.eulerAngles = bodyDir;//Drone 오브젝트 좌우 회전 적용
            if ((moveJoystickLeft.Vertical() != 0.0f) || (moveJoystickRight.Vertical() != 0.0f)) ; //Arming.AttackMode = false;

            //좌측 조이스틱에 따른 날개 회전 애니메이션은 DroneAnim스크립트에서 처리한다.
            if (flyDelay) StartCoroutine("AddCtrlToDrone", moveJoystickRight.Vertical()); //상하강 버튼을 누를시
            if (fuelDelay) StartCoroutine("fuelControl");
            //초당 힘을 가하도록 한다.

            //애니메이션
            if (moveJoystickLeft.Horizontal() + moveJoystickLeft.Vertical() + moveJoystickRight.Vertical() != 0.0f) AnimatorState = false;
            else AnimatorState = true;
        }
        //=============================드론 조작[끝]===============================

        //=============================드론 죽은지 체크[시작]===========================
        /*
        if (Rb.position.y <= Water.gameObject.GetComponent<Water_Time>().getWaterPosition())
        {
            GameOver = true;
            Drone_ON = false;
        }*/
        //=============================드론 죽은지 체크[끝]=============================

        //============================이동구간[시작]===================================
        if (Vector3.Distance(transform.position, Vector3.zero) < 242.0f)
        {
            endX = transform.position.x;
            endZ = transform.position.z;
        }
        else if (Vector3.Distance(transform.position, Vector3.zero) > 245.0f)
        {
            Vector3 setPos = new Vector3(endX - 1, transform.position.y, endZ - 1);
            transform.position = setPos;
        }
        //============================이동구간[끝]=====================================
    }
    //=============================Update함수(프레임마다 호출) [끝]===============================

    //드론 자체적으로 안정성 향상을 위해 주기적으로 자세제어를 호출한다.

    void FixedUpdate()//일정간격으로 호출한다.
    {
        rotVec.x = Mathf.Cos(70 * -bodyDir.z);
        rotVec.y = Mathf.Sin(70 * -bodyDir.z);

        if (DronePowerOn)
        {
            thisRB.AddForce(Vector3.up * Thrust); // Drone의 위(y축)으로 추력만큼 힘을 가한다.
            thisRB.AddRelativeForce(Vector3.forward * 70 * moveJoystickLeft.Vertical());
        }
    }

    void Animations()
    {
        if (DroneAnimator != null)
        {
            DroneAnimator.SetBool("idle", AnimatorState);
        }
    }

    //=============================드론 타겟 추척[시작]=============================
    /*
    void UpdateTarget()
    {
        if(enemies.Length != 0)
        {
            float shortDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)//모든 enemies에 대해
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);//enemy와의 거리
                bool enemyRender = enemy.GetComponent<Renderer>().isVisible; //enemy가 카메라 범위 내에 있는가
                if (distanceToEnemy < shortDistance && enemyRender)//가까이 있고 카메라에 잡혀있다면,
                {
                    shortDistance = distanceToEnemy;
                    nearestEnemy = enemy;//가장 가까운 적 갱신
                }
            }
            if (nearestEnemy != null && shortDistance <= range)//가장 가까운 적이 범위안에 있을 때,
            {
                //nearestEnemy.GetComponent<EnemyAnim>().Targeted();//타겟에게 타게팅 되었음을 알림. @타겟이 없어졌을때 에러남 
                target = nearestEnemy.transform;
                EnemyLastPos = target.position;//적군의 마지막 최근 위치를 저장한다.
                if (Arming.AttackMode) { AutoAim = true; }
                else { AutoAim = false;}
            }
            else//범위 내에 타겟이 없을 때,
            {
                target = null;
            }
        }
    }
    */
    //=============================드론 타겟 추척[끝]=============================


    //=============================드론 공격 반경정의[시작]=============================

    //=============================드론 공격 반경정의[끝]=============================

    //=============================드론 추력 조작[시작]=============================
    //*파라미터로는 1 ~ -1이 넘어온다.
    IEnumerator AddCtrlToDrone(float Up)
    {
        flyDelay = false;
        if (DronePowerOn == false && Up > 0) { Thrust = 40; DronePowerOn = true; }//드론 첫 동작시 초기 모터속도 1650
        if (Thrust > 80 && Up > 0) ;
        else if (DronePowerOn && Up == 0.0f) { Thrust = hovering_Thrust; }
        else//드론에 추력을 가할 때,
        {
            if (Thrust >= 0)
            {
                Thrust += 10 * Up;
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
            //col.transform.gameObject.SetActive(false);
            
        }
        else if (col.tag == "Box")
        {

        }

    }


    //=============================드론 충돌 판정[시작]=============================
    void OnCollisionEnter(Collision collision)//오브젝트와 충돌시 호출.
    {
        if (collision.gameObject.tag.Contains("Box"))
        {
            //박스는 무시
        }
        else
        {
            if (thisRB.velocity.magnitude < 1.3f)
            {

            }
            else
            {
                //Hit((int)thisRB.velocity.magnitude);
            }
        }
    }
    //=============================드론 충돌 판정[끝]===============================



    //=============================드론 연료함수[시작]=============================
    IEnumerator fuelControl()//연료 감소 메소드
    {
        fuelDelay = false;
        if (Fuel <= 0)
        {
            GameOver = true;//게임 종료
            DronePowerOn = false;
        }
        else if (Thrust > 20) Fuel--;       //연료가 남아있을 때 감소시킨다.
        if (Fuel <= 0) GameOver = true;


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
                Fuel += 20;
            }
        }
    }
    //=============================연료 충전함수[끝]===============================

    //=============================프로펠러 회전[시작]=============================
    /*
    void SpinProp(float AmtPropRot)
    {
        prop1.transform.Rotate(Vector3.up * AmtPropRot); // 예로 Vector3.up*Time.deltaTime 은 초당 1도 회전
        prop3.transform.Rotate(Vector3.up * AmtPropRot);
        prop2.transform.Rotate(Vector3.up * AmtPropRot * -1);
        prop4.transform.Rotate(Vector3.up * AmtPropRot * -1);
    }
    */
    //=============================프로펠러 회전[끝]===============================

    //=============================드론 공격받음[시작]=============================
    /*
    public override void Hit(int damage)
    {
        UIManager.gameObject.GetComponent<UIscripts>().damageAni();
        Hp -= damage;
        if (Hp <= 0)
        {
            GameOver = true;
            DronePowerOn = false;
        }
    }
    */
    //=============================드론 공격받음[끝]===============================

    public void SmoothLookAt(Vector3 T)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            Quaternion.LookRotation(-transform.position + T),
              Time.deltaTime * 50);
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

        if (transform.GetChild(3) != null)
        {
            GetComponent<Rigidbody>().mass -= transform.GetChild(3).GetComponent<Rigidbody>().mass;
            transform.GetChild(3).GetComponent<BoxCollider>().enabled = true;
            transform.GetChild(3).GetComponent<Rigidbody>().isKinematic = false;
            transform.GetChild(3).parent = null;//물건 부모해제
        }
        Claw.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
        Claw.transform.localPosition = new Vector3(0, 0, 0);
        //Claw.transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
    }
    public void GrabParticlePlay()
    {
        grabEffect.Play();
        //1.5초 후에 stop예약
        //IEnumerator coroutine = ParticleStop(1.5f);
        //StartCoroutine(coroutine);
    }
    public void GoalInParticlePlay()
    {
        GoalInEffect.Play();
    }
    IEnumerator ParticleStop(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("파티클 종료");
        grabEffect.Stop();
    }
    Vector3[] rayCasting()
    {
        Vector3[] val = new Vector3[2];
        val[0] = new Vector3(0, 0, 0);
        val[1] = Claw.transform.position;//집게 위치
        Ray ray = new Ray(Claw.position, Claw.forward);
        RaycastHit hitObject;
        if (Physics.Raycast(ray, out hitObject, 3.0f))
        {
            print(hitObject.transform);
            if (hitObject.transform.tag.Contains("Box"))
            {
                hitObject.transform.GetComponent<BoxCollider>().enabled = false;
                hitObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                hitObject.transform.SetParent(transform);
                OnGrab.grabState = "Using";
                val[0] = hitObject.transform.GetComponent<Renderer>().bounds.size;//박스 사이즈
                val[1] = hitObject.transform.position;//박스 위치
                return val;//물건의 사이즈의 포지션을 리턴
            }
            else return val;
        }
        else return val;
    }
}
