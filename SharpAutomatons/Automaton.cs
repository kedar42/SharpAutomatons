namespace SharpAutomatons;

public class Automaton
{
    public string Name { get; init; }
    public State InitialState { get; init; }


    public Automaton(string name, State initialState)
    {
        Name = name;
        InitialState = initialState;
    }
    
}