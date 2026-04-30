using Sandbox;
using Sandbox.UI;
using System.Linq;

public class AdminPanelToggle : Component
{
    private bool _isOpen = false;
    private GameObject _screenPanelObject;

    protected override void OnStart()
    {
        var screenPanel = Scene.GetAllComponents<ScreenPanel>().FirstOrDefault();
        if ( screenPanel != null )
            _screenPanelObject = screenPanel.GameObject;

        _isOpen = false;
        if ( _screenPanelObject != null )
            _screenPanelObject.Enabled = false;
    }

    protected override void OnUpdate()
    {
        if ( Input.Pressed( "Score" ) )
        {
            Toggle();
        }
    }

    void Toggle()
    {
        _isOpen = !_isOpen;

        if ( _screenPanelObject != null )
            _screenPanelObject.Enabled = _isOpen;

        Mouse.Visibility = _isOpen
            ? MouseVisibility.Auto
            : MouseVisibility.Hidden;
    }

    public void ForceClose()
    {
        _isOpen = false;
        if ( _screenPanelObject != null )
            _screenPanelObject.Enabled = false;
        Mouse.Visibility = MouseVisibility.Hidden;
    }
}