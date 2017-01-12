using UnityEngine;
using System.Collections;


[System.Serializable] 	// Expose servo specification to UnityEditor
public class GenericServo : IActuator {

	public float maxPos = 170f;
	public float minPos = -170f;
	public float maxVel = 360f;		// Velocity (degrees per second)
	public float maxTor = 10f; 		// Torque

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
	public float command {
		set {
			// Clamp targetVelocity to max velocity to mimic servos
			JointMotor mot = motor;
			mot.targetVelocity = Mathf.Clamp(value, -1f, 1f) * maxVel;
			motor = mot;
		}
	}

	// This is cheating, use a sensor instead
	public float position {
		get {
			return hingeJoint.angle;
		}
	}
}
