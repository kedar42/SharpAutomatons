namespace SharpAutomatons;

public class State
{
    private readonly string _name;
    private readonly bool _isFinal;
    private readonly Dictionary<char, HashSet<State>> _transitions = new Dictionary<char, HashSet<State>>();

    public State(string name, bool isFinal = false)
    {
        _name = name;
        _isFinal = isFinal;
    }

    public string Name => _name;

    public bool IsFinal => _isFinal;
    
    public void AddTransition(char symbol, State to)
    {
        if (!_transitions.ContainsKey(symbol))
        {
            _transitions[symbol] = new HashSet<State>();
        }
        _transitions[symbol].Add(to);
    }
}