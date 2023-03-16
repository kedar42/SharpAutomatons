namespace SharpAutomatons;

public class State
{
    private readonly string _name;
    private readonly Dictionary<char, HashSet<State>> _transitions = new Dictionary<char, HashSet<State>>();


    public State(string name, bool isFinal = false)
    {
        _name = name;
        IsFinal = isFinal;
    }

    public string Name => _name;

    public bool IsFinal { get; set; }

    public void AddTransition(char symbol, State to)
    {
        if (!_transitions.ContainsKey(symbol))
        {
            _transitions[symbol] = new HashSet<State>();
        }

        _transitions[symbol].Add(to);
    }

    public void RemoveTransition(char symbol, State to)
    {
        if (!_transitions.ContainsKey(symbol))
        {
            return;
        }

        _transitions[symbol].Remove(to);

        if (_transitions[symbol].Count == 0)
        {
            _transitions.Remove(symbol);
        }
    }

    public void RemoveTransitions(char symbol)
    {
        _transitions.Remove(symbol);
    }

    public void RemoveTransitions(State state)
    {
        foreach (var (symbol, states) in _transitions)
        {
            states.Remove(state);
            if (states.Count == 0)
            {
                _transitions.Remove(symbol);
            }
        }
    }

    public bool HasTransition(char symbol)
    {
        return _transitions.ContainsKey(symbol);
    }

    public HashSet<State> GetTransitions(char symbol)
    {
        return _transitions[symbol];
    }

    public bool LeadsTo(State state)
    {
        return _transitions.Values.Any(states => states.Contains(state));
    }
}