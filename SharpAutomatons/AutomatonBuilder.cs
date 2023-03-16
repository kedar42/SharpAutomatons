namespace SharpAutomatons;

public class AutomatonBuilder
{
    private readonly string _automatonName;
    private State _initialState;
    private readonly HashSet<State> _states = new();

    // todo make a separate class for nondeterministic state
    // todo make method that makes automaton deterministic before building
    // todo make method that removes unreachable states before building
    // todo look for more optimizations

    public AutomatonBuilder(string automatonName)
    {
        _automatonName = automatonName;
    }

    public AutomatonBuilder InitialState(string stateName)
    {
        var state = GetState(stateName);
        if (state == null)
        {
            return this;
        }
        _initialState = state;
        return this;
    }

    private State CreateStateIfNeeded(string target)
    {
        var state = _states.FirstOrDefault(s => s.Name == target);
        if (state != null) return state;

        state = new State(target);
        _states.Add(state);

        return state;
    }

    public AutomatonBuilder AddState(string name, bool isFinal = false)
    {
        var state = new State(name, isFinal);
        _states.Add(state);

        return this;
    }

    public AutomatonBuilder AddTransition(string from, string to, char symbol)
    {
        var fromState = CreateStateIfNeeded(from);
        var toState = CreateStateIfNeeded(to);

        fromState.AddTransition(symbol, toState);

        return this;
    }

    private State? GetState(string name) // todo get rid of null
    {
        return _states.FirstOrDefault(x => x.Name == name);

    }
    
    public AutomatonBuilder RemoveTransition(string from, char symbol)
    {
        var state = GetState(from);
        if (state == null)
        {
            return this;
        }

        state.RemoveTransitions(symbol);
        return this;
    }

    public AutomatonBuilder MakeFinal(string stateName)
    {
        var state = GetState(stateName);
        if (state == null)
        {
            return this;
        }
        state.IsFinal = true;
        return this;
    }

    public Automaton Build()
    {
        // todo throw errors if the state of automaton is not correct
        return new Automaton(_automatonName, _initialState);
    }
}