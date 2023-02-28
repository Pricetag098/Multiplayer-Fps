using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStates;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] MoveState state;
    public Rigidbody rb;
    public Transform cam;
    public CapsuleCollider col;

    [Header("States")]
    public List<MoveState> moveStates = new List<MoveState>();
   
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < moveStates.Count; i++)
		{
            moveStates[i] = Instantiate(moveStates[i]);
		}
        if (state != null)
        {
            state.OnEnterState(this);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public bool UseCrouchAnim()
    {
        return state.useCrouchAnimation;
    }
    // Update is called once per frame
    void Update()
    {
        if(state != null)
		{
            state.StateUpdate();
		}
    }
	private void FixedUpdate()
	{
        if (state != null)
        {
            state.StateFixedUpdate();
        }
    }

    public void ChangeState(MoveState nState)
	{
        if (state != null)
        {
            state.OnExitState();
            nState.camRotX = state.camRotX;
        }
        state = nState;
        state.OnEnterState(this);
	}
	private void OnDrawGizmos()
	{
        if(state!=null)
		state.OnDrawGizmos();
	}

}
