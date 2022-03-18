package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import android.widget.EditText
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity

import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class MainActivity : AppCompatActivity() {
  private lateinit var username: EditText
  private lateinit var password: EditText
  private lateinit var loginError: TextView

  override fun onCreate(savedInstanceState: Bundle?) {
    super.onCreate(savedInstanceState)
    setContentView(R.layout.activity_main)

    username = findViewById(R.id.username);
    password = findViewById(R.id.password);
    loginError = findViewById(R.id.loginError);
  }

  fun login(view: View) {
    val username = username.text.toString()
	val password = password.text.toString()

    val retrofit = Retrofit.Builder()  
                    .baseUrl("http://192.168.34.68:5265")  
                    .addConverterFactory(GsonConverterFactory.create())  
                    .build()  

    val service = retrofit.create(TabasService::class.java)
	service.checkLogin(username, password).enqueue(object : Callback<Success> {
	  override fun onResponse(call: Call<Success>, response: Response<Success>) {
	    if(response.body()!!.success != 1) {
		  loginError.setVisibility(View.VISIBLE)
		}
	  }

	  override fun onFailure(call: Call<Success>, t: Throwable) {
	    throw t
	  }
	});
  }
}
