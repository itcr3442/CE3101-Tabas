namespace server;
[Serializable]
public enum Rol{
	Embarcador,
	Scan, 
	Administrador,
	Recepcionista,
}

[Serializable]
public enum TipoAvion: uint{
	Pasajeros = 223,
	Cargo = 232,
	Combi = 64,
}
public class DataBaseState{
	public IList<Trabajador>? trabajadores{get;set;}
	public IList<Usuario>? usuarios{get;set;}
	public IList<Vuelo>? vuelos{get;set;}
	public IList<BagCart>? bagcarts{get;set;}
	public IList<Maleta>? maletas{get;set;}
	public IList<RelUsuarioMaleta>? rel_usuario_maleta{get;set;}
	public IList<RelScanRayosXMaleta>? rel_scan_rayosx_maleta{get;set;}
	public IList<RelScanAsignacionMaleta>? rel_scan_asignacion_maleta{get;set;}
	public IList<RelMaletaBagCart>? rel_maleta_bagcart{get;set;}
	public IList<RelVueloBagCart>? rel_vuelo_bagcart{get;set;}

}

public class Trabajador{
	public uint cedula{get; set;}
	public string? nombre{get; set;}
	public string? primer_apellido{get; set;}
	public string? segundo_apellido{get; set;}

}

public class Usuario{
	public uint cedula{get; set;}
	public string? nombre_complet{get; set;}
	public uint telefono{get; set;}
}
public class Maleta{
	public uint numero{get; set;}
	public string? color{get; set;}
	public decimal peso{get; set;}
	public decimal costo_envio{get; set;}
}
public class BagCart{
	public uint id{get; set;}
	public string? marca{get; set;}
	public uint modelo{get; set;}
}
public class Vuelo{
	public uint id{get; set;}
	public TipoAvion tipo_avion{get; set;}
}

public class RelUsuarioMaleta{

	public uint cedula_usuario{get; set;}
	public uint numero_maleta{get; set;}
}
public class RelScanRayosXMaleta{

	public uint cedula_trabajador{get; set;}
	public uint numero_maleta{get; set;}
	public bool aceptada{get; set;}
	public string? comentarios{get; set;}
}
public class RelScanAsignacionMaleta{

	public uint cedula_trabajador{get; set;}
	public uint numero_maleta{get; set;}
	public string? id_vuelo{get; set;}
}
public class RelMaletaBagCart{

	public uint numero_maleta{get; set;}
	public uint id_bagcart{get; set;}
}
public class RelVueloBagCart{

	public string? id_vuelo{get; set;}
	public uint id_bagcart{get; set;}
	public string? sello{get; set;}
}