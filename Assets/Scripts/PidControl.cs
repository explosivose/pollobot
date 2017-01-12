using UnityEngine;
using System.Collections;

[System.Serializable]
public class PidControl {


	public float kp = 0.5f, ki, kd;

	public IActuator actuator {
		get; private set;
	}

	private float previous_error;
	private float integral = 0f;

	public void Init(IActuator actuator) {
		this.actuator = actuator;
	}

	public void SetPoint(float setPoint) {
		float error = setPoint - actuator.position; // TODO use a sensor model to get position
		integral = integral + error * Time.deltaTime;
		float derivative = (error - previous_error) / Time.deltaTime;
		actuator.command = kp * error + ki * integral + kd * derivative;
		previous_error = error;
	}
}
