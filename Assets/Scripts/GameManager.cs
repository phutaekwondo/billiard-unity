using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameplayManager m_gameplayManager = null; // drag and drop the GameplayManager in the inspector
    public ScreenManager m_screenManager = null; // drag and drop the ScreenManager in the inspector

    private void Start() {
        m_gameplayManager = GetComponent<GameplayManager>();
        m_screenManager = GetComponent<ScreenManager>();
    }

    private void Update() 
    {
        HandleGlobalInput();
    }

    private void HandleGlobalInput()
    {
        if ( Input.GetKeyDown( KeyCode.Escape ) )
        {   
            if ( m_gameplayManager.IsPaused() )
            {
                ResumeGameplay();
            }
            else
            {
                PauseGameplay();
            }
        }
    }

    public void PauseGameplay()
    {
        m_gameplayManager.Pause();
        m_screenManager.ActivatePanel( ScreenManager.PanelType.PauseMenu );
    }

    public void RestartGameplay()
    {
        m_gameplayManager.Restart();
        m_screenManager.ActivatePanel( ScreenManager.PanelType.GameplayHub );
    }

    public void ResumeGameplay()
    {
        m_gameplayManager.Resume();
        m_screenManager.ActivatePanel( ScreenManager.PanelType.GameplayHub );
    }
}
