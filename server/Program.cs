using Microsoft.AspNetCore.Mvc;
using server;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

DataBaseState db()
{
	return DataBaseSingleton.Instance.database;
}

app.UseHttpsRedirection();

app.MapGet("/trabajadores", () =>
{
	return db().trabajadores;
});

app.MapGet("/trabajadores/{cedula}", (uint cedula) =>
{
	return JsonSerializer.Serialize(db().trabajadores.Find((x) => x.cedula.Equals(cedula)));
});

app.MapPost("/trabajadores", (Trabajador trabajador) =>
{
	if (db().trabajadores.Contains(trabajador))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().trabajadores.Add(trabajador);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/usuarios", () =>
{
	return db().usuarios;
});

app.MapGet("/usuarios/{cedula}", (uint cedula) =>
{
	return JsonSerializer.Serialize(db().usuarios.Find((x) => x.cedula.Equals(cedula)));
});

app.MapPost("/usuarios", (Usuario user) =>
{
	if (db().usuarios.Contains(user))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().usuarios.Add(user);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/maletas", () =>
{
	return db().maletas;
});

app.MapPost("/maletas", (MaletaQData data) =>
{

	uint numero = 0;

	if (db().maletas.Count() != 0)
	{
		numero = db().maletas.Last().numero + 1;
	}
	var maleta = new Maleta(numero,data.cedula_usuario, data.color, data.peso, data.costo_envio);
	if (db().maletas.Contains(maleta))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().maletas.Add(maleta);
		DataBaseSingleton.Instance.save_state();
		return $"{{\"numero\": {numero}, \"success\": 1}}";
	}
});

app.MapGet("/maletas/info/{numero}", (uint numero) =>
{
	return JsonSerializer.Serialize(db().maletas.Find((maleta) => maleta.numero.Equals(numero)));
});
app.MapGet("/maletas/usuario/{cedula}", (uint cedula) =>
{
	return JsonSerializer.Serialize(db().maletas.Find((maleta) => maleta.cedula_usuario.Equals(cedula)));
});

app.MapGet("/bagcarts", () =>
{
	return db().bagcarts;
});

app.MapPost("/bagcarts", (BagCartQData data) =>
{
	uint id = 0;

	if (db().bagcarts.Count() != 0)
	{
		id = db().bagcarts.Last().id + 1;
	}
	var bagcart = new BagCart(id, data.marca, data.modelo);
	if (db().bagcarts.Contains(bagcart))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().bagcarts.Add(bagcart);
		DataBaseSingleton.Instance.save_state();
		return $"{{\"id\": {id}, \"success\": 1}}";
	}
});

app.MapGet("/bagcarts/info/{id}", (uint id) =>
{
	return JsonSerializer.Serialize(db().bagcarts.Find((x) => x.id.Equals(id)));
});

app.MapGet("/tipo_avion", () =>
{
	return db().tipos_avion;
});

app.MapPost("/tipos_avion", (TipoAvion data) =>
{

	if (db().tipos_avion.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().tipos_avion.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/tipos_avion/{nombre}", (string nombre) =>
{
	return JsonSerializer.Serialize(db().tipos_avion.Find((x) => x.nombre.Equals(nombre)));
});

app.MapGet("/vuelos", () =>
{
	return db().vuelos;
});

app.MapPost("/vuelos", (VueloQData data) =>
{
	uint id = 0;

	if (db().vuelos.Count() != 0)
	{
		id = db().vuelos.Last().id + 1;
	}
	var vuelo = new Vuelo(id, data.tipo_avion);
	if (db().vuelos.Contains(vuelo) || !db().tipos_avion.Exists((x) => x.nombre.Equals(data.tipo_avion)))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().vuelos.Add(vuelo);
		DataBaseSingleton.Instance.save_state();
		return $"{{\"id\": {id}, \"success\": 1}}";
	}
});

app.MapGet("/vuelos/info/{id}", (int id) =>
{
	return JsonSerializer.Serialize(db().vuelos.Find((x) => x.id.Equals(id)));
});

app.MapGet("/rel/scan_rayosx_maleta", () =>
{
	return db().rel_scan_rayosx_maleta;
});

app.MapPost("/rel/scan_rayosx_maleta", (RelScanRayosXMaleta data) =>
{

	if (db().rel_scan_rayosx_maleta.Contains(data) ||
		!db().trabajadores.Exists(x => x.cedula.Equals(data.cedula_trabajador)))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().rel_scan_rayosx_maleta.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/scan_rayosx/maleta/{numero}", (uint numero) =>
{
	return JsonSerializer.Serialize(db().rel_scan_rayosx_maleta.Find((x) => x.numero_maleta.Equals(numero)));
});

app.MapGet("/rel/scan_rayosx/trabajador/{cedula}", (uint cedula) =>
{
	return JsonSerializer.Serialize(db().rel_scan_rayosx_maleta.Find((x) => x.cedula_trabajador.Equals(cedula)));
});

app.MapGet("/rel/maleta_bagcart", () =>
{
	return db().rel_maleta_bagcart;
});

app.MapPost("/rel/maleta_bagcart", (RelMaletaBagCart data) =>
{

	if (db().rel_maleta_bagcart.Contains(data) ||
		!db().rel_scan_rayosx_maleta.Exists((x) => x.numero_maleta.Equals(data.numero_maleta) &&
			x.aceptada == true) ||
		!db().rel_vuelo_bagcart.Exists(x=>x.id_bagcart.Equals(data.id_bagcart)))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().rel_scan_rayosx_maleta.RemoveAll((reg) => reg.numero_maleta.Equals(data.numero_maleta));
		db().rel_maleta_bagcart.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/maleta_bagcart/maleta/{numero}", (uint numero) =>
{
	return JsonSerializer.Serialize(db().rel_maleta_bagcart.Find((x) => x.numero_maleta.Equals(numero)));
});

app.MapGet("/rel/maleta_bagcart/bagcart/{id}", (uint id) =>
{
	return JsonSerializer.Serialize(db().rel_maleta_bagcart.Find((x) => x.id_bagcart.Equals(id)));
});

app.MapGet("/rel/scan_asignacion_maleta", () =>
{
	return db().rel_scan_asignacion_maleta;
});

app.MapPost("/rel/scan_asignacion_maleta", (RelScanAsignacionMaleta data) =>
{

	if (db().rel_scan_asignacion_maleta.Contains(data) ||
		!db().rel_maleta_bagcart.Exists((reg) => reg.numero_maleta.Equals(data.numero_maleta))|| 
		!db().trabajadores.Exists(x => x.cedula.Equals(data.cedula_trabajador)))
	{
		return "{\"success\": 0}";
	}
	else
	{
		//remover maleta del bagcart al pasarla al avion
		db().rel_maleta_bagcart.RemoveAll((reg) => reg.numero_maleta.Equals(data.numero_maleta));
		db().rel_scan_asignacion_maleta.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/scan_asignacion_maleta/maleta/{numero}", (uint numero) =>
{
	return JsonSerializer.Serialize(db().rel_scan_asignacion_maleta.Find((x) => x.numero_maleta.Equals(numero)));
});

app.MapGet("/rel/scan_asignacion_maleta/trabajador/{cedula}", (uint cedula) =>
{
	return JsonSerializer.Serialize(db().rel_scan_asignacion_maleta.Find((x) => x.cedula_trabajador.Equals(cedula)));
});

app.MapGet("/rel/scan_asignacion_maleta/vuelo/{id}", (uint id) =>
{
	return JsonSerializer.Serialize(db().rel_scan_asignacion_maleta.Find((x) => x.id_vuelo.Equals(id)));
});

app.MapGet("/rel/vuelo_bagcart", () =>
{
	return db().rel_vuelo_bagcart;
});

app.MapPost("/rel/vuelo_bagcart", (RelVueloBagCart data) =>
{
	if (db().rel_vuelo_bagcart.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		db().rel_vuelo_bagcart.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/vuelo_bagcart/bagcart/{id}", (uint id) =>
{
	return JsonSerializer.Serialize(db().rel_vuelo_bagcart.Find((x) => x.id_bagcart.Equals(id)));
});

app.MapGet("/rel/vuelo_bagcart/vuelo/{id}", (uint id) =>
{
	return JsonSerializer.Serialize(db().rel_vuelo_bagcart.Find((x) => x.id_vuelo.Equals(id)));
});

app.MapGet("/rel/vuelo_bagcart/sello/{sello}", (string sello) =>
{
	return JsonSerializer.Serialize(db().rel_vuelo_bagcart.Find((x) => x.sello.Equals(sello))); ;
});

app.MapGet("/reportes/maletas_x_cliente/{cedula}", (uint cedula) =>
{
	var result = "";
	Usuario? usuario = db().usuarios.Find((usuario) => usuario.cedula.Equals(cedula));
	if (usuario != null)
	{
		List<Maleta> maletas =
			db().maletas.FindAll(
				(maleta) => maleta.cedula_usuario.Equals(usuario.cedula));
		return JsonSerializer.Serialize(new MaletasXCliente(maletas, usuario));
	}
	return result;
});

app.MapGet("/reportes/conciliacion_maletas/{id_vuelo}", (uint id_vuelo) =>
{
	var result = "";
	Vuelo? vuelo = db().vuelos.Find((vuelo) => vuelo.id.Equals(id_vuelo));
	TipoAvion? tipo_avion = db().tipos_avion.Find((tipo) => tipo.nombre.Equals(vuelo?.tipo_avion));

	if (vuelo != null && tipo_avion != null)
	{
		List<RelScanAsignacionMaleta> rel_maletas_en_avion =
			db().rel_scan_asignacion_maleta.FindAll(
				(x) => x.id_vuelo.Equals(id_vuelo));

		List<RelVueloBagCart> rel_vuelos_de_bagcarts =
			db().rel_vuelo_bagcart.FindAll(
				(x) => x.id_vuelo.Equals(id_vuelo));

		List<RelMaletaBagCart> rel_maletas_en_bagcarts =
			db().rel_maleta_bagcart.FindAll(
				(x) => rel_vuelos_de_bagcarts.Exists(
					(reg) => reg.id_bagcart.Equals(x.id_bagcart)
			));

		List<RelScanRayosXMaleta> rel_maletas_rechazadas =
			db().rel_scan_rayosx_maleta.FindAll(
				(x) => x.aceptada.Equals(false));

		int maletas_en_avion = rel_maletas_en_avion.Count();
		int maletas_en_bagcarts = rel_maletas_en_bagcarts.Count();
		int maletas_rechazadas = rel_maletas_rechazadas.Count();
		int total_maletas = maletas_en_avion + maletas_en_bagcarts + maletas_rechazadas;

		return JsonSerializer.Serialize(
			new ConciliacionMaletas(vuelo.id, tipo_avion.nombre, tipo_avion.capacidad, 
				total_maletas, maletas_en_bagcarts, maletas_en_avion, maletas_rechazadas)
		);
	}
	return result;
});

app.Run();
