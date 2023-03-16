namespace SharpAutomatons;

public class Automaton
{
    private readonly string _name;

    private readonly State _initialState;
    // todo add check if is deterministic


    public Automaton(string name, State initialState)
    {
        _name = name;
        _initialState = initialState;
    }

    public bool Test(string input)
    {
        var currentState = _initialState;
        foreach (var symbol in input)
        {
            currentState = currentState.GetNextState(symbol);

            if (currentState == null) // get rid of this check by using hell state
            {
                return false;
            }
        }

        return currentState.IsFinal;
    }
}