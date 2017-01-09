using UnityEngine;
using System.Collections;


[System.Serializable] 	// Expose servo specification to UnityEditor
public class GenericServo : IActuator {

	public float maxPos;
	public float minPos;
	public float maxVel;	// Velocity
	public float maxTor; 	// Torque

	// Constructor
	public void Init(HingeJoint hingeJoint) {
		this.hingeJoint = hingeJoint;
		UpdateModel();
	}

	// The servo class is a proxy of the Unity3D HingeJoint
	public HingeJoint hingeJoint {
		get; private set;
	}

	private JointMotor motor {
		get { return hingeJoint.motor; }
		set { hingeJoint.motor = value; }
	}

	private JointLimits limits {
		get { return hingeJoint.limits; }
		set { hingeJoint.limits = value; }
	}

	// IActuator - Map servo specification to hingejoint
	public void UpdateModel() {
		hingeJoint.useMotor = true;
		hingeJoint.useLimits = true;
		JointLimits lim = limits;
		JointMotor mot = motor;
		lim.min = minPos;
		lim.max = maxPos;
		mot.force = maxTor;
		limits = lim;
		motor = mot;
	}

	// Actuate!
	public void Command(float command) {
		command = Mathf.Clamp (command, -1f, 1f);
		JointMotor mot = motor;
		mot.targetVelocity = command * maxVel;
		motor = mot;
	}

	// This is cheating, use a sensor instead
	public float Position() {
		// FIXME this is always positive which fucks up PID control 
		return Quaternion.Angle(hingeJoint.connectedBody.transform.rotation, hingeJoint.transform.rotation);
	}
}
