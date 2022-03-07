namespace server;
using System.Text.Json;

public sealed class DataBaseSingleton
{
	public DataBaseState database { get; set; }
	private static object syncRoot = new object();
	private static volatile DataBaseSingleton instance;
	private DataBaseSingleton()
	{
		database = new DataBaseState();
	}
	static DataBaseSingleton() { }
	public static DataBaseSingleton Instance
	{
		get
		{
			if (instance == null)
			{
				lock (syncRoot)
				{
					if (instance == null)
						instance = new Singleton();
				}
			}

			return instance;
		}
		get { return instance; }
	}

}