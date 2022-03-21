package cr.ac.tec.ce3101.tabas.app

import android.app.AlertDialog
import android.content.Context
import android.content.DialogInterface
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

/* Los objetos de sesión contienen la información relacionada
 * a una sesión activa con el servidor. Los métodos públicos
 * ofrecidos por esta clase permiten realizar solicitudes y
 * aceptan un parámetro de callback, el cual es invocado al
 * obtener una respuesta del servidor.
 */
class Session(
    url: String,
    private val username: String,
    private val password: String,
    private val cx: Context
) {
    private val service: TabasService

    // Al construir el objeto de sesión se inicializa el cliente REST.
    init {
        val retrofit =
            Retrofit.Builder()
                .baseUrl(url)
                .addConverterFactory(GsonConverterFactory.create())
                .build()

        service = retrofit.create(TabasService::class.java)
    }

    // Intenta iniciar sesión con las credenciales asociadas.
    fun login(auth: (Boolean) -> Unit) {
        service
            .checkLogin(username, password)
            .enqueue(
                object : Cb<Success>() {
                    override fun onResponse(call: Call<Success>, response: Response<Success>) {
                        auth(response.body()!!.success == 1)
                    }
                }
            )
    }

    // Enumera todas las maletas.
    fun maletas(cb: (List<Maleta>) -> Unit) {
        service
            .maletas()
            .enqueue(
                object : Cb<List<Maleta>>() {
                    override fun onResponse(
                        call: Call<List<Maleta>>,
                        response: Response<List<Maleta>>
                    ) {
                        cb(response.body()!!)
                    }
                }
            )
    }

    // Enumera todos los bagcarts.
    fun bagcarts(cb: (List<Bagcart>) -> Unit) {
        service
            .bagcarts()
            .enqueue(
                object : Cb<List<Bagcart>>() {
                    override fun onResponse(
                        call: Call<List<Bagcart>>,
                        response: Response<List<Bagcart>>
                    ) {
                        cb(response.body()!!)
                    }
                }
            )
    }

    // Obtiene, de existir, el resultado de escaneo de rayos para una maleta.
    fun getScanRayos(maleta: Maleta, cb: (RelScanRayos?) -> Unit) {
        service
            .getScanRayos(maleta.numero)
            .enqueue(
                object : Cb<RelScanRayos?>() {
                    override fun onResponse(
                        call: Call<RelScanRayos?>,
                        response: Response<RelScanRayos?>
                    ) {
                        cb(response.body())
                    }
                }
            )
    }

    // Registra un escaneo de rayos junto a su comentario y estado de aceptación.
    fun postScanRayos(maleta: Maleta, accept: Boolean, comment: String, cb: () -> Unit) {
        val rel =
            RelScanRayos().apply {
                cedula_trabajador = cedula()
                numero_maleta = maleta.numero
                aceptada = accept
                comentarios = comment
            }

        service
            .postScanRayos(password, rel)
            .enqueue(
                object : Cb<Success>() {
                    override fun onResponse(call: Call<Success>, response: Response<Success>) = cb()
                }
            )
    }

    // Obtiene, de existir, la relación de escaneo de abordaje para la maleta dada.
    fun getScanAbordaje(maleta: Maleta, cb: (RelScanAbordaje?) -> Unit) {
        service
            .getScanAbordaje(maleta.numero)
            .enqueue(
                object : Cb<RelScanAbordaje?>() {
                    override fun onResponse(
                        call: Call<RelScanAbordaje?>,
                        response: Response<RelScanAbordaje?>
                    ) {
                        cb(response.body())
                    }
                }
            )
    }

    // Registra una maleta en avión (escaneo de abordaje). 
    fun postScanAbordaje(maleta: Maleta, cb: () -> Unit) {
        val rel =
            RelScanAbordaje().apply {
                cedula_trabajador = cedula()
                numero_maleta = maleta.numero
            }

        service
            .postScanAbordaje(password, rel)
            .enqueue(
                object : Cb<Success>() {
                    override fun onResponse(call: Call<Success>, response: Response<Success>) = cb()
                }
            )
    }

    // Asocia una maleta a un bagcart.
    fun postMaletaBagcart(maleta: Maleta, bagcart: Bagcart, cb: () -> Unit) {
        val rel =
            RelMaletaBagcart().apply {
                numero_maleta = maleta.numero
                id_bagcart = bagcart.id
            }

        service
            .postMaletaBagcart(cedula(), password, rel)
            .enqueue(
                object : Cb<Success>() {
                    override fun onResponse(call: Call<Success>, response: Response<Success>) = cb()
                }
            )
    }

    // Conveniencia par convertir username de cadena a entero
    fun cedula(): Int = username.toInt()

    /* Cb ("callback") se utiliza para evitar código repetido
     * en las invocaciones anteriores de endpoints, específicamente
     * la lógica de fallo de conexión o red. En este caso se muestra
     * un diálogo sencillo.
     */
    inner abstract class Cb<T> : Callback<T> {
        override fun onFailure(call: Call<T>, err: Throwable) {
            val builder = AlertDialog.Builder(cx)
            builder
                .setCancelable(false)
                .setMessage("Error de red")
                .setPositiveButton(
                    "Aceptar",
                    DialogInterface.OnClickListener { dialog, which -> Unit }
                )
                .create()

            builder.create().show()
        }
    }
}
