using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameplayManager m_gameplayManager; // drag GameplayManager into this field in the inspector
    public RefereeController m_refereeController; // drag RefereeController into this field in the inspector

    protected ShootController m_shootController;
    protected CueBallPositionController m_cueBallPositionController;

    private PlayerControllerState m_state;

    private void Start() 
    {
        m_shootController = GetComponent<ShootController>();
        m_shootController.m_OnShoot += OnShoot;
        m_cueBallPositionController = GetComponent<CueBallPositionController>();
        m_cueBallPositionController.m_OnChoosingPositionFinished += OnChoosingPositionFinished;
        m_state = new PlayerControllerState_Aiming(this);
    }

    private void Update() 
    {
        m_state = m_state.Update();
        //log the type of state
        Debug.Log(m_state.GetType());
    }
    
    private void OnChoosingPositionFinished()
    {
        m_state = new PlayerControllerState_Aiming(this);
    }

    private void OnShoot()
    {
        //change state to waiting for balls stop
        m_state = new PlayerControllerState_WaitingForBallsStop(this);
    }

    abstract class PlayerControllerState
    {
        protected PlayerController m_playerController;
        protected ShootController m_shootController;
        protected CueBallPositionController m_cueBallPositionController;
        protected GameplayManager m_gameplayManager;
        protected RefereeController m_refereeController;

        public PlayerControllerState(PlayerController playerController)
        {
            m_playerController = playerController;
            m_shootController = playerController.m_shootController;
            m_cueBallPositionController = playerController.m_cueBallPositionController;
            m_gameplayManager = playerController.m_gameplayManager;
            m_refereeController = playerController.m_refereeController;
        }

        public virtual PlayerControllerState Update()
        {
            if ( m_gameplayManager.IsBallsMoving() )
            {
                return new PlayerControllerState_WaitingForBallsStop(m_playerController);
            }
            else if ( m_refereeController.IsFoul() )
            {
                return new PlayerControllerState_ChoosingCueBallPosition(m_playerController);
            }
            else
            {
                return this;
            }
        }
    }

    class PlayerControllerState_Aiming : PlayerControllerState
    {
        public PlayerControllerState_Aiming(PlayerController playerController) : base(playerController)
        {
            m_shootController.Enable();
            m_cueBallPositionController.Disable();
        }

        public override PlayerControllerState Update()
        {
            return base.Update();
        }
    }

    class PlayerControllerState_WaitingForBallsStop : PlayerControllerState
    {
        public PlayerControllerState_WaitingForBallsStop(PlayerController playerController) : base(playerController)
        {
            m_shootController.Disable();
            m_cueBallPositionController.Disable();
        }

        public override PlayerControllerState Update()
        {
            if ( !m_gameplayManager.IsBallsMoving() )
            {
                if ( m_refereeController.IsFoul() )
                {
                    return new PlayerControllerState_ChoosingCueBallPosition(m_playerController);
                }
                return new PlayerControllerState_Aiming(m_playerController);
            }
            return this;
        }
    }

    class PlayerControllerState_ChoosingCueBallPosition : PlayerControllerState
    {
        public PlayerControllerState_ChoosingCueBallPosition(PlayerController playerController) : base(playerController)
        {
            m_shootController.Disable();
            m_cueBallPositionController.Enable();
        }
        public override PlayerControllerState Update()
        {
            if ( m_gameplayManager.IsBallsMoving() )
            {
                return new PlayerControllerState_WaitingForBallsStop(m_playerController);
            }

            return this;
        }
    }
}
