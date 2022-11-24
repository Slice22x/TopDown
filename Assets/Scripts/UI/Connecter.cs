using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Connecter : MonoBehaviour
{
    public Transform Position1;
    public Transform Position2;

    [Range(0f, 1f)]
    public float Offset;

    public float XScaleAdder;
    public float YScaleAdder;

    [Range(1f, 10f)]
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        var Scale = (Position2.position - Position1.position).normalized + new Vector3(XScaleAdder,YScaleAdder);
        transform.localScale = Scale + new Vector3(0.2f,0.2f);

        Vector2 LookDir = Position2.position - Position1.position;
        float Ang = Mathf.Atan2(LookDir.y, LookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f,0f,Ang);

        transform.position = Vector3.Lerp(Position1.position, Position2.position, Offset);
    }
}
