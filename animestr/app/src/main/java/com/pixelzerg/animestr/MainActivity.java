package com.pixelzerg.animestr;

import android.app.Activity;
import android.app.SearchManager;
import android.content.Context;
import android.os.Bundle;
import android.support.v4.view.MenuItemCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ExpandableListView;
import android.widget.SearchView;
import android.widget.TextView;

import java.util.ArrayList;
import java.lang.*;
import java.util.List;

import AnimeScraper.Core.AnimeData;
import AnimeScraper.Scrapers.AnimeDaoScraper;

public class MainActivity extends AppCompatActivity
        implements SearchView.OnQueryTextListener, SearchView.OnCloseListener{

    public static Activity activity;

    public static SearchManager searchManager;
    public static android.widget.SearchView searchView;
    public static MyExpandableListAdapter listAdapter;
    public static ExpandableListView myList;
    public static ArrayList<ParentRow> parentList = new ArrayList<ParentRow>();
    public static ArrayList<ParentRow> showTheseParentList = new ArrayList<ParentRow>();
    public static MenuItem searchItem;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        listAdapter = new MyExpandableListAdapter(MainActivity.this,parentList);
        activity = this;
        AnimeScraper.Core.Main.LoadScrapers();
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

//        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
//        fab.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
//                        .setAction("Action", null).show();
//            }
//        });

        searchManager = (SearchManager)getSystemService(Context.SEARCH_SERVICE);

        parentList = new ArrayList<ParentRow>();
        showTheseParentList = new ArrayList<ParentRow>();

//        this.LoadSearch("aka");


    }

    public List<AnimeData> tempAnimeDataList;
    private void loadData(){
//        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
//
//        StrictMode.setThreadPolicy(policy);


//        ArrayList<AnimeData> AnimeDatas = new ArrayList<AnimeData>();
//        ParentRow parentRow = null;
//
//        AnimeDatas.add(new AnimeData("TEST LALAL WOOP WOP",R.mipmap.ic_launcher));
//        AnimeDatas.add(new AnimeData("lorem ipsum dolor sit amet, consectetur apidiscing elit.", R.mipmap.ic_launcher));
//        AnimeDatas.add(new AnimeData("No, I am your father", R.mipmap.ic_launcher));
//        parentRow = new ParentRow("First Group", AnimeDatas);
//        parentList.add(parentRow);
//
//        AnimeDatas = new ArrayList<AnimeData>();
//        AnimeDatas.add(new AnimeData("Time to dance like a good banana",R.mipmap.icon_akame));
//        AnimeDatas.add(new AnimeData("oops wrong hole again, sorry Dad!",R.mipmap.ic_launcher));
//        parentRow = new ParentRow("Second Group",AnimeDatas);
//        parentList.add(parentRow);
//        new Thread(new Runnable() {
//            @Override
//            public void run() {
//                tempAnimeDataList = new AnimeDaoScraper().Search("you");
//                System.out.println("fin downloading data!");
//                parentList.add(new ParentRow("AnimeDao", tempAnimeDataList));
//                MainActivity.this.runOnUiThread(new Runnable() {
//                    @Override
//                    public void run() {
//
//                    }
//                });
//
//            }
//        }).start();

    }

    public static void expandAll(){
        for(int i = 0; i < listAdapter.getGroupCount();i++){
            myList.expandGroup(i);
        }
    }

    public void LoadSearch(final String query){
//        System.out.println("displayList();");
//        new Thread(new Runnable() {
//            @Override
//            public void run() {
//                System.out.println("Loading Data...");
                //loadData();
//                System.out.println("fin loading!!!");
//            }
//        }).start();

        //loadData();
        ((TextView)findViewById(R.id.textView)).setText("Searching...");
        new Thread(new Runnable() {
            @Override
            public void run() {
                tempAnimeDataList = new AnimeDaoScraper().Search(query,true);
                System.out.println("fin downloading data!");
                parentList.add(new ParentRow("AnimeDao", tempAnimeDataList));
                MainActivity.this.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        myList = (ExpandableListView)findViewById(R.id.expandableListView_search);
                        listAdapter = new MyExpandableListAdapter(MainActivity.this,parentList);

                        ((TextView)findViewById(R.id.textView)).setText("");
                        myList.setAdapter(listAdapter);
                        expandAll();

                    }
                });

            }
        }).start();

//        myList = (ExpandableListView)findViewById(R.id.expandableListView_search);
//        listAdapter = new MyExpandableListAdapter(MainActivity.this,parentList);
//
//        myList.setAdapter(listAdapter);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);

        searchItem = menu.findItem(R.id.action_search);
        searchView = (SearchView) MenuItemCompat.getActionView(searchItem);
        searchView.setSearchableInfo(searchManager.getSearchableInfo(getComponentName()));
        searchView.setIconifiedByDefault(false);
        searchView.setOnQueryTextListener(this);
        searchView.setOnCloseListener(this);
        searchView.requestFocus();
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public boolean onClose() {
        listAdapter.filterData("");
        expandAll();
        return false;
    }

    @Override
    public boolean onQueryTextSubmit(String query) {
        listAdapter.filterData(query);
        expandAll();
        return false;
    }

    @Override
    public boolean onQueryTextChange(String newText) {
        if(listAdapter == null) {
            System.out.println("listAdapter=" + listAdapter);
        }
        listAdapter.filterData(newText);
        expandAll();
        return false;
    }
}
