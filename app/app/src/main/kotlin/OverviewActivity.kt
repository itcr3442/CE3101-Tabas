package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity

class OverviewActivity : AppCompatActivity() {
  private lateinit var session: Session

  override fun onCreate(savedInstanceState: Bundle?) {
    super.onCreate(savedInstanceState)
    setContentView(R.layout.activity_overview)

    session = (getApplication() as TabasApp).session!!
  }
}
