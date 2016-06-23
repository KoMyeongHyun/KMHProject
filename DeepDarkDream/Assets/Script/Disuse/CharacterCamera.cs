using UnityEngine;
using System.Collections;

public class CharacterCamera : MonoBehaviour {

    public float smoothTime = 0.1f;
    public float maxSpeed = 150.0f;

    public float heightSmoothTime = 0.1f;

    public float distance = 2.5f;
    public float height = 0.75f;

    private float f_heightVelocity;
    private float f_angleVelocity;

    private Vector3 v3_velocity;

    private Transform target;
    private Transform cameraTransform;

    private float f_maxRotation;
    private CharacterController c_charactorControl;

    //타킷
    private float f_targetHeight = Mathf.Infinity;
    private Vector3 v3_centerOffset = Vector3.zero;

	// Use this for initialization
	void Awake ()
    {
        cameraTransform = Camera.main.transform;
        target = GameObject.FindWithTag("Player").transform;
        c_charactorControl = GameObject.FindWithTag("Player").GetComponent<CharacterController>();

        v3_centerOffset = target.GetComponent<Collider>().bounds.center - target.position;
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //Vector3 targetCenter = target.position + v3_centerOffset;

        float originalTargetAngle = target.eulerAngles.y;
        float currentAngle = cameraTransform.eulerAngles.y;
        float targetAngle = originalTargetAngle;

        if (AngleDistance(currentAngle, targetAngle) > 160.0f) { }
	}

    public float AngleDistance(float a, float b)
    {
        a = Mathf.Repeat(a, 360);
        b = Mathf.Repeat(b, 360);

        return Mathf.Abs(b - a);
    }
}
