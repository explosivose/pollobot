using UnityEngine;
using System.Collections;

public interface IActuator  {

	// Update the physics model of the actuator
	void UpdateModel();

	// Send a command to the actuator
	float command { set; } 

	// Get the current actuator position (should only be used for debugging)
	float position { get; }
}
