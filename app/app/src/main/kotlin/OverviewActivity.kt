package cr.ac.tec.ce3101.tabas.app

import android.os.Bundle
import android.view.View
import android.widget.ListView
import android.widget.ArrayAdapter
import android.widget.AdapterView.OnItemClickListener
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.fragment.app.FragmentPagerAdapter
import androidx.viewpager.widget.ViewPager
import androidx.appcompat.app.AppCompatActivity

import com.google.android.material.tabs.TabLayout

class OverviewActivity : AppCompatActivity() {
  private lateinit var session: Session
  private lateinit var pager: ViewPager

  override fun onCreate(savedInstanceState: Bundle?) {
    super.onCreate(savedInstanceState)
    setContentView(R.layout.activity_overview)

    session = (getApplication() as TabasApp).session!!
    pager = findViewById(R.id.viewPager)
    pager.adapter = PagerAdapter(supportFragmentManager)

    findViewById<TabLayout>(R.id.tabLayout).setupWithViewPager(pager)
  }

  inner class PagerAdapter(fm: FragmentManager): FragmentPagerAdapter(fm) {
    override fun getCount(): Int = 2
  
    override fun getPageTitle(position: Int): CharSequence = when(position) {
      0 -> "Maletas"
      else -> "BagCarts"
    }
  
    override fun getItem(position: Int): Fragment = when(position) {
      0 -> ListFragment(arrayOf("a", "b"), { id -> run {} })
      else -> ListFragment(arrayOf("d", "e", "f"), { id -> run {} })
    }
  }
}

class ListFragment(private val items: Array<String>, private val onClick: (Int) -> Unit) : Fragment(R.layout.fragment_list) {
  private lateinit var listView: ListView

  override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
    listView = view.findViewById(R.id.list)
    listView.adapter = ArrayAdapter(view.context, R.layout.list_item, items)
    listView.onItemClickListener = OnItemClickListener { parent, view, position, id -> onClick(position) }
  }
}
