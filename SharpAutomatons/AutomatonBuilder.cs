namespace SharpAutomatons;

public class AutomatonBuilder
{
    private readonly string _automatonName;

    private readonly string _initialState;
    private readonly HashSet<string> _finalStates = new();

    // todo Add support for multiple character states
    private readonly Dictionary<string, Dictionary<string, HashSet<string>>> _transitions = new();


    public AutomatonBuilder(string automatonName, string initialState)
    {
        _automatonName = automatonName;
        _initialState = initialState;
        _transitions.Add(initialState, new Dictionary<string, HashSet<string>>());
    }

    public AutomatonBuilder AddTransition(string from, string path, string to)
    {
        if (!_transitions.ContainsKey(from))
            _transitions.Add(from, new Dictionary<string, HashSet<string>>());
        var transition = _transitions[from];
        if (!transition.ContainsKey(path))
            transition.Add(path, new HashSet<string>());
        transition[path].Add(to);
        return this;
    }

    public AutomatonBuilder AddFinalState(string state)
    {
        _finalStates.Add(state);
        return this;
    }

    //todo remove recursion

    private bool IsNondeterministic() =>
        _transitions.Values.Any(state => state.Values.Any(transition => transition.Count > 1));

    private void RemoveNondeterminism()
    {
        List<(string, string, string)> toAdd = new();
        while (IsNondeterministic())
        {
            foreach (var state in _transitions.Values)
            {
                foreach (var transition in state.Values)
                {
                    if (transition.Count <= 1) continue;
                    var newTransition = transition.Aggregate("", (current, path) => current + path);
                    if (transition.Any(to => _finalStates.Contains(to))) _finalStates.Add(newTransition);
                    foreach (var target in transition)
                    {
                        if (!_transitions.ContainsKey(target)) continue;
                        foreach (var path in _transitions[target])
                        {
                            foreach (var newTarget in path.Value)
                            {
                                toAdd.Add((newTransition, path.Key, newTarget));
                            }
                        }
                    }

                    transition.Clear();
                    transition.Add(newTransition);
                }
            }

            foreach (var addition in toAdd)
            {
                AddTransition(addition.Item1, addition.Item2, addition.Item3);
            }
        }
    }

    private bool ConstructPathways(IReadOnlyDictionary<string, Dictionary<string, string>> transitions, State current,
        IDictionary<string, State> allStates)
    {
        if (!_transitions.ContainsKey(current.Name)) return _finalStates.Contains(current.Name);
        var targets = transitions[current.Name];
        foreach (var (path, to) in targets)
        {
            if (allStates.TryGetValue(to, out var state))
            {
                current.AddTransition(path, state);
                continue;
            }

            var newState = new State(to);
            current.AddTransition(path, newState);
            allStates.Add(to, newState);
            if (!ConstructPathways(transitions, newState, allStates))
            {
                current.RemoveTransition(path);
            }
            
        }

        return current.HasTransitions() || _finalStates.Contains(current.Name);
    }

    private Dictionary<string, Dictionary<string, string>> DeterministicTransitions()
    {
        var deterministicTransitions = new Dictionary<string, Dictionary<string, string>>();
        foreach (var transition in _transitions)
        {
            deterministicTransitions.Add(transition.Key, new Dictionary<string, string>());
            foreach (var path in transition.Value)
            {
                deterministicTransitions[transition.Key].Add(path.Key, path.Value.First());
            }
        }

        return deterministicTransitions;
    }

    public Automaton Build()
    {
        RemoveNondeterminism();
        var deterministicTransitions = DeterministicTransitions();
        var initialState = new State(_initialState);
        var allStates = new Dictionary<string, State>();
        ConstructPathways(deterministicTransitions, initialState, allStates);

        return new Automaton(_automatonName, initialState);
    }
}