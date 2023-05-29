using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    [SerializeField] private GameObject[] _checkPoints;

    private int TargetIndex = 0;
    private Vector3[] _cornerPosition;
    private Vector3[] _cornerRotation;
    public GameObject StartSelect;
    public void ShowInfo()
    {
       
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    void SetCheckPoint()
    {
        _cornerPosition = new Vector3[_checkPoints.Length];
        _cornerRotation = new Vector3[_checkPoints.Length];

        for (int i = 0; i < _checkPoints.Length; i++)
        {
            _cornerPosition[i] = _checkPoints[i].transform.position;
        }

        for (int i = 0; i < _checkPoints.Length; i++)
        {
            _cornerRotation[i] = _checkPoints[i].transform.eulerAngles;
        }

    }

    void SetStart()
    {
        Vector3 StartPosition = StartSelect.transform.position;
        Vector3 StartAngle = StartSelect.transform.eulerAngles;

        transform.position = StartPosition;
        transform.eulerAngles = StartAngle;

        //Debug.Log($"(Current: {transform.position.x},{transform.position.y},{transform.position.z}) - Rotation y: {transform.eulerAngles}");
    }

    void CarMove(Vector3 target)
    {
        Vector3 position = transform.position;
        
        if (position.x < target.x && target.x - position.x > 1)
        {   
            position.x += _speed /10;
        }
        if (position.x > target.x && position.x - target.x > 1)
        {
            position.x -= _speed /10;
        }
        if (position.z < target.z && target.z - position.z > 1)
        {
            position.z += _speed /10;
        }
        if (position.z > target.z && position.z - target.z > 1)
        {
            position.z -= _speed /10;
        }

        transform.position = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetStart();

        SetCheckPoint();

        SetClosestTarget();

    }

    private void SetClosestTarget()
    {
        for (int i = 0; i < _checkPoints.Length; i++)
        {
            Vector3 CheckPointPos = _checkPoints[i].transform.position;
            Vector3 StartPos = StartSelect.transform.position;
            float Magnitude = (CheckPointPos - StartPos).sqrMagnitude;
            float Magnitude_min = (_checkPoints[TargetIndex].transform.position - StartPos).sqrMagnitude;
           
            if (Magnitude < Magnitude_min && CheckPointPos.x <= StartPos.x || CheckPointPos.z <= StartPos.z)
            {
                TargetIndex = i;
            }
            else { continue; }
        }

        Debug.Log(TargetIndex);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 TargetPosition = _cornerPosition[TargetIndex];
        Vector3 TargetRotation = _cornerRotation[TargetIndex];

        if ((transform.position - TargetPosition).magnitude > 2)
        {
            CarMove(TargetPosition);
        }
        else
        {
            TargetIndex++;
            transform.transform.eulerAngles = TargetRotation;
            if (TargetIndex == 4) TargetIndex = 0;
        }

        SpeedController();

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            ClearLog();
        }

    }

    private void SpeedController()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _speed += 0.01f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _speed -= 0.01f;
        }
    }
}
