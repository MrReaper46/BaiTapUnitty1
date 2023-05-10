using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Speed;

    public Vector2[] CheckPoint;

    public int Current = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        Vector3 rotation = transform.eulerAngles;
        position.x -= 1;
        transform.position = position;
    }

}
