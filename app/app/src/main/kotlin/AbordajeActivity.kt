package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity

/* La actividad de abordaje muestra al funcionario un botón que le permite
 * marcar una maleta dada como escaneada para abordaje y colocada en el avión.
 */
class AbordajeActivity : AppCompatActivity() {
    // Referencias contextuales obtenidas del singleton de aplicación
    private lateinit var session: Session
    private lateinit var maleta: Maleta

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_abordaje)

        val application = getApplication() as TabasApp
        session = application.session!!
        maleta = application.maleta!!
    }

    /* La lógica es simplemente un POST a la REST API para indicar
     * la operación. `finish()` termina la actividad y retorna a
     * la actividad anterior.
     */
    fun scan(view: View) {
        session.postScanAbordaje(maleta, { finish() })
    }
}
