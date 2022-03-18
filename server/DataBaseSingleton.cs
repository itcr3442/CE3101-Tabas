namespace server;
using System.Text.Json;

/// <summary>
/// Singleton que evita acceso múltiple a los datos de la base de datos
/// </summary>
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
            // se crean algunos datos iniciales para poder interactuar con la base de datos
			database = new DataBaseState();
			Trabajador[] x = {
				new Trabajador{ cedula = 42069, password_hash = "cusadmin", nombre = "admin",
				primer_apellido = "",segundo_apellido = "", rol = "admin"},
				new Trabajador{ cedula = 69420, password_hash = "cusadmin", nombre = "recep1",
				primer_apellido = "",segundo_apellido = "", rol = "recepcionista"},
			};
			foreach (var item in x)
			{
				database.trabajadores.Add(item);
			}

			Rol[] y = {
				new Rol{nombre = "admin", descripcion = "administrative role"},
				new Rol{nombre = "scan", descripcion = "escaneador de rayos x"},
				new Rol{nombre = "recepcionista",descripcion = "recepcionista"},
				new Rol{nombre = "embarcador",descripcion = "sube las maletas al avion"},
			};
			foreach (var item in y)
			{
				database.roles.Add(item);
			}

			TipoAvion[] z = {
				new TipoAvion{nombre = "Boeing 787 Max",capacidad=30, secciones_bodega= 3},
				new TipoAvion{nombre = "Boeing 787",capacidad=30, secciones_bodega= 3},
			};
			foreach (var item in z)
			{
				database.tipos_avion.Add(item);
			}

			Avion[] w = {
				new Avion{nserie = 42069, modelo = "Boeing 787", horas_uso = 420},
				new Avion{nserie = 69420, modelo = "Boeing 787 Max", horas_uso = 69},
			};
			foreach (var item in w)
			{
				database.aviones.Add(item);
			}
			Usuario[] m = {
				new Usuario{cedula = 112312, password_hash = "123", nombre = "Pedro",
				primer_apellido = "Aguilar",segundo_apellido = "Zapata",telefono = 69420},
				new Usuario{cedula = 42069, password_hash = "321", nombre = "Luis",
				primer_apellido = "Vol",segundo_apellido = "Shein",telefono = 88776655}
			};
			foreach (var item in m)
			{
				database.usuarios.Add(item);
			}

			save_state();
		}
		else
		{
			string jsonString = File.ReadAllText(filepath);
			var possible_db = JsonSerializer.Deserialize<DataBaseState>(jsonString);
			if (possible_db == null)
			{
				database = new DataBaseState();
			}
			else
			{
				database = possible_db;
			}
		}
	}
	public void save_state()
	{
        // se toma el estado actual y se guarda en el archivo Data.json 
		var options = new JsonSerializerOptions { WriteIndented = true };
		string jsonString = JsonSerializer.Serialize(database, options);
		File.WriteAllText(Environment.CurrentDirectory + "/Data.json", jsonString);
	}
}
