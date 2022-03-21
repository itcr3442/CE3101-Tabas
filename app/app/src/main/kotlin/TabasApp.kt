package cr.ac.tec.ce3101.tabas.app

import android.app.Application

/* Este es un singleton que contiene el estado contextual de la
 * aplicación: la sesión establecida con el servidor y posiblemente
 * una maleta en consideración bajo la actividad actualmente activa.
 */
class TabasApp : Application() {
    var session: Session? = null
    var maleta: Maleta? = null
}
