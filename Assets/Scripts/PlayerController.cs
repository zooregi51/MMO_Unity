using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    float _speed = 10.0f;

    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard; // 혹시라도 다른 곳에서 구독 신청하면 해지하고 다시 신청
        Managers.Input.KeyAction += OnKeyboard;

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


    float _yAngle = 0.0f;

    void Update()
    {
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

    void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }
    }

}
