namespace server;
using System.Text.Json;

public sealed class DataBaseSingleton
{
	public DataBaseState database { get; set; }
	private static object syncRoot = new object();
	private static readonly Lazy<DataBaseSingleton> lazy =
		new Lazy<DataBaseSingleton>(() => new DataBaseSingleton());

	public static DataBaseSingleton Instance { get { return lazy.Value; } }
	private DataBaseSingleton()
	{
		string filepath = Environment.CurrentDirectory + "/Data.json";
		if (!File.Exists(filepath))
		{
			database = new DataBaseState();
		}else{
			string jsonString = File.ReadAllText(filepath);
			var possible_db = JsonSerializer.Deserialize<DataBaseState>(jsonString);
			if (possible_db == null){
				database = new DataBaseState();
			}else{
				database = possible_db;
			}
		}
	}
	public void save_state()
	{
		var options = new JsonSerializerOptions { WriteIndented = true };
		string jsonString = JsonSerializer.Serialize(database, options);
		File.WriteAllText(Environment.CurrentDirectory + "/Data.json", jsonString);
	}
}