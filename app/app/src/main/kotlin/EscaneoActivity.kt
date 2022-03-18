package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import android.widget.EditText
import androidx.appcompat.app.AppCompatActivity

class EscaneoActivity : AppCompatActivity() {
  private lateinit var session: Session
  private lateinit var maleta: Maleta
  private lateinit var comment: EditText

  override fun onCreate(savedInstanceState: Bundle?) {
    super.onCreate(savedInstanceState)
    setContentView(R.layout.activity_escaneo)

    comment = findViewById(R.id.comment);

    val application = getApplication() as TabasApp
	session = application.session!!
	maleta = application.maleta!!
  }

  fun accept(view: View) {
  }

  fun reject(view: View) {
  }
}

