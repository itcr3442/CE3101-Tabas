package cr.ac.tec.ce3101.tabas.app

import android.content.Intent
import android.os.Bundle
import android.view.View
import android.widget.AdapterView.OnItemClickListener
import android.widget.ArrayAdapter
import android.widget.ListView
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentPagerAdapter
import androidx.viewpager.widget.ViewPager
import com.google.android.material.tabs.TabLayout

/* La actividad de overview es la vista principal de la aplicación.
 * Esta organiza las maletas disponibles según su siguiente operación,
 * la cual puede ser escaneo de rayos junto a aprobación/rechazo o
 * escaneo en abordaje junto a carga en avión.
 */
class OverviewActivity : AppCompatActivity() {
    // Estado contextual
    private lateinit var session: Session
    private lateinit var pager: ViewPager
    private lateinit var maletas: List<Maleta>

    /* Variables de enumeración. El proceso de enumeración consiste en
     * determinar, para cada maleta, si esta corresponde en escaneo,
     * abordaje, o ninguna.
     */
    private var escaneo: MutableList<Maleta> = ArrayList()
    private var abordaje: MutableList<Maleta> = ArrayList()
    private var ignored = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_overview)

        pager = findViewById(R.id.viewPager)
        session = (getApplication() as TabasApp).session!!
    }

    /* La lógica de enumeración se ejecuta cada vez que la actividad
     * entra en foreground con tal de garantizar refrescamientos luego
     * de modificar estado en una de las subactividades.
     */
    override fun onResume() {
        super.onResume()

        escaneo.clear()
        abordaje.clear()
        ignored = 0

        /* Por cada maleta conocida, se prueba si existe un registro de
         * escaneo de rayos. De NO ser así, se coloca en la pestaña de
         * escaneo. Si el registro existe, se prueba luego si NO existe
         * registro de abordaje, en cuyo caso se coloca en la pestaña de
         * abordaje. Si ambos registros existen, la maleta no se muestra.
         */
        session.maletas({ maletas ->
            run {
                this.maletas = maletas
                for (maleta in maletas) {
                    session.getScanRayos(
                        maleta,
                        { rel ->
                            run {
                                if (rel == null) {
                                    escaneo.add(maleta)
                                } else if (rel.aceptada) {
                                    session.getScanAbordaje(
                                        maleta,
                                        { rel ->
                                            run {
                                                if (rel == null) {
                                                    abordaje.add(maleta)
                                                } else {
                                                    ignored += 1
                                                }

                                                showIfReady()
                                            }
                                        }
                                    )
                                } else {
                                    ignored += 1
                                }

                                showIfReady()
                            }
                        }
                    )
                }
            }
        })
    }

    /* Cada vez que se determina el grupo en que corresponde una maleta
     * durante el proceso de enumeración, se verifica si la lista está
     * completa. De ser este el caso, se muestra.
     */
    private fun showIfReady() {
        if (escaneo.size + abordaje.size + ignored == maletas.size) {
            pager.adapter = PagerAdapter(supportFragmentManager)
            findViewById<TabLayout>(R.id.tabLayout).setupWithViewPager(pager)
        }
    }

    /* Este adaptador se encarga de enumerar y proveer comportamiento para
     * las dos pestañas (escaneo y abordaje).
     */
    private inner class PagerAdapter(fm: FragmentManager) : FragmentPagerAdapter(fm) { 
        override fun getCount(): Int = 2

        // Asocia títulos a partir de índices
        override fun getPageTitle(position: Int): CharSequence =
            when (position) {
                0 -> "Escaneo"
                else -> "Abordaje"
            }

        // Asocia índices a fragmentos de vistas
        override fun getItem(position: Int): Fragment =
            when (position) {
                // Pestaña de escaneo
                0 ->
                    ListFragment(
                        escaneo,
                        { maleta ->
                            run {
                                // Contextualiza para esta maleta particular
                                (getApplication() as TabasApp).maleta = maleta

                                // Cambia a la actividad de escaneo
                                startActivity(
                                    Intent(this@OverviewActivity, EscaneoActivity::class.java)
                                )
                            }
                        }
                    )

                // Caso contrario debe ser la pestaña de abordaje
                else ->
                    ListFragment(
                        abordaje,
                        { maleta ->
                            run {
                                // Contextualiza para esta maleta particular
                                (getApplication() as TabasApp).maleta = maleta

                                // Cambia a la actividad de abordaje
                                startActivity(
                                    Intent(this@OverviewActivity, AbordajeActivity::class.java)
                                )
                            }
                        }
                    )
            }
    }
}

/* Un fragmento es una porción no independiente de una view. Este fragmento
 * conforma una lista de maletas a mostrar en overview, sean para escaneo de
 * rayos o para abordaje. `onCLick` es la acción a ejecutar cuando el usuario
 * presiona una de las maletas.
 */
class ListFragment(private val maletas: List<Maleta>, private val onClick: (Maleta) -> Unit) :
    Fragment(R.layout.fragment_list) {
    private lateinit var listView: ListView

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        val items = maletas.map({ m -> "#${m.numero}" }).toTypedArray()

        listView = view.findViewById(R.id.list)
        listView.adapter = ArrayAdapter(view.context, R.layout.list_item, items)
        listView.onItemClickListener = OnItemClickListener { parent, view, position, id ->
            onClick(maletas[position])
        }
    }
}
