using InControl;

public class GamePlayActions : PlayerActionSet
{
    public PlayerAction select;
    public PlayerAction deselect;
    public PlayerAction up, down, left, right;
    public PlayerTwoAxisAction directionalAxis;

    // UI Actions
    public PlayerAction menuPause, menuSelect, menuGoBack;
    public PlayerAction menuLeft, menuRight, menuUp, menuDown;

    // Specific Keys
    public PlayerAction leftCtrl, leftShift, enter, tab;

    public GamePlayActions()
    {
        select = CreatePlayerAction("Select");
        deselect = CreatePlayerAction("Deselect");

        up = CreatePlayerAction("Up");
        down = CreatePlayerAction("Down");
        left = CreatePlayerAction("Left");
        right = CreatePlayerAction("Right");
        directionalAxis = CreateTwoAxisPlayerAction(left, right, down, up);

        menuPause = CreatePlayerAction("MenuPause");
        menuSelect = CreatePlayerAction("MenuSelect");
        menuGoBack = CreatePlayerAction("MenuGoBack");

        menuLeft = CreatePlayerAction("MenuLeft");
        menuRight = CreatePlayerAction("MenuRight");
        menuUp = CreatePlayerAction("MenuUp");
        menuDown = CreatePlayerAction("MenuDown");

        // Specific Key Actions
        leftCtrl = CreatePlayerAction("LeftCtrl");
        leftShift = CreatePlayerAction("LeftShift");
        enter = CreatePlayerAction("Enter");
        tab = CreatePlayerAction("Tab");
    }
}
