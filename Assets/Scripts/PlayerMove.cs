using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStates;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] MoveState state;
    public Rigidbody rb;
    public Transform cam;

    
    // Start is called before the first frame update
    void Start()
    {
        if (state != null)
        {
            state.OnEnterState(this);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
