namespace SharpAutomatons;

public class AutomatonBuilder
{
    private readonly string _automatonName;

    private readonly string _initialState;
    private readonly HashSet<string> _finalStates = new();

    private readonly Dictionary<string, Dictionary<string, HashSet<string>>> _rules = new();


    public AutomatonBuilder(string automatonName, string initialState)
    {
        _automatonName = automatonName;
        _initialState = initialState;
        _rules.Add(initialState, new Dictionary<string, HashSet<string>>());
    }

    public AutomatonBuilder AddTransition(string from, string path, string to)
    {
        if (!_rules.ContainsKey(from))
            _rules.Add(from, new Dictionary<string, HashSet<string>>());
        var transition = _rules[from];
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

    private bool IsNondeterministic() =>
        _rules.Values.Any(state => state.Values.Any(transition => transition.Count > 1));

    private void RemoveNondeterminism()
    {
        while (IsNondeterministic())
        {
            var toFix = FindAllNondeterministicRules();

            foreach (var (state, character) in toFix)
            {
                var targets = _rules[state][character];
                var newStateName = targets.Aggregate("", (current, target) => current + target);
                if (!_rules.ContainsKey(newStateName))
                {
                    foreach (var oldTarget in targets)
                    {
                        if (!_rules.ContainsKey(oldTarget)) continue;
                        var oldTargetTransitions = _rules[oldTarget];
                        foreach (var (path, oldTargets) in oldTargetTransitions)
                        {
                            foreach (var target in oldTargets)
                            {
                                AddTransition(newStateName, path, target);
                            }
                        }
                    }
                }

                _rules[state][character].Clear();
                _rules[state][character].Add(newStateName);
            }
        }
    }

    private void RemoveEpsilonTransitions()
    {
        // todo
    }

    // maybe instead of searching for them I can keep a list of them and update it when adding the state / transition / rule
    private IEnumerable<(string, string)> FindAllNondeterministicRules()
    {
        // rewrite to linq
        var result = new List<(string, string)>();
        foreach (var (state, rule) in _rules)
        {
            foreach (var (character, targets) in rule)
            {
                if (targets.Count > 1)
                    result.Add((state, character));
            }
        }

        return result;
    }

    private bool ConstructPathways(IReadOnlyDictionary<string, Dictionary<string, string>> transitions, State current,
        IDictionary<string, State> allStates)
    {
        if (!_rules.ContainsKey(current.Name)) return _finalStates.Contains(current.Name);
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
        foreach (var transition in _rules)
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
        // merge duplicit states (both have the same ins and outs)
        var allStates = new Dictionary<string, State>();
        ConstructPathways(deterministicTransitions, initialState, allStates);

        return new Automaton(_automatonName, initialState);
    }
}