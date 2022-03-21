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
				new Trabajador{ cedula = 0, password_hash = "cusadmin", nombre = "admin",
				primer_apellido = "",segundo_apellido = "", rol = "admin"},
				new Trabajador{ cedula = 111, password_hash = "cusadmin", nombre = "Julio",
				primer_apellido = "a",segundo_apellido = "a", rol = "recepcionista"},
				new Trabajador{ cedula = 222, password_hash = "cusadmin", nombre = "Julian",
				primer_apellido = "d",segundo_apellido = "d", rol = "embarcador"},
				new Trabajador{ cedula = 333, password_hash = "cusadmin", nombre = "Juliana",
				primer_apellido = "v",segundo_apellido = "a", rol = "scan"},
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
				new TipoAvion{nombre = "Boeing 787 Max",capacidad=30, secciones_bodega= 2},
				new TipoAvion{nombre = "Boeing 787",capacidad=30, secciones_bodega= 3},
				new TipoAvion{nombre = "Boeing 783 Max",capacidad=40, secciones_bodega= 5},
				new TipoAvion{nombre = "Airbus A300",capacidad=20, secciones_bodega= 5},
				new TipoAvion{nombre = "Airbus A310",capacidad=20, secciones_bodega= 4},
				new TipoAvion{nombre = "IIL-26",capacidad=80, secciones_bodega= 20},
			};
			foreach (var item in z)
			{
				database.tipos_avion.Add(item);
			}

			Avion[] w = {
				new Avion{nserie = 42069, modelo = "Boeing 787", horas_uso = 410},
				new Avion{nserie = 41079, modelo = "Boeing 787 Max", horas_uso = 65},
				new Avion{nserie = 42368, modelo = "Boeing 783 Max", horas_uso = 62},
				new Avion{nserie = 67420, modelo = "Airbus A300", horas_uso =12},
				new Avion{nserie = 68420, modelo = "Airbus A310", horas_uso = 688},
				new Avion{nserie = 11233, modelo = "IIL-26", horas_uso = 1111},
			};
			foreach (var item in w)
			{
				database.aviones.Add(item);
			}
			Vuelo[] v = {
				new Vuelo{numero = 0, avion=42368},
				new Vuelo{numero = 1, avion=41079},
				new Vuelo{numero = 2, avion=41079},
			};
			foreach (var item in v){
				database.vuelos.Add(item);
			}
			Usuario[] m = {
				new Usuario{cedula = 112312, password_hash = "123", nombre = "Pedro",
				primer_apellido = "Aguilar",segundo_apellido = "Zapata",telefono = 62420},
				new Usuario{cedula = 65563331, password_hash = "321", nombre = "Luis",
				primer_apellido = "Vol",segundo_apellido = "Shein",telefono = 88776655},
				new Usuario{cedula = 11113341, password_hash = "2231s", nombre = "Carlos",
				primer_apellido = "Mora",segundo_apellido = "Carvajal",telefono = 88776655},
				new Usuario{cedula = 711213341, password_hash = "sksksksksk", nombre = "Valeria",
				primer_apellido = "Parripula",segundo_apellido = "Salazar",telefono = 88111995}
			};
			foreach (var item in m)
			{
				database.usuarios.Add(item);
			}
			Maleta[] n ={
				new Maleta{numero = 0, cedula_usuario=112312, color=0x0, peso=1.0M,costo_envio=1.0M, nvuelo=0},
				new Maleta{numero = 1, cedula_usuario=11113341, color=0x22311, peso=2.0M,costo_envio=2.0M, nvuelo=0},
				new Maleta{numero = 2, cedula_usuario=11113341, color=0x23123, peso=2.0M,costo_envio=3.0M, nvuelo=0},
				new Maleta{numero = 3, cedula_usuario=65563331, color=0x1223, peso=2.0M,costo_envio=3.0M, nvuelo=1},
				new Maleta{numero = 4, cedula_usuario=65563331, color=0x1223, peso=5.0M,costo_envio=3.0M, nvuelo=1},
				new Maleta{numero = 5, cedula_usuario=65563331, color=0x1223, peso=5.0M,costo_envio=3.0M, nvuelo=1},
				new Maleta{numero = 6, cedula_usuario=112312, color=0x1223, peso=5.0M,costo_envio=3.0M, nvuelo=0},
				
				new Maleta{numero = 7, cedula_usuario=711213341, color=0x1223, peso=5.0M,costo_envio=3.0M, nvuelo=2},
				new Maleta{numero = 8, cedula_usuario=711213341, color=0x1223, peso=5.0M,costo_envio=3.0M, nvuelo=2},
				new Maleta{numero = 9, cedula_usuario=711213341, color=0x1223, peso=5.0M,costo_envio=3.0M, nvuelo=2},

			};
			foreach (var item in n){
				database.maletas.Add(item);
			}

			BagCart[] b = {
				new BagCart(0,"kyk",2021),
				new BagCart(1,"kyak",2022),
				new BagCart(2,"kscyk",2020),
				new BagCart(3,"kyxxxk",2019),
				new BagCart(4,"kyhhk",2018),
			};
			foreach (var item in b){
				database.bagcarts.Add(item);
			}
			RelMaletaBagCart[] rmb = {
				new RelMaletaBagCart{numero_maleta = 0, id_bagcart = 1},
				new RelMaletaBagCart{numero_maleta = 1, id_bagcart = 1},
				new RelMaletaBagCart{numero_maleta = 3, id_bagcart = 0},
				new RelMaletaBagCart{numero_maleta = 4, id_bagcart = 0},
				new RelMaletaBagCart{numero_maleta = 6, id_bagcart = 1},
			};
			foreach (var item in rmb){
				database.rel_maleta_bagcart.Add(item);
			}
			RelScanRayosXMaleta[] rsx = {
				new RelScanRayosXMaleta{cedula_trabajador = 333, numero_maleta= 0, aceptada = true, comentarios = ""},
				new RelScanRayosXMaleta{cedula_trabajador = 333, numero_maleta= 1, aceptada = true, comentarios = ""},
				new RelScanRayosXMaleta{cedula_trabajador = 333, numero_maleta= 2,aceptada = false, comentarios = "peligroso"},
				new RelScanRayosXMaleta{cedula_trabajador = 333, numero_maleta= 3, aceptada = true, comentarios = ""},
				new RelScanRayosXMaleta{cedula_trabajador = 333, numero_maleta= 4, aceptada = true, comentarios = ""},
				new RelScanRayosXMaleta{cedula_trabajador = 333, numero_maleta= 6, aceptada = true, comentarios = ""}
			};
			foreach (var item in rsx) {
				database.rel_scan_rayosx_maleta.Add(item);
			}
			RelVueloBagCart[] rvb = {
				new RelVueloBagCart{id_bagcart = 0, id_vuelo = 1, sello = "asdasdasd2123"},
				new RelVueloBagCart{id_bagcart = 1, id_vuelo = 0, sello = "asdasdasdadas"},
				new RelVueloBagCart{id_bagcart = 2, id_vuelo = 0},
				new RelVueloBagCart{id_bagcart = 3, id_vuelo = 0},
			};
			foreach (var item in rvb){
				database.rel_vuelo_bagcart.Add(item);
			}
			RelScanAsignacionMaleta[] rsam = {
				new RelScanAsignacionMaleta{numero_maleta = 0, cedula_trabajador = 222},
				new RelScanAsignacionMaleta{numero_maleta = 1, cedula_trabajador = 222},
			};
			foreach (var item in rsam){
				database.rel_scan_asignacion_maleta.Add(item);
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
