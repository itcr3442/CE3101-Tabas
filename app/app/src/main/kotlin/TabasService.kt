package cr.ac.tec.ce3101.tabas.app

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

interface TabasService {
  @GET("check_login")
  fun checkLogin(@Query("cedula") id: String, @Query("password_hash") password: String): Call<Success>
}

class Success {
  var success: Int = 0
}
