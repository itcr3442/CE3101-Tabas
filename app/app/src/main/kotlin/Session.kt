package cr.ac.tec.ce3101.tabas.app

import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class Session(url: String, private val username: String, private val password: String) {
  private val service: TabasService

  init {
    val retrofit = Retrofit.Builder()
                    .baseUrl(url)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build()

    service = retrofit.create(TabasService::class.java)
  }

  fun login(auth: (Boolean) -> Unit) {
    service.checkLogin(username, password).enqueue(object : Callback<Success> {
      override fun onResponse(call: Call<Success>, response: Response<Success>) {
        auth(response.body()!!.success == 1)
      }

      override fun onFailure(call: Call<Success>, t: Throwable) {
        throw t
      }
    });
  }
}
