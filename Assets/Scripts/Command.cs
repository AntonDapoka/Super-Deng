
public abstract class Command
{
    public abstract void Execute();

    public virtual void Undo(){}
}
