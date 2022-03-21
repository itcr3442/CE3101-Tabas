package cr.ac.tec.ce3101.tabas.app

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.EditText
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

class MainActivity : AppCompatActivity() {
    private lateinit var baseUrl: EditText
    private lateinit var username: EditText
    private lateinit var password: EditText
    private lateinit var loginError: TextView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        baseUrl = findViewById(R.id.baseUrl)
        username = findViewById(R.id.username)
        password = findViewById(R.id.password)
        loginError = findViewById(R.id.loginError)
    }

    fun login(view: View) {
        val baseUrl = baseUrl.text.toString()
        val username = username.text.toString()
        val password = password.text.toString()

        val session = Session(baseUrl, username, password, this)
        session.login({ success ->
            run {
                if (success) {
                    (getApplication() as TabasApp).session = session
                    startActivity(Intent(this, OverviewActivity::class.java))
                } else {
                    loginError.setVisibility(View.VISIBLE)
                }
            }
        })
    }
}
