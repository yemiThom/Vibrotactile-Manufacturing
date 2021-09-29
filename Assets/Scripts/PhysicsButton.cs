using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] Transform buttonTop;
    [SerializeField] Transform buttonLowerLimit;
    [SerializeField] Transform buttonUpperLimit;

    [SerializeField] float threshold;
    [SerializeField] float force = 10f;
    [SerializeField] float upperLowerDiff;
        
    [SerializeField] bool rotY;
    [SerializeField] bool rotFoward;
    [SerializeField] bool isPressed;

    
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(),buttonTop.GetComponent<Collider>());

        if(transform.eulerAngles != Vector3.zero)
        {
            Vector3 savedAngle = transform.eulerAngles;
            transform.eulerAngles = Vector3.zero;
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
            transform.eulerAngles = savedAngle;
        }
        else
        {
            upperLowerDiff = buttonUpperLimit.position.y - buttonLowerLimit.position.y;
        }
    }

    void Update()
    {
        buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y, 0);
        buttonTop.transform.localEulerAngles = new Vector3(0,0,0);

        if(buttonTop.localPosition.y >= 0)
        {
            buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x, buttonUpperLimit.position.y, buttonUpperLimit.position.z);
        }
        else
        {
            buttonTop.GetComponent<Rigidbody>().AddForce(buttonTop.transform.up * force * Time.fixedDeltaTime);
        }

        if(buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y)
        {
            buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x, buttonLowerLimit.position.y, buttonLowerLimit.position.z);
        }

        if(Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * threshold)
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }

        if(isPressed)
        {
            Pressed();
        }
        if(!isPressed)
        {
            Released();
        }
    }

    void Pressed()
    {        
        if(rotY)
        {
            GameController.Instance.OnYRotButtonPressed(rotFoward);
        }
        else
        {
            GameController.Instance.OnZRotButtonPressed(rotFoward);
        }
    }

    void Released()
    {

    }
}
