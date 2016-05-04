package com.pixelzerg.animestr;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.koushikdutta.urlimageviewhelper.UrlImageViewHelper;

import AnimeScraper.Core.AnimeData;
import AnimeScraper.Scrapers.AnimeDaoScraper;

public class AnimeView extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_anime_view);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        //toolbar.setTitle("Fatfat");
        setSupportActionBar(toolbar);

//        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
//        fab.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
//                        .setAction("Action", null).show();
//            }
//        });

        LoadData((AnimeData) getIntent().getSerializableExtra("AnimeData"));


    }

    private AnimeData parsedData;
    private boolean progint = true;
    private double progindex = 2;

    public void LoadData(final AnimeData data){
        ((ProgressBar)findViewById(R.id.pbar)).setVisibility(View.VISIBLE);

        setSupportActionBar(((Toolbar) findViewById(R.id.toolbar)));
        getSupportActionBar().setTitle(data.Title);

        ((ProgressBar)findViewById(R.id.pbar)).setProgress(10);
        UrlImageViewHelper.setUrlDrawable((ImageView) findViewById(R.id.imageView), data.CoverUrl, R.mipmap.icon_placeholder);
        ((ProgressBar)findViewById(R.id.pbar)).setProgress(20);

        new Thread(new Runnable() {
            @Override
            public void run() {

//                System.out.println(data.toString());
                parsedData = new AnimeDaoScraper().ScrapeAnimeData(data.SourceUrl);
                AnimeView.this.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        ((ProgressBar)findViewById(R.id.pbar)).setProgress(40);
                        //non episodes
                        ((TextView) findViewById(R.id.altT)).setText(/*"Titles: " +*/ parsedData.Title + ", " + parsedData.Alternative);
                        ((TextView) findViewById(R.id.epsT)).setText(parsedData.EpisodeCount + " Episodes");
                        ((TextView) findViewById(R.id.descT)).setText(parsedData.Description);
                        ((TextView) findViewById(R.id.sourceT)).setText("Source: " + parsedData.SourceUrl);

                        ((ProgressBar)findViewById(R.id.pbar)).setProgress(50);

                        //episodes part
                        new Thread(new Runnable(){
                            @Override
                            public void run() {
                                while(progint){
                                    AnimeView.this.runOnUiThread(new Runnable() {
                                        @Override
                                        public void run() {
                                            ProgressBar pbar = ((ProgressBar) findViewById(R.id.pbar));
                                            pbar.setProgress((int)(pbar.getProgress()+(Math.abs(pbar.getMax()-pbar.getProgress())/progindex)));
                                        }
                                    });
                                    progindex+=0.01;
                                    try {
                                        Thread.sleep(500);
                                    } catch (Exception e) {
                                        e.printStackTrace();
                                    }
                                }
                            }
                        }).start();

                        new Thread(new Runnable() {
                            @Override
                            public void run() {
                                new AnimeDaoScraper().ParseEpisodes(parsedData);
                                AnimeView.this.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        ((ProgressBar)findViewById(R.id.pbar)).setProgress(60);

                                        //episodes
//                                        ArrayList<String> listItems = new ArrayList<String>();
//                                        ArrayAdapter<String> adapter = new ArrayAdapter<String>(AnimeView.this,android.R.layout.simple_list_item_1, listItems);
//                                        ((ListView)findViewById(R.id.episodeList)).setAdapter(adapter);

                                        for (int i = 0; i < parsedData.Episodes.size(); i++) {
                                            Button button = new Button(AnimeView.this);

                                            button.setText(parsedData.Episodes.get(i).Title);

//                                            LayoutParams lp = button.getLayoutParams();
//                                            lp.width = LayoutParams.MATCH_PARENT;
//                                            lp.height = LayoutParams.WRAP_CONTENT;
//                                            button.setLayoutParams(lp);

                                            ((LinearLayout)findViewById(R.id.listP)).addView(button);
//                                            RelativeLayout.LayoutParams rp = (RelativeLayout.LayoutParams)button.getLayoutParams();
//                                            rp.addRule(RelativeLayout.ALIGN_PARENT_LEFT);
//                                            rp.addRule(RelativeLayout.ALIGN_PARENT_RIGHT);
//                                            rp.addRule(RelativeLayout.ALIGN_PARENT_START);
//                                            rp.addRule(RelativeLayout.ALIGN_PARENT_END);
//
//                                            button.setLayoutParams(rp);

                                            final int ipassed = i;

                                            button.setOnClickListener(new View.OnClickListener() {
                                                @Override
                                                public void onClick(View v) {
                                                    Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(parsedData.Episodes.get(ipassed).VideoUrl));
                                                    intent.setDataAndType(Uri.parse(parsedData.Episodes.get(ipassed).VideoUrl),"video/*");
                                                    startActivity(intent);
                                                }
                                            });


                                            System.out.println("Loaded episode " + parsedData.Episodes.get(i).EpisodeNo);
                                        }
                                        progint = false;
                                        ((ProgressBar)findViewById(R.id.pbar)).setProgress(((ProgressBar)findViewById(R.id.pbar)).getMax());
                                       ((ProgressBar)findViewById(R.id.pbar)).setVisibility(View.INVISIBLE);


                                        System.out.println("fin loading episode data!");
                                    }
                                });
                            }
                        }).start();
                    }
                });
//                System.out.println(parsedData.toString());

                //                System.out.println(parsedData.toString());

                System.out.println("fin downloading episode data!");


            }
        }).start();





//        final ListView listview = (ListView) findViewById(R.id.listview);
//        String[] values = new String[] { "Android", "iPhone", "WindowsMobile",
//                "Blackberry", "WebOS", "Ubuntu", "Windows7", "Max OS X",
//                "Linux", "OS/2", "Ubuntu", "Windows7", "Max OS X", "Linux",
//                "OS/2", "Ubuntu", "Windows7", "Max OS X", "Linux", "OS/2",
//                "Android", "iPhone", "WindowsMobile" };
//
//        final ArrayList<String> list = new ArrayList<String>();
//        for (int i = 0; i < values.length; ++i) {
//            list.add(values[i]);
//        }
//        final StableArrayAdapter adapter = new StableArrayAdapter(this,
//                android.R.layout.simple_list_item_1, list);
//        listview.setAdapter(adapter);
//
//        listview.setOnItemClickListener(new AdapterView.OnItemClickListener() {
//
//            @Override
//            public void onItemClick(AdapterView<?> parent, final View view,
//                                    int position, long id) {
//                final String item = (String) parent.getItemAtPosition(position);
//                view.animate().setDuration(2000).alpha(0)
//                        .withEndAction(new Runnable() {
//                            @Override
//                            public void run() {
//                                list.remove(item);
//                                adapter.notifyDataSetChanged();
//                                view.setAlpha(1);
//                            }
//                        });
//            }
//
//        });
    }
}
