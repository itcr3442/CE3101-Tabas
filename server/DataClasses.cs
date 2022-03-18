namespace server;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;


/// <summary>
/// Contiene el estado de la base de datos. Esta es la estructura que se 
/// serializa/deserializa para interactuar con el archivo JSON que contiene
/// los datos
/// </summary>
public class DataBaseState
{
	public List<Trabajador> trabajadores { get; set; }
	public List<Usuario> usuarios { get; set; }
	public List<TipoAvion> tipos_avion { get; set; }
	public List<Avion> aviones { get; set; }
	public List<Vuelo> vuelos { get; set; }
	public List<BagCart> bagcarts { get; set; }
	public List<Maleta> maletas { get; set; }
	public List<Rol> roles { get; set; }
	public List<RelScanRayosXMaleta> rel_scan_rayosx_maleta { get; set; }
	public List<RelScanAsignacionMaleta> rel_scan_asignacion_maleta { get; set; }
	public List<RelMaletaBagCart> rel_maleta_bagcart { get; set; }
	public List<RelVueloBagCart> rel_vuelo_bagcart { get; set; }

	public DataBaseState()
	{
		trabajadores = new List<Trabajador>();
		usuarios = new List<Usuario>();
		vuelos = new List<Vuelo>();
		tipos_avion = new List<TipoAvion>();
		aviones = new List<Avion>();
		bagcarts = new List<BagCart>();
		maletas = new List<Maleta>();
		roles = new List<Rol>();
		rel_scan_rayosx_maleta = new List<RelScanRayosXMaleta>();
		rel_scan_asignacion_maleta = new List<RelScanAsignacionMaleta>();
		rel_maleta_bagcart = new List<RelMaletaBagCart>();
		rel_vuelo_bagcart = new List<RelVueloBagCart>();
	}

}
/// <summary>
/// DataClass utilizada para almacenar los datos sobre un tipo de avion.
/// </summary>
public class TipoAvion
{
	/// <summary>
	/// Nombre del tipo de avion. Se utiliza como llave primaria. 
	/// </summary>
	/// <value>"Boeing 787"</value>
	public string? nombre { get; set; }
	/// <summary>
	///	Indica la cantidad de maletas que puede almacenar un tipo de avion. 
	/// </summary>
	/// <value>23</value>
	public uint capacidad { get; set; }
	/// <summary>
	///	Se refieren a los espacios destinados a maletas en el avión 
	/// </summary>
	/// <value>3</value>
	public uint secciones_bodega { get; set; }
}

/// <summary>
/// DataClass utilizada para almacenar los datos sobre un avion en específico.
/// </summary>
public class Avion
{
	public uint nserie { get; set; }
	public uint horas_uso { get; set; }
	public string? modelo { get; set; }
}


/// <summary>
/// DataClass utilizada para almacenar los datos sobre un vuelo.
/// </summary>
public class Vuelo
{
	public uint numero { get; set; }
	public uint avion { get; set; }
}

/// <summary>
/// DataClass utilizada para recepción de los datos necesarios para crear
/// una instancia de un vuelo en la base de datos. No se recibe un número
/// de vuelo pues la REST API se encarga de asignarlo para evitar conflictos
/// </summary>
public class VueloQData
{
	public uint avion { get; set; }
}

public class Rol
{
	public string? nombre { get; set; }
	public string? descripcion { get; set; }
}

public class Trabajador
{
	public uint cedula { get; set; }
	public string? password_hash { get; set; }
	public string? nombre { get; set; }
	public string? primer_apellido { get; set; }
	public string? segundo_apellido { get; set; }
	public string? rol { get; set; }
	public Trabajador(){}

}

public class Usuario
{
	public uint cedula { get; set; }
	public string? password_hash { get; set; }
	public string? nombre { get; set; }
	public string? primer_apellido { get; set; }
	public string? segundo_apellido { get; set; }
	public uint telefono { get; set; }
}

public class Maleta
{
	public uint numero { get; set; }
	public uint cedula_usuario { get; set; }
	public int color { get; set; }
	public decimal peso { get; set; }
	public decimal costo_envio { get; set; }
	public uint nvuelo { get; set; }
}

public class MaletaQData
{
	public uint cedula_usuario { get; set; }
	public uint nvuelo { get; set; }
	public int color { get; set; }
	public decimal peso { get; set; }
	public decimal costo_envio { get; set; }
}

public class BagCart
{
	public uint id { get; set; }
	public string marca { get; set; }
	public uint modelo { get; set; }

	public BagCart(uint id, string marca, uint modelo)
	{
		this.id = id;
		this.marca = marca;
		this.modelo = modelo;
	}
}

public class BagCartQData
{
	public string marca { get; set; }
	public uint modelo { get; set; }
	public BagCartQData()
	{
		marca = "Genérica";
	}
}

public class RelScanRayosXMaleta
{

	public uint cedula_trabajador { get; set; }
	public uint numero_maleta { get; set; }
	public bool aceptada { get; set; }
	public string? comentarios { get; set; }
}

public class RelScanAsignacionMaleta
{

	public uint cedula_trabajador { get; set; }
	public uint numero_maleta { get; set; }
}

public class RelMaletaBagCart
{

	public uint numero_maleta { get; set; }
	public uint id_bagcart { get; set; }
}

public class RelVueloBagCart
{

	public uint id_vuelo { get; set; }
	public uint id_bagcart { get; set; }
	public string sello { get; set; }

	public RelVueloBagCart()
	{
		sello = "";
	}
}

public class QRelVueloBagCart
{
	public uint id_vuelo { get; set; }
	public uint id_bagcart { get; set; }
}

public class MaletasXCliente
{
	public List<Maleta> maletas { get; set; }
	public Usuario usuario { get; set; }
	public MaletasXCliente(List<Maleta> maletas, Usuario usuario)
	{
		this.maletas = maletas;
		this.usuario = usuario;
	}
}

public class ConciliacionMaletas
{
	public uint numero_vuelo { get; set; }
	public string tipo_avion { get; set; }
	public uint capacidad { get; set; }
	public int total_maletas { get; set; }
	public int maletas_en_bagcarts { get; set; }
	public int maletas_en_avion { get; set; }
	public int maletas_rechazadas { get; set; }
	public ConciliacionMaletas(uint numero_vuelo, string tipo_avion, uint capacidad, int total_maletas, int maletas_en_bagcarts, int maletas_en_avion, int maletas_rechazadas)
	{
		this.numero_vuelo = numero_vuelo;
		this.tipo_avion = tipo_avion;
		this.capacidad = capacidad;
		this.total_maletas = total_maletas;
		this.maletas_en_bagcarts = maletas_en_bagcarts;
		this.maletas_en_avion = maletas_en_avion;
		this.maletas_rechazadas = maletas_rechazadas;
	}

}