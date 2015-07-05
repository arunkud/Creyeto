package com.heygo.arunkumar.activity;

import android.app.ActionBar;
import android.app.FragmentTransaction;
import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.FragmentActivity;
import android.util.Log;
import android.support.v4.view.ViewPager;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;

import com.example.arunkumar.test10.R;
import com.heygo.arunkumar.activity.ViewHandles.ViewPageSelector;


public class ChatHomeActivity extends FragmentActivity implements ActionBar.TabListener {

    ViewPager pager;
    ActionBar actionBar;
    ViewPageSelector selectr;
    Menu menu_entry;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.chat_home);
        pager = (ViewPager)findViewById(R.id.pager);

        pager.setOnPageChangeListener(new ViewPager.SimpleOnPageChangeListener() {
            @Override
            public void onPageSelected(int position) {
                actionBar.setSelectedNavigationItem(position);
            }
        });

        selectr =  new ViewPageSelector(getSupportFragmentManager());
        actionBar = getActionBar();

        pager.setAdapter(selectr);
        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_TABS);
        ActionBar.Tab tab1 = actionBar.newTab().setText(R.string.tabOne_title);
        tab1.setTabListener(this);

        ActionBar.Tab tab2 = actionBar.newTab().setText(R.string.tabTwo_title);
        tab2.setTabListener(this);

        ActionBar.Tab tab3 = actionBar.newTab().setText(R.string.tabThree_title);
        tab3.setTabListener(this);

        actionBar.addTab(tab1);
        actionBar.addTab(tab2);
        actionBar.addTab(tab3);

    }


    @Override
    public void onTabSelected(ActionBar.Tab tab, FragmentTransaction ft) {
        Log.d("ARUN", "onTabSelected");
        pager.setCurrentItem(tab.getPosition());

    }

    @Override
    public void onTabUnselected(ActionBar.Tab tab, FragmentTransaction ft) {
        Log.d("ARUN", "onTabSelected");
    }

    @Override
    public void onTabReselected(ActionBar.Tab tab, FragmentTransaction ft) {
        Log.d("ARUN", "onTabSelected");
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle item selection
        switch (item.getItemId()) {
            case R.id.action_addContact:
                openContactActivity();
                return true;
            case R.id.action_about:
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    private void openContactActivity() {
        Intent intent = new Intent(this, FindContacts.class);
        startActivity(intent);
    }
}
