using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pollobot : MonoBehaviour {

	public GenericServo servo;

	public float kp = 1, ki, kd;
	private float command;
	private float prev_error;
	private float integral = 0;

	// References to child rigidbodies
	private List<Rigidbody> rigidbodies = new List<Rigidbody>();

	// References to child objects
	private Transform centreOfMass;
	private Transform femur;

	// Use this for initialization
	void Start () {
		femur = transform.FindChild("Femur");
		centreOfMass = transform.FindChild("Centre Of Mass");
		servo.Init(femur.GetComponent<HingeJoint> ());
		gameObject.GetComponentsInChildren<Rigidbody>(rigidbodies);
	}
	
	// Update is called once per frame
	void Update () {

		// Move the centreOfMass transform to pollobot's C.O.M.
		MarkCentreOfMass();

		servo.UpdateModel();
		// Simple PID control for servo
		// TODO move PID functionality to a controller class
		float error = command - servo.Position (); // TODO use a sensor model to get position
		integral = integral + error * Time.deltaTime;
		float derivative = (error - prev_error) / Time.deltaTime;
		float output = kp * error + ki * integral + kd * derivative;
		prev_error = error;
		servo.Command (output);
	}

	void MarkCentreOfMass() {
		Vector3 centre = Vector3.zero;
		float totalMass = 0f;
		foreach (Rigidbody r in rigidbodies) {
			totalMass += r.mass;
			Debug.DrawRay (r.worldCenterOfMass, Vector3.forward);
			centre += r.worldCenterOfMass * r.mass;
		}
		centre /= totalMass;
		centreOfMass.transform.position = centre;
	}

	void OnGUI() {
		GUILayout.Label ("Pollobot v0");
		GUILayout.Label(command.ToString());
		command = GUILayout.HorizontalSlider (command, -180, 180);
		GUILayout.Label (servo.Position ().ToString());
	}
}
