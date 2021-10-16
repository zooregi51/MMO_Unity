using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 1. 위치 벡터
// 2. 방향 벡터 (어떤 방향을 나타내기도 하고, 크기도 나타냄)

struct MyVector
{
    public float x;
    public float y;
    public float z;

    //           +
    //      +    +
    // +---------+ 피타고라스의 정리
    // 2D라면 피타고라스 한방이면 됩니다만.
    // 3D 좌표라면(x, y) 평면에 한 번 해주고 그 길이를 이용해
    // 다른 평면이랑 한번 계산을 해야 합니다.
    public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } }
    public MyVector normalized { get { return new MyVector(x / magnitude, y / magnitude, z / magnitude); } }

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MyVector operator *(MyVector a, float d)
    {
        return new MyVector(a.x * d, a.y * d, a.z * d);
    }
}

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;

    Vector3 _destPos;
    Texture2D _attackIcon;
    Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resources.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resources.Load<Texture2D>("Textures/Cursor/Hand");

        _stat = gameObject.GetComponent<PlayerStat>();

        // 혹시라도 다른 곳에서 바인딩하면 해지하고 다시 신청        
        Managers.Input.MouseAction -= OnMousedClicked;
        Managers.Input.MouseAction += OnMousedClicked;

        //MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
        //MyVector from = new MyVector(5.0f, 0.0f, 0.0f);
        //MyVector dir = to - from; // (5.0f, 0.0f, 0.0f)

        //dir = dir.normalized; //(1.0f, 0.0f, 0.0f)

        //MyVector newPos = from + dir * _speed;

        // 방향 벡터
        // 1. 거리(크기) magnitude    5   
        // 2. 실제 방향  normalized    -> 우측으로 
    }

    // GameObject (Player)
    // Transform
    // PlayerController (현재 위치)


    // float _yAngle = 0.0f;

    // float wait_run_ratio = 0.0f;

    // 스테이트 단점 : 동시에 2가지를 못함    
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    PlayerState _state = PlayerState.Idle;

    void UpdateDie()
    {
        // 아무것도 못함
    }

    void UpdateMoving()
    {              
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)                    
            _state = PlayerState.Idle;
        
        else
        {            
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);

            Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                _state = PlayerState.Idle;
                return;
            }

            //transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 애니메이션 
        Animator anim = GetComponent<Animator>();

        //현재 게임 상태에 대한 정보를 넘겨준다.
        anim.SetFloat("speed", _stat.MoveSpeed);

        //wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }

    void UpdateIdle()
    {
        // 애니메이션 
        Animator anim = GetComponent<Animator>();

        anim.SetFloat("speed", 0);

        //wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }        

    void Update()
    {
        UpdateMouseCursor();

        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
        }

        // Local -> World
        // TransformDirection

        // World -> Local
        // InverseTransformDirection

        // transform.Translate
        // 자기가 바라보고있는 로컬을 기준으로 연산        

        // transform.rotation

        // 절대 회전값
        //transform.eulerAngles = new Vector3(0.0f, Time.deltaTime * 100.0f, 0.0f);

        // +, -, delta
        //transform.Rotate(new Vector3(0.0f, Time.deltaTime * 100.0f, 0.0f));

        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, _yAngle, 0.0f));
    }

    //void OnKeyboard()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //        transform.position += Vector3.forward * Time.deltaTime * _speed;
    //    }

    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //        transform.position += Vector3.back * Time.deltaTime * _speed;
    //    }

    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //        transform.position += Vector3.left * Time.deltaTime * _speed;
    //    }

    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //        transform.position += Vector3.right * Time.deltaTime * _speed;
    //    }

    //    _moveToDest = false;
    //}

    void UpdateMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {                
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            else
            {
                if (_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    void OnMousedClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
            //Debug.Log($"Raycast Camera @ {hit.collider.gameObject.tag}");

            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                Debug.Log("Monster Click!");
            }

            else
            {
                Debug.Log("Ground Click!");
            }
        }
    }
}
