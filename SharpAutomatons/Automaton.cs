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
        private string _automatonName;
        private State _initialState;
        private HashSet<State> _states = new HashSet<State>();

        public AutomatonBuilder(string automatonName)
        {
            _automatonName = automatonName;
        }

        public AutomatonBuilder InitialState(State initialState)
        {
            _initialState = initialState;
            return this;
        }

        public AutomatonBuilder AddState(string from, string to, char symbol)
        {
            State CreateStateIfNeeded(string target)
            {
                var state = _states.FirstOrDefault(s => s.Name == target);
                if (state != null) return state;

                state = new State(target);
                _states.Add(state);

                return state;
            }

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

        public Automaton Build()
        {
            return new Automaton(_automatonName, _initialState);
        }
    }
}