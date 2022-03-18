using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using server;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
									builder =>
									{
										builder.WithOrigins("http://localhost:4200", "http://127.0.0.1:4200")
									.AllowAnyHeader()
									.WithMethods("POST", "GET");
									});
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

DataBaseState db()
{
    return DataBaseSingleton.Instance.database;
}

db();

//app.UseHttpsRedirection();

/// <summary>
/// GET que permite comprobar los credenciales de un trabajador.
/// </summary>
app.MapGet("/check_login", (uint cedula, string password_hash) =>
{
    if (!db().trabajadores.Exists(x =>
            x.cedula == cedula &&
            x.password_hash == password_hash
        ))
    {
        return "{\"success\": 0}";
    }
    else
    {
        return "{\"success\": 1}";
    }
});
/// <summary>
/// GET que permite confirmar los credenciales de un usario.
/// </summary>
app.MapGet("/check_login_user", (uint cedula, string password_hash) =>
{
    if (!db().usuarios.Exists(x =>
            x.cedula == cedula &&
            x.password_hash == password_hash
        ))
    {
        return "{\"success\": 0}";
    }
    else
    {
        return "{\"success\": 1}";
    }
});
/// <summary>
/// GET que obtiene la lista de trabajadores con contraseñas ofuscadas
/// </summary>
app.MapGet("/trabajadores", (uint cedula_admin, string password_hash) =>
{
    if (!db().trabajadores.Exists(
        x => x.cedula == cedula_admin &&
            x.password_hash == password_hash &&
            x.rol == "admin"
    ))
    {
        return null;
    }
    else
    {
        List<Trabajador> x = new List<Trabajador>();
        foreach (var item in db().trabajadores)
        {
            x.Add(new Trabajador
            {
                cedula = item.cedula,
                password_hash = "*************",
                nombre = item.nombre,
                primer_apellido = item.primer_apellido,
                segundo_apellido = item.segundo_apellido,
                rol = item.rol

            });
        }
        return x;
    }
});

/// <summary>
///	GET que permite obtener los datos de un trabajador en particular 
/// </summary>
app.MapGet("/trabajadores/{cedula}", (uint cedula, uint cedula_admin, string password_hash) =>
{
    if (!db().trabajadores.Exists(x =>
            x.cedula == cedula_admin &&
            x.password_hash == password_hash &&
            x.rol == "admin"
        ))
    {
        return null;
    }
    else
    {
        return JsonSerializer.Serialize(db().trabajadores.Find((x) => x.cedula.Equals(cedula)));
    }
});

/// <summary>
/// POST para la creación de trabajadores en la base de datos
/// </summary>
app.MapPost("/trabajadores", (uint cedula, string password_hash, Trabajador trabajador) =>
{
    // diferentes causas de fallo resultan en distintos código de error
    // retornados al consumidor del servicio
    if (db().trabajadores.Exists(x => x.cedula == trabajador.cedula))
    {
        return "{\"success\": -1}";
    }
    else if (!db().roles.Exists(x => x.nombre == trabajador.rol))
    {
        return "{\"success\": -2}";

    }
    else if (!db().trabajadores.Exists(x =>
       x.cedula == cedula &&
       x.password_hash == password_hash &&
       x.rol == "admin"
  ))
    {
        return "{\"success\": -3}";

    }
    else
    {
        db().trabajadores.Add(trabajador);
        DataBaseSingleton.Instance.save_state();
        return "{\"success\": 1}";
    }
});
/// <summary>
/// GET que obtiene la lista de usuarios del servicio
/// </summary>
app.MapGet("/usuarios", () =>
{
    List<Usuario> x = new List<Usuario>();
    foreach (var item in db().usuarios)
    {
        x.Add(item);
    }
    foreach (var item in x)
    {
        item.password_hash = "*********";
    }
    return x;
});

/// <summary>
/// GET que permite obtener la información de un usuario en específico
/// </summary>
app.MapGet("/usuarios/{cedula}", (uint cedula) =>
{
    return JsonSerializer.Serialize(db().usuarios.Find((x) => x.cedula.Equals(cedula)));
});

app.MapPost("/usuarios", (uint cedula, string password_hash, Usuario user) =>
{
    if (db().usuarios.Contains(user) ||
        !db().trabajadores.Exists(x =>
            x.cedula == cedula &&
            x.password_hash == password_hash &&
            (x.rol == "admin" || x.rol == "recepcionista")
        ))
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

/// <summary>
///	GET que permite obtener la lista de todas las maletas registradas 
/// </summary>
app.MapGet("/maletas", () =>
{
    return db().maletas;
});

app.MapPost("/maletas", (uint cedula, string password_hash, MaletaQData data) =>
{

    uint numero = 0;

    if (db().maletas.Count() != 0)
    {
        numero = db().maletas.Last().numero + 1;
    }

    var maleta = new Maleta
    {
        nvuelo = data.nvuelo,
        numero = numero,
        cedula_usuario = data.cedula_usuario,
        color = data.color,
        peso = data.peso,
        costo_envio = data.costo_envio
    };

    if (db().maletas.Contains(maleta) ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
    {
        return "{\"success\": -1}";
    }
    else if (!db().usuarios.Exists(x => x.cedula == data.cedula_usuario))
    {
        return "{\"success\": -2}";
    }
    else
    {
        db().maletas.Add(maleta);
        DataBaseSingleton.Instance.save_state();
        return $"{{\"numero\": {numero}, \"success\": 1}}";
    }
});
/// <summary>
/// GET para conseguir información de una maleta de un número específico
/// </summary>
app.MapGet("/maletas/info/{numero}", (uint numero) =>
{
    return JsonSerializer.Serialize(db().maletas.Find((maleta) => maleta.numero.Equals(numero)));
});

/// <summary>
/// GET para conseguir todas las maletas  de un usuario
/// </summary>
app.MapGet("/maletas/usuario/{cedula}", (uint cedula) =>
{
    return JsonSerializer.Serialize(db().maletas.Find((maleta) => maleta.cedula_usuario.Equals(cedula)));
});

/// <summary>
/// GET para obtener la lista de todos los bagcarts registrados
/// </summary>
app.MapGet("/bagcarts", () =>
{
    return db().bagcarts;
});

/// <summary>
/// POST para la creación de un bagcart en la base de datos
/// </summary>
app.MapPost("/bagcarts", (uint cedula, string password_hash, BagCartQData data) =>
{
    uint id = 0;

    if (db().bagcarts.Count() != 0)
    {
        id = db().bagcarts.Last().id + 1;
    }
    var bagcart = new BagCart(id, data.marca, data.modelo);
    if (db().bagcarts.Contains(bagcart) ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
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
/// <summary>
/// GET para obtener información de un bagcart en específico a partir del id
/// </summary>
app.MapGet("/bagcarts/info/{id}", (uint id) =>
{
    return JsonSerializer.Serialize(db().bagcarts.Find((x) => x.id.Equals(id)));
});

/// <summary>
/// GET para obtener la lista de todos los tipos de avion
/// </summary>
app.MapGet("/tipo_avion", () =>
{
    return db().tipos_avion;
});

/// <summary>
/// POST para la agregar un tipo de avion a la base de datos
/// </summary>
app.MapPost("/tipos_avion", (uint cedula, string password_hash, TipoAvion data) =>
{

    if (db().tipos_avion.Contains(data) ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
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

/// <summary>
/// GET para obtener la información de un tipo de avion en específico
/// </summary>
app.MapGet("/tipos_avion/{nombre}", (string nombre) =>
{
    return JsonSerializer.Serialize(db().tipos_avion.Find((x) => x.nombre == nombre));
});

/// <summary>
/// GET para obtener la lista de todos los vuelos registrados
/// </summary>
app.MapGet("/vuelos", () =>
{
    return db().vuelos;
});

/// <summary>
/// POST para el registro de un nuevo vuelo en la base de datos
/// </summary>
app.MapPost("/vuelos", (uint cedula, string password_hash, VueloQData data) =>
{
    uint numero = 0;

    if (db().vuelos.Count() != 0)
    {
        numero = db().vuelos.Last().numero + 1;
    }
    var vuelo = new Vuelo { numero = numero, avion = data.avion };
    if (db().vuelos.Contains(vuelo) || !db().aviones.Exists((x) => x.nserie.Equals(data.avion)) ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
    {
        return "{\"success\": 0}";
    }
    else
    {
        db().vuelos.Add(vuelo);
        DataBaseSingleton.Instance.save_state();
        return $"{{\"numero\": {numero}, \"success\": 1}}";
    }
});

/// <summary>
/// GET para obtener la información de un vuelo en específico en base a su número
/// </summary>
app.MapGet("/vuelos/info/{numero}", (int numero) =>
{
    return JsonSerializer.Serialize(db().vuelos.Find((x) => x.numero.Equals(numero)));
});

/// <summary>
/// GET para obtener la lista de todos los registros de maletas escaneadas con rayos x
/// </summary>
app.MapGet("/rel/scan_rayosx_maleta", () =>
{
    return db().rel_scan_rayosx_maleta;
});

/// <summary>
/// POST para registrar un resultado de un escaneo con rayos x de una maleta
/// </summary>
app.MapPost("/rel/scan_rayosx_maleta", (string password_hash, RelScanRayosXMaleta data) =>
{

    if (db().rel_scan_rayosx_maleta.Contains(data) ||
        !db().trabajadores.Exists(x => x.cedula.Equals(data.cedula_trabajador) &&
                x.password_hash == password_hash
        ))
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

/// <summary>
/// GET para obtener un registro de scan de de rayos x de una maleta en base a su numero
/// </summary>
app.MapGet("/rel/scan_rayosx_maleta/maleta/{numero}", (uint numero) =>
{
    return JsonSerializer.Serialize(db().rel_scan_rayosx_maleta.Find((x) => x.numero_maleta.Equals(numero)));
});

/// <summary>
/// GET para obtener la lista de todas las maletas escaneadas en rayos x por un trabajador
/// </summary>
app.MapGet("/rel/scan_rayosx_maleta/trabajador/{cedula}", (uint cedula) =>
{
    return JsonSerializer.Serialize(db().rel_scan_rayosx_maleta.Find((x) => x.cedula_trabajador.Equals(cedula)));
});

/// <summary>
/// GET que Obtiene la lista de todos los registros que asocian una maleta a un bagcart
/// </summary>
app.MapGet("/rel/maleta_bagcart", () =>
{
    return db().rel_maleta_bagcart;
});

/// <summary>
/// POST para crear un nuevo registro de asociación entre una maleta y un bagcart
/// </summary>
app.MapPost("/rel/maleta_bagcart", (uint cedula, string password_hash, RelMaletaBagCart data) =>
{

    if (db().rel_maleta_bagcart.Contains(data) ||
        !db().rel_scan_rayosx_maleta.Exists((x) => x.numero_maleta.Equals(data.numero_maleta) &&
            x.aceptada == true) ||
        !db().rel_vuelo_bagcart.Exists(x => x.id_bagcart.Equals(data.id_bagcart)) ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
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

/// <summary>
/// GET para obtener la información de asociación de una maleta a un bagcart en base
/// al numero de la maleta
/// </summary>
app.MapGet("/rel/maleta_bagcart/maleta/{numero}", (uint numero) =>
{
    return JsonSerializer.Serialize(db().rel_maleta_bagcart.Find((x) => x.numero_maleta.Equals(numero)));
});

/// <summary>
/// GET para obtener la información de asociación de varias maletas a un bagcart
/// en base al id del bagcart 
/// </summary>
app.MapGet("/rel/maleta_bagcart/bagcart/{id}", (uint id) =>
{
    return JsonSerializer.Serialize(db().rel_maleta_bagcart.Find((x) => x.id_bagcart.Equals(id)));
});

/// <summary>
/// GET que obtiene la lista de todas las maletas que han sido escaneadas y abordadas en un vuelo
/// </summary>
app.MapGet("/rel/scan_asignacion_maleta", () =>
{
    return db().rel_scan_asignacion_maleta;
});

/// <summary>
/// POST que permite crear el registro de que se ha escaneado y abordado una maleta en un vuelo
/// </summary>
app.MapPost("/rel/scan_asignacion_maleta", (string password_hash, RelScanAsignacionMaleta data) =>
{

    if (db().rel_scan_asignacion_maleta.Contains(data) ||
        !db().rel_maleta_bagcart.Exists((reg) => reg.numero_maleta.Equals(data.numero_maleta)) ||
        !db().trabajadores.Exists(x => x.cedula.Equals(data.cedula_trabajador) &&
                x.password_hash == password_hash
        ))
    {
        return "{\"success\": 0}";
    }
    else
    {
        //remover maleta del bagcart al pasarla al avion
        //db().rel_maleta_bagcart.RemoveAll((reg) => reg.numero_maleta.Equals(data.numero_maleta));
        db().rel_scan_asignacion_maleta.Add(data);
        DataBaseSingleton.Instance.save_state();
        return "{\"success\": 1}";
    }
});

/// <summary>
/// GET que obtiene la información de abordaje de una maleta en base a su número,
/// asumiendo que la maleta ya ha sido abordada
/// </summary>
app.MapGet("/rel/scan_asignacion_maleta/maleta/{numero}", (uint numero) =>
{
    return JsonSerializer.Serialize(db().rel_scan_asignacion_maleta.Find((x) => x.numero_maleta.Equals(numero)));
});

/// <summary>
/// GET que permite obtener todas las maletas que ha escaneado y abordado un trabajador
/// </summary>
app.MapGet("/rel/scan_asignacion_maleta/trabajador/{cedula}", (uint cedula) =>
{
    return JsonSerializer.Serialize(db().rel_scan_asignacion_maleta.Find((x) => x.cedula_trabajador.Equals(cedula)));
});

/// <summary>
/// GET que permite obtener la lista de todas las maletas que han sido embarcadas en un vuelo
/// </summary>
app.MapGet("/rel/scan_asignacion_maleta/vuelo/{numero}", (uint numero) =>
{
    var maletas = db().maletas.FindAll(x => x.numero == numero);
    return JsonSerializer.Serialize(db().rel_scan_asignacion_maleta.FindAll(
        (rel) => maletas.Exists(
            (maleta) => maleta.numero == rel.numero_maleta
    )));
});

/// <summary>
/// GET que obtiene la lista de todos los registros de asociación entre un bagcart y un vuelo
/// </summary>
/// <value></value>
app.MapGet("/rel/vuelo_bagcart", () =>
{
    return db().rel_vuelo_bagcart;
});

/// <summary>
/// POST que permite crear un registro de asociación entre un bagcart y un vuelo
/// </summary>
app.MapPost("/rel/vuelo_bagcart", (uint cedula, string password_hash, QRelVueloBagCart data) =>
{
    var reg = new RelVueloBagCart
    {
        id_bagcart = data.id_bagcart,
        id_vuelo = data.id_vuelo,
        sello = ""
    };
    if (db().rel_vuelo_bagcart.Contains(reg) ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
    {
        return "{\"success\": 0}";
    }
    else
    {
        db().rel_vuelo_bagcart.Add(reg);
        DataBaseSingleton.Instance.save_state();
        return "{\"success\": 1}";
    }
});

/// <summary>
/// POST que permite comunicar que se debe cerrar un bagcart 
/// </summary>
app.MapPost("/rel/vuelo_bagcart/cierre/bagcart/{id}", (uint id, uint cedula, string password_hash, string sello) =>
{
    var x = db().rel_vuelo_bagcart.Find(x => x.id_bagcart == id);
    if (x == null ||
        !db().trabajadores.Exists(
            x => x.cedula == cedula &&
                x.password_hash == password_hash
        ))
    {
        return "{\"success\": 0}";
    }
    else
    {
        x.sello = sello;
        DataBaseSingleton.Instance.save_state();
        return "{\"success\": 1}";
    }
});

/// <summary>
/// GET que permite obtener la relación de un bagcart a un vuelo en base
/// al id del bagcart 
/// </summary>
app.MapGet("/rel/vuelo_bagcart/bagcart/{id}", (uint id) =>
{
    return JsonSerializer.Serialize(db().rel_vuelo_bagcart.Find((x) => x.id_bagcart.Equals(id)));
});

/// <summary>
/// GET que permite obtener la lista de todos los bagcarts asociados a un vuelo
/// </summary>
app.MapGet("/rel/vuelo_bagcart/vuelo/{id}", (uint id) =>
{
    return JsonSerializer.Serialize(db().rel_vuelo_bagcart.FindAll((x) => x.id_vuelo.Equals(id)));

});

/// <summary>
/// GET Permite obtener un registro de asociación de un bagcart a un vuelo en base al sello del bagcart
/// </summary>
app.MapGet("/rel/vuelo_bagcart/sello/{sello}", (string sello) =>
{
    return JsonSerializer.Serialize(db().rel_vuelo_bagcart.Find((x) => x.sello.Equals(sello))); ;
});

/// <summary>
/// GET que obtiene los datos necesarios para el reporte de maletas por cliente
/// </summary>
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

/// <summary>
/// GET que obtiene los datos necesarios para el reporte de conciliación de maletas
/// de un vuelo en específico 
/// </summary>
app.MapGet("/reportes/conciliacion_maletas/{nvuelo}", (uint nvuelo) =>
{
    var result = "";
    Vuelo? vuelo = db().vuelos.Find((vuelo) => vuelo.numero.Equals(nvuelo));
    Avion? avion = db().aviones.Find(x => x.nserie == vuelo?.avion);
    TipoAvion? tipo_avion = db().tipos_avion.Find((tipo) => tipo.nombre == avion?.modelo);

    if (vuelo != null && tipo_avion != null && tipo_avion.nombre != null)
    {
        var maletas = db().maletas.FindAll(x => x.nvuelo == vuelo.numero);
        var maletas_en_avion = db().rel_scan_asignacion_maleta.FindAll(
            x => maletas.Exists(maleta => maleta.numero == x.numero_maleta
        ));
        var maletas_no_en_avion = db().rel_scan_asignacion_maleta.FindAll(
            x => !maletas.Exists(maleta => maleta.numero == x.numero_maleta
        ));
        var maletas_en_bagcarts = db().rel_maleta_bagcart.FindAll(
            x => maletas_no_en_avion.Exists(maleta => maleta.numero_maleta == x.numero_maleta));

        var maletas_rechazadas = db().rel_maleta_bagcart.FindAll(
            x => !maletas_no_en_avion.Exists(maleta => maleta.numero_maleta == x.numero_maleta));
        return JsonSerializer.Serialize(
            new ConciliacionMaletas(vuelo.numero, tipo_avion.nombre, tipo_avion.capacidad,
                maletas.Count(), maletas_en_bagcarts.Count(), maletas_en_avion.Count(), maletas_rechazadas.Count())
        );
    }
    return result;
});

app.Run();
