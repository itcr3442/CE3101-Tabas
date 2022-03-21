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

class OverviewActivity : AppCompatActivity() {
    private lateinit var session: Session
    private lateinit var pager: ViewPager
    private lateinit var maletas: List<Maleta>
    private var escaneo: MutableList<Maleta> = ArrayList()
    private var abordaje: MutableList<Maleta> = ArrayList()
    private var ignored = 0

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_overview)

        pager = findViewById(R.id.viewPager)
        session = (getApplication() as TabasApp).session!!
    }

    override fun onResume() {
        super.onResume()

        escaneo.clear()
        abordaje.clear()
        ignored = 0

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

    private fun showIfReady() {
        if (escaneo.size + abordaje.size + ignored == maletas.size) {
            pager.adapter = PagerAdapter(supportFragmentManager)
            findViewById<TabLayout>(R.id.tabLayout).setupWithViewPager(pager)
        }
    }

    private inner class PagerAdapter(fm: FragmentManager) : FragmentPagerAdapter(fm) {
        override fun getCount(): Int = 2

        override fun getPageTitle(position: Int): CharSequence =
            when (position) {
                0 -> "Escaneo"
                else -> "Abordaje"
            }

        override fun getItem(position: Int): Fragment =
            when (position) {
                0 ->
                    ListFragment(
                        escaneo,
                        { maleta ->
                            run {
                                (getApplication() as TabasApp).maleta = maleta
                                startActivity(
                                    Intent(this@OverviewActivity, EscaneoActivity::class.java)
                                )
                            }
                        }
                    )
                else ->
                    ListFragment(
                        abordaje,
                        { maleta ->
                            run {
                                (getApplication() as TabasApp).maleta = maleta
                                startActivity(
                                    Intent(this@OverviewActivity, AbordajeActivity::class.java)
                                )
                            }
                        }
                    )
            }
    }
}

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
