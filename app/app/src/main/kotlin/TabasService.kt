package cr.ac.tec.ce3101.tabas.app

import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path
import retrofit2.http.Query

interface TabasService {
    @GET("check_login")
    fun checkLogin(
        @Query("cedula") id: String,
        @Query("password_hash") password: String
    ): Call<Success>

    @GET("maletas") fun maletas(): Call<List<Maleta>>

    @GET("bagcarts") fun bagcarts(): Call<List<Bagcart>>

    @GET("rel/scan_rayosx_maleta/maleta/{numero}")
    fun getScanRayos(@Path("numero") numero: Int): Call<RelScanRayos?>

    @POST("rel/scan_rayosx_maleta")
    fun postScanRayos(
        @Query("password_hash") password: String,
        @Body scan: RelScanRayos
    ): Call<Success>

    @GET("rel/scan_asignacion_maleta/maleta/{numero}")
    fun getScanAbordaje(@Path("numero") numero: Int): Call<RelScanAbordaje?>

    @POST("rel/scan_asignacion_maleta")
    fun postScanAbordaje(
        @Query("password_hash") password: String,
        @Body scan: RelScanAbordaje
    ): Call<Success>

    @POST("rel/maleta_bagcart")
    fun postMaletaBagcart(
        @Query("cedula") id: Int,
        @Query("password_hash") password: String,
        @Body rel: RelMaletaBagcart
    ): Call<Success>
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

class Bagcart {
    var id: Int = 0
    var marca: String = ""
    var modelo: Int = 0
}

class RelMaletaBagcart {
    var numero_maleta: Int = 0
    var id_bagcart: Int = 0
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
