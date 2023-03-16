namespace SharpAutomatons;

public class State
{
    private readonly string _name;
    private readonly Dictionary<char, State> _transitions = new();


    public State(string name, bool isFinal = false)
    {
        _name = name;
        IsFinal = isFinal;
    }

    public string Name => _name;

    public bool IsFinal { get; set; }

    public void AddTransition(char symbol, State to)
    {
        _transitions[symbol] = to;
    }

    public void RemoveTransitions(char symbol)
    {
        _transitions.Remove(symbol);
    }
    

    public State? GetNextState(char symbol)
    {
        // todo try to use hell to throw error instead of this
        return !_transitions.ContainsKey(symbol) ? null : _transitions[symbol];
    }
    
}