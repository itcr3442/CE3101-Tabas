package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.LinearLayout
import androidx.appcompat.app.AppCompatActivity

/* La actividad de escaneo permite aceptar una maleta en un bagcart, así
 * como rechazarla. Ambas operaciones involucran un comentario opcional.
 */
class EscaneoActivity : AppCompatActivity() {
    private lateinit var session: Session
    private lateinit var maleta: Maleta
    private lateinit var comment: EditText

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_escaneo)

        comment = findViewById(R.id.comment)

        // Estado contextual
        val application = getApplication() as TabasApp
        session = application.session!!
        maleta = application.maleta!!

        /* Por cada bagcart, se coloca un botón individual para aceptar
         * la maleta en este bagcart. El botón de rechazo está definido
         * estáticamente en el XML de layout.
         */
        session.bagcarts({ bagcarts ->
            run {
                val layout = findViewById<LinearLayout>(R.id.acceptView)

                for (bagcart in bagcarts) {
                    layout.addView(
                        Button(this).apply {
                            text =
                                "Aceptar en bagcart #${bagcart.id} (${bagcart.marca}, ${bagcart.modelo})"

                            setOnClickListener { view -> accept(bagcart) }
                        }
                    )
                }
            }
        })
    }

    /* Para aceptar la maleta, se asocia la misma al bagcart y luego
     * se marca el escaneo de rayos como aceptado junto al comentario.
     */
    fun accept(bagcart: Bagcart) {
        session.postMaletaBagcart(maleta, bagcart, { postScan(true) })
    }

    /* Si se rechaza, se registra esto para propósitos de escaneo de rayos.
     * No se crea ninguna asociación con ningún bagcart.
     */
    fun reject(view: View) {
        postScan(false)
    }

    // Lógica común a tanto aceptar como rechazar.
    fun postScan(accept: Boolean) {
        session.postScanRayos(maleta, accept, comment.text.toString(), { finish() })
    }
}
