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


    public class AutomatonBuilder
    {
        private readonly string _automatonName;
        private State _initialState;
        private readonly HashSet<State> _states = new();

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
        
        public AutomatonBuilder AddTransition(string from, string to, string symbols)
        {
            var fromState = CreateStateIfNeeded(from);
            var toState = CreateStateIfNeeded(to);

            foreach (var symbol in symbols)
            {
                fromState.AddTransition(symbol, toState);
            }

            return this;
        }

        public AutomatonBuilder AddTransition(string from, string to, char symbol)
        {
            var fromState = CreateStateIfNeeded(from);
            var toState = CreateStateIfNeeded(to);

            fromState.AddTransition(symbol, toState);

            return this;
        }

        public AutomatonBuilder RemoveState(State state)
        {
            _states.Remove(state);
            foreach (var s in _states)
            {
                s.RemoveTransitions(state);
            }

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