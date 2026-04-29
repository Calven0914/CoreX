using Sandbox;

public abstract class BaseCommand
{
    public abstract string Name { get; }
    public virtual string[] Aliases => new string[] { };
    public abstract string RequiredPermission { get; }
    public abstract string Usage { get; }

    public abstract void Execute( GameObject caller, string[] args );
}