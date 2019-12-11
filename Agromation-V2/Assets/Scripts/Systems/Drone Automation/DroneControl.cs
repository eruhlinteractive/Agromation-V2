using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneControl : MonoBehaviour
{
	/// <summary>
	/// Commands: MOV,ROT, RETURN
	/// </summary>


	private List<string> commands = new List<string>();
	private Vector3 homePos;
	private Quaternion homeRot;
	private Grid _grid;


	[SerializeField] private float moveSpeed;
	[SerializeField] private float rotSpeed;

	[SerializeField] bool isGroundDrone = false;

	public bool startparse = false;

	public string testCommands;
	public bool isParsing = false;
	[SerializeField] private int energy;

    // Start is called before the first frame update
    void Start()
    {
		_grid = Grid.Instance;
		homePos = _grid.GetNearestPointOnGridWithY(transform.position);
		//homePos = homePos + (Vector3.up * (transform.position.y - homePos.y));
		transform.position = homePos;
		homeRot = transform.rotation;
		DayNightCycle.newDay += ExecuteCommands;
    }

    // Update is called once per frame
    void Update()
    {
		if (isGroundDrone)
		{
			RaycastHit hit;
			if(Physics.Raycast(transform.position - transform.localScale.y/2 * Vector3.up,Vector3.down, out hit, 3f))
			{
				transform.position = new Vector3(transform.position.x, hit.point.y + transform.localScale.y / 2, transform.position.z);
			}
		}
		if (startparse)
		{
			if(!isParsing)
			{
				ParseInstructions(testCommands);
				isParsing = true;
			}
		}
    }

	/// <summary>
	/// Set the current command string
	/// </summary>
	/// <param name="instructions">The new instruction set</param>
	public void SetCommands(string instructions)
	{
		ParseInstructions(instructions);
	}

	/// <summary>
	/// Starts the running of commands
	/// </summary>
	private void ExecuteCommands()
	{
		StartCoroutine(RunCommands());
	}


	/// <summary>
	/// Parses through instructions
	/// </summary>
	private void ParseInstructions(string instructions)
	{
		//Clear the previous commands
		commands.Clear();


		string[] splitInst = instructions.Split('\n');  //Split instructions by line

		//Loop through each line in the instruction list
		for (int i = 0; i < splitInst.Length; i++)
		{
			string[] singleInstructionSplit = splitInst[i].Split(',');  //Split instruction line into instruction and value

			//If its not a return command
			if (singleInstructionSplit[0].ToUpper().Trim() != "RETURN")
			{
				//Player entered a rotation command
				if (singleInstructionSplit[0].Trim().ToUpper() == "ROT")
				{
					//Check Value Validity
					if (singleInstructionSplit[1].Trim().ToUpper() == "RIGHT")
					{
						commands.Add("RIGHT");
					}
					else if (singleInstructionSplit[1].Trim().ToUpper() == "LEFT")
					{
						commands.Add("LEFT");
					}

				}


				//Player entered Movement command
				else if (singleInstructionSplit[0].Trim().ToUpper() == "MOV")
				{
					//Try to parse a value out of the statement
					int instructionValue = -1;
					int.TryParse(singleInstructionSplit[1], out instructionValue);

					//If the value was successfully parsed
					if (instructionValue >= 0)
					{
						commands.Add(instructionValue.ToString());
					}
				}


			}
			//Check if it was a return Command
		//else if(singleInstructionSplit[0].ToUpper().Trim() == "RETURN")
		//{
		//	commands.Add("RETURN");
		//	
		//}

		}
		StartCoroutine(RunCommands());
	}

	IEnumerator RunCommands()
	{
		for (int i = 0; i < commands.Count; i++)
		{
			//Rotation Commands
			if(commands[i] == "RIGHT")
			{
				Quaternion targetRot = transform.rotation * Quaternion.AngleAxis(90, Vector3.up);

				//Rotate to target rotation
				while(Quaternion.Angle(transform.rotation,targetRot) > 1f)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
					yield return new WaitForSeconds(0.01f);
				}
				
			}
			else if(commands[i] == "LEFT")
			{
				Quaternion targetRot =  transform.rotation * Quaternion.AngleAxis(-90, Vector3.up);
				//Rotate to target rotation
				while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
					yield return new WaitForSeconds(0.01f);
				}
			}
			//Movement Commands
			else
			{
				int movDist = int.Parse(commands[i]);
				Vector3 targetPoint = _grid.GetNearestPointOnGridWithY((transform.position + (transform.forward * movDist)));

				while (Vector3.Distance(transform.position, new Vector3(targetPoint.x,transform.position.y,targetPoint.z)) > 0.01f)
				{
					transform.position = Vector3.MoveTowards(transform.position, targetPoint, Time.deltaTime * moveSpeed);
					
					yield return new WaitForSeconds(0.01f);
				}
					
			}

			yield return new WaitForSeconds(1f);
			//Debug.Log("Command " + i + " finished");
		}


		//AutoReturn
		{
			///else if (commands[i] == "RETURN")
			Quaternion targetRot = Quaternion.LookRotation(homePos - transform.position, transform.up);
			//Face home position
			while (Quaternion.Angle(transform.rotation, targetRot) > 4f)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
				//Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(lookAtHomeRot, Vector3.up), Time.deltaTime * rotSpeed);
				yield return new WaitForSeconds(0.01f);
			}


			//Lerp to home position
			while (Vector3.Distance(transform.position, new Vector3(homePos.x, transform.position.y, homePos.z)) > 0.01f)
			{
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(homePos.x, transform.position.y, homePos.z), Time.deltaTime * moveSpeed);
				yield return new WaitForSeconds(0.01f);
			}


			//Lerp to original "resting" rotation
			while (Quaternion.Angle(transform.rotation, homeRot) > 1f)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, homeRot, Time.deltaTime * rotSpeed);
				yield return new WaitForSeconds(0.01f);
			}
		}

	}

}
