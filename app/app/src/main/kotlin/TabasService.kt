package cr.ac.tec.ce3101.tabas.app

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;
import retrofit2.http.Query;

interface TabasService {
  @GET("check_login")
  fun checkLogin(@Query("cedula") id: String, @Query("password_hash") password: String): Call<Success>

  @GET("maletas")
  fun maletas(): Call<List<Maleta>>

  @GET("rel/scan_rayosx_maleta/maleta/{numero}")
  fun relScanRayos(@Path("numero") numero: Int): Call<RelScanRayos?>

  @GET("rel/scan_asignacion_maleta/maleta/{numero}")
  fun relScanAbordaje(@Path("numero") numero: Int): Call<RelScanAbordaje?>
}

class Success {
  var success: Int = 0
}

class Maleta {
  var numero: Int = 0
  var cedula_usuario: Int = 0
  var color: Int = 0
  var peso: Float = 0.0f
  var costo_envio: Float = 0.0f
  var nvuelo: Int = 0
}

class RelScanRayos {
  var cedula_trabajador: Int = 0
  var numero_maleta: Int = 0
  var aceptada: Boolean = false
  var comentarios: String? = ""
}

class RelScanAbordaje {
  var cedula_trabajador: Int = 0
  var numero_maleta: Int = 0
}
