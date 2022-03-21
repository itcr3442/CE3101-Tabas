package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.LinearLayout
import androidx.appcompat.app.AppCompatActivity

class EscaneoActivity : AppCompatActivity() {
    private lateinit var session: Session
    private lateinit var maleta: Maleta
    private lateinit var comment: EditText

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_escaneo)

        comment = findViewById(R.id.comment)

        val application = getApplication() as TabasApp
        session = application.session!!
        maleta = application.maleta!!

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

    fun accept(bagcart: Bagcart) {
        session.postMaletaBagcart(maleta, bagcart, { postScan(true) })
    }

    fun reject(view: View) {
        postScan(false)
    }

    fun postScan(accept: Boolean) {
        session.postScanRayos(maleta, accept, comment.text.toString(), { finish() })
    }
}
