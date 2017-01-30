using UnityEngine;
using System.Collections;


//[System.Serializable] 	// Expose servo specification to UnityEditor
public class GenericServo : IActuator {

	[System.Serializable]
	public class Parameters {
		public float maxPos = 170f;
		public float minPos = -170f;
		public float maxVel = 360f;		// Velocity (degrees per second)
		public float maxTor = 10f; 		// Torque
	}

	public Parameters parameters {
		get; private set;
	}

	public GenericServo(HingeJoint hingeJoint, Parameters parameters) {
		Init (hingeJoint, parameters);
	}

	// Initialize
	public void Init(HingeJoint hingeJoint, Parameters parameters) {
		Debug.Log ("GenericServo initialized.");
		this.hingeJoint = hingeJoint;
		this.parameters = parameters;
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
		lim.min = parameters.minPos;
		lim.max = parameters.maxPos;
		mot.force = parameters.maxTor;
		limits = lim;
		motor = mot;
	}

	// Actuate!
	public float command {
		set {
			// Clamp targetVelocity to max velocity to mimic servos
			JointMotor mot = motor;
			mot.targetVelocity = Mathf.Clamp(value, -1f, 1f) * parameters.maxVel;
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
