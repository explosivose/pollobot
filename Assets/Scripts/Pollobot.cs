using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pollobot : MonoBehaviour {

	// TODO think about recording and plotting experiments
	//	left/right foot and pelvic displacement over time (plot x,y,z seprately) 
	//  pid and servo parameters, masses of rigidbodies

	public GenericServo.Parameters patellaServoParams;
	public PidControl.Parameters patellaPidParams;

	private GenericServo leftPatellaServo;
	private GenericServo rightPatellaServo;
	private PidControl leftPatellaControl; 
	private PidControl rightPatellaControl;

	private float command;

	// References to child rigidbodies
	private List<Rigidbody> rigidbodies = new List<Rigidbody>();

	// References to child objects
	private Transform centreOfMass;
	private Transform rightFemur;
	private Transform leftFemur;

	// Use this for initialization
	void Start () {
		// Centre of mass marker
		gameObject.GetComponentsInChildren<Rigidbody>(rigidbodies);
		centreOfMass = transform.FindChild("Centre Of Mass");
		// right leg servo setup
		rightFemur = transform.FindChild("Right Femur");
		rightPatellaServo = new GenericServo(rightFemur.GetComponent<HingeJoint>(), patellaServoParams);
		// left leg servo setup
		leftFemur = transform.FindChild ("Left Femur");
		leftPatellaServo = new GenericServo(rightFemur.GetComponent<HingeJoint>(), patellaServoParams);
		leftPatellaControl = new PidControl(leftPatellaServo, patellaPidParams);
	}
	
	// Update is called once per frame
	void Update () {

		// Move the centreOfMass marker to pollobot's C.O.M.
		MarkCentreOfMass();

		rightPatellaServo.UpdateModel();
		leftPatellaServo.UpdateModel();
		leftPatellaControl.SetPoint(command);
		// TODO add a second controller for the left patella servo
		// TODO add "Left/Right Forward/Side Trochanter" servos (the hip ones)
	}

	// Move the centreOfMass marker to pollobot's C.O.M.
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
		// TODO read up on complex automation and control for robot limbs
		// TODO separate manual GUI control class for testing available controllers
		GUILayout.Label ("Pollobot v0");
		GUILayout.Label(command.ToString());
		command = GUILayout.HorizontalSlider (command, -180, 180);
		GUILayout.Label (rightPatellaServo.position.ToString());
	}
}
