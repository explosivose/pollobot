using UnityEngine;
using System.Collections;

public class PidControl {

	[System.Serializable]
	public class Parameters {
		public float kp = 0.5f, ki, kd;
	}

	public Parameters parameters {
		get;
		private set;
	}

	public IActuator actuator {
		get; private set;
	}

	private float previous_error;
	private float integral = 0f;

	public PidControl(IActuator actuator, Parameters parameters) {
		this.actuator = actuator;
		this.parameters = parameters;
	}
		
	public void SetPoint(float setPoint) {
		float error = setPoint - actuator.position; // TODO use a sensor model to get position
		integral = integral + error * Time.deltaTime;
		float derivative = (error - previous_error) / Time.deltaTime;
		actuator.command = parameters.kp * error + parameters.ki * integral + parameters.kd * derivative;
		previous_error = error;
	}
}
