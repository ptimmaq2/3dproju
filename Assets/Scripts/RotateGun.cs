using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{

    public float currentAngle;
    public float startAngle;
    public bool rotating;
    public float rotateDuration;
    public float counter;
    private float _xAngle;
    public float xAngle
    {
        get
        {
            return _xAngle;
        }
        set
        {
            Debug.Log("Uusi kulma, johon piippu k‰‰ntyy on: " + value);
            startAngle = transform.localRotation.eulerAngles.x;
            _xAngle = value;
            rotating = true;
            counter = 0;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        counter += Time.deltaTime;

        if(counter > rotateDuration && rotating == true)
        {
            //piippu on ollut k‰‰ntym‰ss‰ ja juuri k‰‰ntynyt oikeaan kulmaan.
            rotating = false;
        }

        currentAngle = Mathf.LerpAngle(startAngle, xAngle, counter / rotateDuration);
        transform.localEulerAngles = new Vector3(currentAngle, 0, 0);
    }
}
