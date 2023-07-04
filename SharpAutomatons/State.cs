namespace SharpAutomatons;

public class State
{
    
    private Dictionary<string, State> _transitions = new();
    public string Name { get; init; }
    public bool IsFinal { get; init; }
    
    public State(string name, bool isFinal = false)
    {
        Name = name;
        IsFinal = isFinal;
    }
    
    public State AddTransition(string path, State to)
    {
        _transitions.Add(path, to);
        return this;
    }
    
    public State RemoveTransition(string path)
    {
        _transitions.Remove(path);
        return this;
    }
    
    public bool HasTransitions()
    {
        return _transitions.Count > 0;
    }
    
    public string? GetNext(string path)
    {
        return _transitions.TryGetValue(path, out var transition) ? transition.Name : null;
    }
}