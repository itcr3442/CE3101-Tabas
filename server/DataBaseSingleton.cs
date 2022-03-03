namespace server;
using System.Text.Json;

public sealed class DataBaseSingleton
{
	public DataBaseState database { get; set; }
	private static readonly DataBaseSingleton instance = new DataBaseSingleton();
	private DataBaseSingleton()
	{
		database = new DataBaseState();
	}
	static DataBaseSingleton() { }
	public static DataBaseSingleton Instance
	{
		get { return instance; }
	}

}