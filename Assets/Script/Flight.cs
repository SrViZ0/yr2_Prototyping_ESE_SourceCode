using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Flight : MonoBehaviour
{
    [SerializeField] private float horiz/* = 5f*/;
    //[SerializeField] private float horizAmount = 240f;
    [SerializeField] private float vert;
    [SerializeField] private float side;

    public Vector3 tensor;
    public Rigidbody rb;
    public float responsiveness = 100f;
    public float vertResponsiveness = 10f;


    // Start is called before the first frame update
    private float responseModifiers
    {
        get { return (rb.mass / 10f) * responsiveness; } //takes plane mass into account
    }

    private float responseModifiers2
    {
        get { return (rb.mass / 10f) * vertResponsiveness; } //takes plane mass into account
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Instance.buffSelect || GameManager.Instance.guide)
        {
            GetComponent<AudioSource>().volume = 0.5f;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0.8f;
        }
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.disabled) { SpaceFlight(); }
    }
    private void SpaceFlight()
    {
        transform.position += transform.forward * GameManager.Instance.netFlightSpeed * Time.deltaTime;

        vert = Input.GetAxis("Horizontal");
        horiz = Input.GetAxis("Vertical");
        side = Input.GetAxis("Tilt");
        rb.AddTorque(transform.right * -horiz * responseModifiers2);
        rb.AddTorque(transform.up * vert * responseModifiers);
        rb.AddTorque(transform.forward * -side * responseModifiers2);

        //// inputs
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //horiz += horizontalInput * horizAmount * Time.deltaTime;
        //vert = Mathf.Lerp(0, 70, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        //side = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);


        //// apply rotation
        //transform.localRotation = Quaternion.Euler(Vector3.up * horiz + Vector3.right * -vert + Vector3.forward * side);

        //rb = GetComponent<Rigidbody>();


    }

}
