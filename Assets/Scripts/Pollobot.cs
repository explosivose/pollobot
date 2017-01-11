using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pollobot : MonoBehaviour {

	public GenericServo leftPatellaServo;
	public GenericServo rightPatellaServo;

	public float kp = 1, ki, kd;
	private float command;
	private float prev_error;
	private float integral = 0;

	// References to child rigidbodies
	private List<Rigidbody> rigidbodies = new List<Rigidbody>();

	// References to child objects
	private Transform centreOfMass;
	private Transform rightFemur;
	private Transform leftFemur;

	// Use this for initialization
	void Start () {
		// right leg servo setup
		rightFemur = transform.FindChild("Right Femur");
		rightPatellaServo.Init(rightFemur.GetComponent<HingeJoint> ());
		// left leg servo setup
		leftFemur = transform.FindChild ("Left Femur");
		leftPatellaServo.Init(leftFemur.GetComponent<HingeJoint> ());
		// Centre of mass marker
		gameObject.GetComponentsInChildren<Rigidbody>(rigidbodies);
		centreOfMass = transform.FindChild("Centre Of Mass");
	}
	
	// Update is called once per frame
	void Update () {

		// Move the centreOfMass transform to pollobot's C.O.M.
		MarkCentreOfMass();

		rightPatellaServo.UpdateModel();
		leftPatellaServo.UpdateModel();
		// Simple PID control for servo
		// TODO move PID functionality to a controller class
		// TODO add a second controller for the left patella servo
		// TODO add "Left/Right Forward/Side Trochanter" servos (the hip ones)
		float error = command - rightPatellaServo.Position (); // TODO use a sensor model to get position
		integral = integral + error * Time.deltaTime;
		float derivative = (error - prev_error) / Time.deltaTime;
		float output = kp * error + ki * integral + kd * derivative;
		prev_error = error;
		rightPatellaServo.Command (output);
	}

	void MarkCentreOfMass() {
		Vector3 centre = Vector3.zero;
		float totalMass = 0f;
		foreach (Rigidbody r in rigidbodies) {
			totalMass += r.mass;
			// Debug.DrawRay (r.worldCenterOfMass, Vector3.forward);
			centre += r.worldCenterOfMass * r.mass;
		}
		centre /= totalMass;
		centreOfMass.transform.position = centre;
	}

	void OnGUI() {
		GUILayout.Label ("Pollobot v0");
		GUILayout.Label(command.ToString());
		command = GUILayout.HorizontalSlider (command, -180, 180);
		GUILayout.Label (rightPatellaServo.Position ().ToString());
	}
}
