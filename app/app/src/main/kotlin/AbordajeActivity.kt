package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity

class AbordajeActivity : AppCompatActivity() {
    private lateinit var session: Session
    private lateinit var maleta: Maleta

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_abordaje)

        val application = getApplication() as TabasApp
        session = application.session!!
        maleta = application.maleta!!
    }

    fun scan(view: View) {
        session.postScanAbordaje(maleta, { finish() })
    }
}
