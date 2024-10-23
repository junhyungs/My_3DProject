
public interface INode
{
    public enum State
    {
        Running,
        Success,
        Fail
    }

    public State Evaluate();
}