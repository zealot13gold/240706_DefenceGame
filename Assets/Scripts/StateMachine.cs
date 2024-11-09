using UnityEngine;

/*

 */

public class StateMachine : MonoBehaviour 
{
    public BaseState currentState;
    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public void ChangeState(BaseState nextState)                                        // 행동을 변경
    {
        Debug.LogFormat("StateManager - 현재 상태 : {0}, 다음 상태 : {1}", currentState, nextState);

        if (currentState == nextState) return;                                          // 행동이 변하지 않는다면 함수를 종료

        else if (currentState != null)                                                  // currentState가 0이 아니라면(처음 시작 시 currentState는 null)
        {
            currentState.OnStateExit();    
        }

        currentState = nextState;                                                       // 행동을 변경
        currentState.OnStateEnter();                                                    // 새로운 행동을 시작                                    
    }
}