using UnityEngine;
using InControl;

public class GameControls : MonoBehaviour
{
    public static GamePlayActions gamePlayActions;

    void Start()
    {
        gamePlayActions = new GamePlayActions();
        BindDefaultControls();
    }

    void BindDefaultControls()
    {
        gamePlayActions.select.AddDefaultBinding(Mouse.LeftButton);
        gamePlayActions.select.AddDefaultBinding(InputControlType.Action1);

        gamePlayActions.deselect.AddDefaultBinding(Mouse.RightButton);
        gamePlayActions.deselect.AddDefaultBinding(InputControlType.Action2);
        
        gamePlayActions.up.AddDefaultBinding(InputControlType.LeftStickUp);
        gamePlayActions.up.AddDefaultBinding(InputControlType.DPadUp);
        
        gamePlayActions.down.AddDefaultBinding(InputControlType.LeftStickDown);
        gamePlayActions.up.AddDefaultBinding(InputControlType.DPadDown);
        
        gamePlayActions.left.AddDefaultBinding(InputControlType.LeftStickLeft);
        gamePlayActions.up.AddDefaultBinding(InputControlType.DPadLeft);
        
        gamePlayActions.right.AddDefaultBinding(InputControlType.LeftStickRight);
        gamePlayActions.up.AddDefaultBinding(InputControlType.DPadRight);

        // UI Actions
        gamePlayActions.menuPause.AddDefaultBinding(Key.Escape);

        gamePlayActions.menuSelect.AddDefaultBinding(Mouse.LeftButton);
        gamePlayActions.menuSelect.AddDefaultBinding(InputControlType.Action1);

        gamePlayActions.menuGoBack.AddDefaultBinding(Key.Escape);
        gamePlayActions.menuGoBack.AddDefaultBinding(InputControlType.Action2);

        gamePlayActions.menuUp.AddDefaultBinding(Key.UpArrow);
        gamePlayActions.menuUp.AddDefaultBinding(Key.W);
        gamePlayActions.menuUp.AddDefaultBinding(InputControlType.DPadUp);

        gamePlayActions.menuDown.AddDefaultBinding(Key.DownArrow);
        gamePlayActions.menuDown.AddDefaultBinding(Key.S);
        gamePlayActions.menuDown.AddDefaultBinding(InputControlType.DPadDown);

        gamePlayActions.menuLeft.AddDefaultBinding(Key.LeftArrow);
        gamePlayActions.menuLeft.AddDefaultBinding(Key.A);
        gamePlayActions.menuLeft.AddDefaultBinding(InputControlType.DPadLeft);

        gamePlayActions.menuRight.AddDefaultBinding(Key.RightArrow);
        gamePlayActions.menuRight.AddDefaultBinding(Key.D);
        gamePlayActions.menuRight.AddDefaultBinding(InputControlType.DPadRight);

        // Specific Key Actions
        gamePlayActions.leftCtrl.AddDefaultBinding(Key.LeftControl);
        gamePlayActions.leftShift.AddDefaultBinding(Key.LeftShift);
        gamePlayActions.enter.AddDefaultBinding(Key.Return);
        gamePlayActions.tab.AddDefaultBinding(Key.Tab);
    }
}
