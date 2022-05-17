package com.example.anticovid;

import androidx.appcompat.app.AppCompatActivity;
import androidx.viewpager.widget.ViewPager;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.google.android.material.tabs.TabLayout;

public class MainActivity extends AppCompatActivity {

    TabLayout tabLayout;
    ViewPager viewPager;
    SharedPreferences sp;
    float v = 0;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


            sp = this.getSharedPreferences("MyUserPrefs", Context.MODE_PRIVATE);
            String username = sp.getString("uName",null);
            //Log.d("UserName: ", username);

            if (username != null){
                finish();
                Intent intent = new Intent(this, HomeView.class);
                startActivity(intent);


            }
            else {
                Toast.makeText(this,"Please Login",Toast.LENGTH_LONG).show();
                tabLayout = findViewById(R.id.tab_layout);
                viewPager = findViewById(R.id.view_pager);

                tabLayout.addTab(tabLayout.newTab().setText("Login"));
                tabLayout.addTab(tabLayout.newTab().setText("Signup"));
                tabLayout.setTabGravity(TabLayout.GRAVITY_FILL);

                final LoginAdapter adapter = new LoginAdapter(getSupportFragmentManager(), this,tabLayout.getTabCount());
                viewPager.setAdapter(adapter);

                viewPager.addOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
                tabLayout.setTranslationY(300);
                tabLayout.setAlpha(v);
                tabLayout.animate().translationY(0).alpha(1).setDuration(1000).setStartDelay(100).start();


            }


        }


    }
