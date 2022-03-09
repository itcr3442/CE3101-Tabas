namespace server;
using System.Text.Json;
[Serializable]
public enum Rol
{
	Embarcador,
	Scan,
	Administrador,
	Recepcionista,
}

public class TipoAvion
{
	public string nombre {get;set;}
	public uint capacidad {get;set;}
	public TipoAvion(string nombre, uint capacidad){
		this.nombre = nombre;
		this.capacidad = capacidad;
	}
}

public class DataBaseState
{
	public List<Trabajador> trabajadores { get; set; }
	public List<Usuario> usuarios { get; set; }
	public List<TipoAvion> tipos_avion { get; set; }
	public List<Vuelo> vuelos { get; set; }
	public List<BagCart> bagcarts { get; set; }
	public List<Maleta> maletas { get; set; }
	public List<RelUsuarioMaleta> rel_usuario_maleta { get; set; }
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
		bagcarts = new List<BagCart>();
		maletas = new List<Maleta>();
		rel_usuario_maleta = new List<RelUsuarioMaleta>();
		rel_scan_rayosx_maleta = new List<RelScanRayosXMaleta>();
		rel_scan_asignacion_maleta = new List<RelScanAsignacionMaleta>();
		rel_maleta_bagcart = new List<RelMaletaBagCart>();
		rel_vuelo_bagcart = new List<RelVueloBagCart>();
	}

}

public class Trabajador
{
	public uint cedula { get; set; }
	public string? nombre { get; set; }
	public string? primer_apellido { get; set; }
	public string? segundo_apellido { get; set; }

}

public class Usuario
{
	public uint cedula { get; set; }
	public string? nombre_completo { get; set; }
	public uint telefono { get; set; }
}

public class Maleta
{
	public uint numero { get; set; }
	public int color { get; set; }
	public decimal peso { get; set; }
	public decimal costo_envio { get; set; }
	public Maleta(uint numero, int color, decimal peso, decimal costo_envio)
	{
		this.numero = numero;
		this.color = color;
		this.peso = peso;
		this.costo_envio = costo_envio;
	}
}

public class MaletaQData
{
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

public class Vuelo
{
	public uint id { get; set; }
	public string tipo_avion { get; set; }
	public Vuelo(uint id, string tipo_avion)
	{
		this.id = id;
		this.tipo_avion = tipo_avion;
	}
}

public class VueloQData{
	public string tipo_avion{get;set;}
	public VueloQData(){
		tipo_avion = "None";
	}
}

public class RelUsuarioMaleta
{

	public uint cedula_usuario { get; set; }
	public uint numero_maleta { get; set; }
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
	public uint id_vuelo { get; set; }
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

	public RelVueloBagCart(){
		sello = "";
	}
}


public class MaletasXCliente{
	public List<Maleta> maletas {get;set;}
	public Usuario usuario{get;set;} 
	public MaletasXCliente(List<Maleta> maletas, Usuario usuario){
		this.maletas = maletas;
		this.usuario = usuario;
	}
}
public class ConciliacionMaletas{
	public uint numero_vuelo {get;set;}
	public string tipo_avion {get;set;}
	public uint capacidad {get;set;}
	public int total_maletas{get;set;}
	public int maletas_en_bagcarts{get;set;}
	public int maletas_en_avion{get;set;}
	public int maletas_rechazadas{get;set;}
	public ConciliacionMaletas(uint numero_vuelo, string tipo_avion, uint capacidad, int total_maletas, int maletas_en_bagcarts, int maletas_en_avion, int maletas_rechazadas){
		this.numero_vuelo = numero_vuelo;
		this.tipo_avion = tipo_avion;
		this.capacidad = capacidad;
		this.total_maletas = total_maletas;
		this.maletas_en_bagcarts = maletas_en_bagcarts;
		this.maletas_en_avion = maletas_en_avion;
		this.maletas_rechazadas = maletas_rechazadas;
	}

}