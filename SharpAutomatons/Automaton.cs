namespace SharpAutomatons;

public class Automaton
{
    private readonly string _name;

    private readonly State _initialState;
    // todo add check if is deterministic


    protected Automaton(string name, State initialState)
    {
        _name = name;
        _initialState = initialState;
    }

    public bool Test(string input)
    {
        var currentState = _initialState;
        foreach (var symbol in input)
        {
            var nextState = currentState.GetNextState(symbol);

            if (nextState == null) // get rid of this check by using hell state
            {
                return false;
            }
        }

        return currentState.IsFinal;
    }


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

        public AutomatonBuilder InitialState(State initialState)
        {
            _initialState = initialState;
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

        public AutomatonBuilder RemoveTransition(string from, char symbol)
        {
            var state = _states.FirstOrDefault(x => x.Name == from);
            if (state == null)
            {
                return this;
            }

            state.RemoveTransitions(symbol);
            return this;
        }

        public AutomatonBuilder MakeFinal(State state)
        {
            state.IsFinal = true;
            return this;
        }

        public Automaton Build()
        {
            return new Automaton(_automatonName, _initialState);
        }
    }
}