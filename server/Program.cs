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

app.UseHttpsRedirection();

app.MapGet("/trabajadores", () =>
{
	return DataBaseSingleton.Instance.database.trabajadores;
});

app.MapGet("/trabajadores/{cedula}", (uint cedula) =>
{
	var result = "";
	var db = DataBaseSingleton.Instance.database.trabajadores;
	if (db != null)
	{
		foreach (var item in db)
		{
			if (item.cedula.Equals(cedula))
			{
				result = JsonSerializer.Serialize(item);
			}
		}
	}
	return result;
});

app.MapPost("/trabajadores", (Trabajador trabajador) =>
{
	if (DataBaseSingleton.Instance.database.trabajadores.Contains(trabajador))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.trabajadores.Add(trabajador);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/usuarios", () =>
{
	return DataBaseSingleton.Instance.database.usuarios;
});

app.MapGet("/usuarios/{cedula}", (uint cedula) =>
{
	var result = "";
	var db = DataBaseSingleton.Instance.database.usuarios;
	if (db != null)
	{
		foreach (var item in db)
		{
			if (item.cedula.Equals(cedula))
			{
				result = JsonSerializer.Serialize(item);
			}
		}
	}
	return result;
});

app.MapPost("/usuarios", (Usuario user) =>
{
	if (DataBaseSingleton.Instance.database.usuarios.Contains(user))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.usuarios.Add(user);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/maletas", () =>
{
	return DataBaseSingleton.Instance.database.maletas;
});

app.MapPost("/maletas", (MaletaQData data) =>
{
	uint numero = 0;
	if (DataBaseSingleton.Instance.database.maletas.Count() != 0)
	{
		numero = DataBaseSingleton.Instance.database.maletas.Last().numero + 1;
	}
	var maleta = new Maleta(numero, data.color, data.peso, data.costo_envio);
	if (DataBaseSingleton.Instance.database.maletas.Contains(maleta))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.maletas.Add(maleta);
		DataBaseSingleton.Instance.save_state();
		return $"{{\"numero\": {numero}, \"success\": 1}}";
	}
});

app.MapGet("/maletas/info/{numero}", (int numero) =>
{
	var result = "";
	var db = DataBaseSingleton.Instance.database.maletas;
	if (db != null)
	{
		foreach (var item in db)
		{
			if (item.numero.Equals(numero))
			{
				result = JsonSerializer.Serialize(item);
			}
		}
	}
	return result;
});

app.MapGet("/bagcarts", () =>
{
	return DataBaseSingleton.Instance.database.bagcarts;
});

app.MapPost("/bagcarts", (BagCartQData data) =>
{
	uint id = 0;
	if (DataBaseSingleton.Instance.database.bagcarts.Count() != 0)
	{
		id = DataBaseSingleton.Instance.database.bagcarts.Last().id + 1;
	}
	var bagcart = new BagCart(id, data.marca, data.modelo);
	if (DataBaseSingleton.Instance.database.bagcarts.Contains(bagcart))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.bagcarts.Add(bagcart);
		DataBaseSingleton.Instance.save_state();
		return $"{{\"id\": {id}, \"success\": 1}}";
	}
});

app.MapGet("/bagcarts/info/{id}", (int id) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.bagcarts)
	{
		if (item.id.Equals(id))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/tipo_avion", () =>
{
	return DataBaseSingleton.Instance.database.tipos_avion;
});

app.MapPost("/tipos_avion", (TipoAvion data) =>
{
	if (DataBaseSingleton.Instance.database.tipos_avion.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.tipos_avion.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/tipos_avion/{nombre}", (string nombre) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.tipos_avion)
	{
		if (item.nombre.Equals(nombre))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/vuelos", () =>
{
	return DataBaseSingleton.Instance.database.vuelos;
});

app.MapPost("/vuelos", (VueloQData data) =>
{
	uint id = 0;
	if (DataBaseSingleton.Instance.database.vuelos.Count() != 0)
	{
		id = DataBaseSingleton.Instance.database.vuelos.Last().id + 1;
	}
	var vuelo = new Vuelo(id, data.tipo_avion);
	if (DataBaseSingleton.Instance.database.vuelos.Contains(vuelo))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.vuelos.Add(vuelo);
		DataBaseSingleton.Instance.save_state();
		return $"{{\"id\": {id}, \"success\": 1}}";
	}
});

app.MapGet("/vuelos/info/{id}", (int id) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.vuelos)
	{
		if (item.id.Equals(id))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/usuario_maleta", () =>
{
	return DataBaseSingleton.Instance.database.rel_usuario_maleta;
});

app.MapPost("/rel/usuario_maleta", (RelUsuarioMaleta data) =>
{
	if (DataBaseSingleton.Instance.database.rel_usuario_maleta.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.rel_usuario_maleta.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/usuario_maleta/usuario/{cedula}", (uint cedula) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_usuario_maleta)
	{
		if (item.cedula_usuario.Equals(cedula))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/usuario_maleta/maleta/{numero}", (uint numero) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_usuario_maleta)
	{
		if (item.numero_maleta.Equals(numero))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/scan_rayosx_maleta", () =>
{
	return DataBaseSingleton.Instance.database.rel_scan_rayosx_maleta;
});

app.MapPost("/rel/scan_rayosx_maleta", (RelScanRayosXMaleta data) =>
{
	if (DataBaseSingleton.Instance.database.rel_scan_rayosx_maleta.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.rel_scan_rayosx_maleta.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/scan_rayosx/maleta/{numero}", (uint numero) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_scan_rayosx_maleta)
	{
		if (item.numero_maleta.Equals(numero))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/scan_rayosx/trabajador/{cedula}", (uint cedula) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_scan_rayosx_maleta)
	{
		if (item.cedula_trabajador.Equals(cedula))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/maleta_bagcart", () =>
{
	return DataBaseSingleton.Instance.database.rel_maleta_bagcart;
});

app.MapPost("/rel/maleta_bagcart", (RelMaletaBagCart data) =>
{
	var sin_rayosx = true;
	// Buscar que esté escaneado con rayos X antes de asignar a un bagcart
	foreach (var item in DataBaseSingleton.Instance.database.rel_scan_rayosx_maleta)
	{
		if (item.numero_maleta == data.numero_maleta)
		{
			sin_rayosx = false;
		}
	}
	if (DataBaseSingleton.Instance.database.rel_maleta_bagcart.Contains(data) || sin_rayosx)
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.rel_maleta_bagcart.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/maleta_bagcart/maleta/{numero}", (uint numero) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_maleta_bagcart)
	{
		if (item.numero_maleta.Equals(numero))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/maleta_bagcart/bagcart/{id}", (uint id) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_maleta_bagcart)
	{
		if (item.id_bagcart.Equals(id))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/scan_asignacion_maleta", () =>
{
	return DataBaseSingleton.Instance.database.rel_scan_asignacion_maleta;
});

app.MapPost("/rel/scan_asignacion_maleta", (RelScanAsignacionMaleta data) =>
{
	if (DataBaseSingleton.Instance.database.rel_scan_asignacion_maleta.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.rel_scan_asignacion_maleta.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/scan_asignacion_maleta/maleta/{numero}", (uint numero) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_scan_asignacion_maleta)
	{
		if (item.numero_maleta.Equals(numero))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/scan_asignacion_maleta/trabajador/{cedula}", (uint cedula) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_scan_asignacion_maleta)
	{
		if (item.cedula_trabajador.Equals(cedula))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/scan_asignacion_maleta/vuelo/{id}", (uint id) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_scan_asignacion_maleta)
	{
		if (item.id_vuelo.Equals(id))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/vuelo_bagcart", () =>
{
	return DataBaseSingleton.Instance.database.rel_vuelo_bagcart;
});

app.MapPost("/rel/vuelo_bagcart", (RelVueloBagCart data) =>
{
	if (DataBaseSingleton.Instance.database.rel_vuelo_bagcart.Contains(data))
	{
		return "{\"success\": 0}";
	}
	else
	{
		DataBaseSingleton.Instance.database.rel_vuelo_bagcart.Add(data);
		DataBaseSingleton.Instance.save_state();
		return "{\"success\": 1}";
	}
});

app.MapGet("/rel/vuelo_bagcart/bagcart/{id}", (uint id) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_vuelo_bagcart)
	{
		if (item.id_bagcart.Equals(id))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/vuelo_bagcart/vuelo/{id}", (uint id) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_vuelo_bagcart)
	{
		if (item.id_vuelo.Equals(id))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.MapGet("/rel/vuelo_bagcart/sello/{sello}", (string sello) =>
{
	var result = "";
	foreach (var item in DataBaseSingleton.Instance.database.rel_vuelo_bagcart)
	{
		if (item.sello.Equals(sello))
		{
			result = JsonSerializer.Serialize(item);
		}
	}
	return result;
});

app.Run();
