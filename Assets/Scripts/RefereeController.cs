using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefereeController : MonoBehaviour
{
    public CollisionChecker m_bottomSurfaceCollisionChecker; //drag CollisionChecker into this field in the inspector
    public CueBallPositionController m_cueBallPositionController; //drag CueBallPositionController into this field in the inspector
    public GameplayManager m_gameplayManager; //drag GameplayManager into this field in the inspector

    private RefereeControllerState m_state;
    private bool m_isFoul = false;

    private void Start() 
    {
        m_state = new RefereeControllerState_WaitingForPlayerShoot(this);
        m_bottomSurfaceCollisionChecker.m_onCollision += OnBottomSurfaceCollision;

        m_gameplayManager.m_OnGameplayRestart += Restart;

        m_cueBallPositionController.m_OnChoosingPositionFinished += OnChoosingPositionFinished;
    }

    private void Restart()
    {
        m_state = new RefereeControllerState_WaitingForPlayerShoot(this);
    }

    private void OnChoosingPositionFinished()
    {
        m_state = new RefereeControllerState_WaitingForPlayerShoot(this);
    }

    private void Update() 
    {
        m_state = m_state.Update();
    }

    private void OnBottomSurfaceCollision(string senderTag, string collisionObjectTag)
    {
        if (senderTag == "BottomSurface" && collisionObjectTag == "CueBall")
        {
            m_state = new RefereeControllerState_Penalizing(this);
        }
    }

    public void SetFoul(bool isFoul)
    {
        m_isFoul = isFoul;
    }

    public bool IsFoul()
    {
        return m_isFoul;
    }

    abstract class RefereeControllerState
    {
        protected RefereeController m_refereeController;
        public RefereeControllerState(RefereeController refereeController)
        {
            m_refereeController = refereeController;
        }

        public abstract RefereeControllerState Update();
    }

    class RefereeControllerState_WaitingForPlayerShoot : RefereeControllerState
    {
        public RefereeControllerState_WaitingForPlayerShoot(RefereeController refereeController) : base(refereeController)
        {
            m_refereeController.SetFoul(false);
        }
        public override RefereeControllerState Update()
        {
            //Need to implement
            return this;
        }
    }

    class RefereeControllerState_Penalizing : RefereeControllerState
    {
        public RefereeControllerState_Penalizing(RefereeController refereeController) : base(refereeController)
        {
            m_refereeController.SetFoul(true);
        }
        public override RefereeControllerState Update()
        {
            //Need to implement
            return this;
        }
    }
}
