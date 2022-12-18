using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUIEvents : MonoBehaviour
{
    public ScreenManager m_screenManager = null; // drag and drop the ScreenManager in the inspector


    private void Start() {
        if ( m_screenManager == null ) {
            m_screenManager = GameObject.FindObjectOfType<ScreenManager>();
        }
        if ( m_screenManager == null ) {
            throw new System.Exception( "ScreenManager not found" );
        }
    }

    private void QuitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OnOptionsButtonClicked()
    {
        m_screenManager.ActivatePanel( ScreenManager.PanelType.Options );
    }
    public void OnMainMenuButtonClicked()
    {
        m_screenManager.ActivatePanel( ScreenManager.PanelType.MainMenu );
    }
    public void OnQuitButtonClicked()
    {
        m_screenManager.ActivatePanel( ScreenManager.PanelType.QuitComfirmation );
    }
    public void OnQuitYesButtonClicked()
    {
        QuitGame();
    }
    public void OnQuitNoButtonClicked()
    {
        m_screenManager.ActivatePanel( ScreenManager.PanelType.MainMenu );
    }
    public void OnPlayButtonClicked()
    {
        m_screenManager.EnterGamePlay();
    }
    public void OnPauseButtonClicked()
    {
        m_screenManager.ExitGamePlay();
    }

}
