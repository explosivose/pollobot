using UnityEngine;
using System.Collections;

public class Pollobot : MonoBehaviour {

	public Transform tibia;
	public Transform femur;
	public GenericServo servo;

	public float kp = 1, ki, kd;
	private float command;
	private float prev_error;
	private float integral = 0;


	// Use this for initialization
	void Start () {
		femur = transform.FindChild("Femur");
		servo.Init(femur.GetComponent<HingeJoint> ());
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnGUI() {
		GUILayout.Label ("Pollobot v0");
		GUILayout.Label(command.ToString());
		command = GUILayout.HorizontalSlider (command, -180, 180);
		GUILayout.Label (servo.Position ().ToString());
	}
}
