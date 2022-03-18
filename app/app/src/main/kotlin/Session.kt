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
    service.checkLogin(username, password).enqueue(object : SimpleCallback<Success> {
      override fun onResponse(call: Call<Success>, response: Response<Success>) {
        auth(response.body()!!.success == 1)
      }
    })
  }

  fun maletas(cb: (List<Maleta>) -> Unit) {
    service.maletas().enqueue(object : SimpleCallback<List<Maleta>> {
      override fun onResponse(call: Call<List<Maleta>>, response: Response<List<Maleta>>) {
        cb(response.body()!!)
      }
    })
  }

  fun relScanRayos(maleta: Maleta, cb: (RelScanRayos?) -> Unit) {
    service.relScanRayos(maleta.numero).enqueue(object : SimpleCallback<RelScanRayos?> {
      override fun onResponse(call: Call<RelScanRayos?>, response: Response<RelScanRayos?>) {
        cb(response.body())
      }
    })
  }

  fun relScanAbordaje(maleta: Maleta, cb: (RelScanAbordaje?) -> Unit) {
    service.relScanAbordaje(maleta.numero).enqueue(object : SimpleCallback<RelScanAbordaje?> {
      override fun onResponse(call: Call<RelScanAbordaje?>, response: Response<RelScanAbordaje?>) {
        cb(response.body())
      }
    })
  }
}

interface SimpleCallback<T> : Callback<T> {
  override fun onFailure(call: Call<T>, err: Throwable) {
    throw err
  }
}
