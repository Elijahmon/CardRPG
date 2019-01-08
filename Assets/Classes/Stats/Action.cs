
public class Action {

    int currentAction;
    int startingAction;

    public void Init()
    {
        Init(1);
    }
    public void Init(int action)
    {
        currentAction = action;
        startingAction = action;
    }
    public void Tick()
    {

    }

    public int GetCurrentAction()
    {
        return currentAction;
    }

    public int GetStartingAction()
    {
        return startingAction;
    }

    public void IncreaseStartingAction(int amount)
    {
        startingAction += amount;
    }
}
