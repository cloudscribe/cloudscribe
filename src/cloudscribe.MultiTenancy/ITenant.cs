namespace cloudscribe.Multitenancy
{
    /// <summary>
    /// Used to retreive configured TTenant instances.
    /// </summary>
    /// <typeparam name="TTenant">The type of tenant being requested.</typeparam>
    public interface ITenant<out TTenant>
	{
		TTenant Value { get; }
	}
}