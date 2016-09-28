package com.pixelzerg.animestr;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.ExpandableListView;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.lang.*;
import java.util.List;

import AnimeScraper.Core.AnimeData;
import AnimeScraper.Scrapers.AnimeDaoScraper;

//import com.koushikdutta.urlimageviewhelper.UrlImageViewHelper;

import com.koushikdutta.urlimageviewhelper.UrlImageViewHelper;

/**
 * Created by PixelZerg on 16/04/2016.
 */
public class MyExpandableListAdapter extends BaseExpandableListAdapter {

    private Context context;
    private ArrayList<ParentRow> parentRowList;
    private ArrayList<ParentRow> originalList;

    private int filtcount = 0;

    public MyExpandableListAdapter(Context context, ArrayList<ParentRow> originalList) {
        this.context = context;
        this.parentRowList = new ArrayList<>();
        this.parentRowList.addAll(originalList);
        this.originalList=new ArrayList<>();
        //this.originalList = originalList;
        this.originalList.addAll(originalList);
    }

    @Override
    public int getGroupCount() {
        return parentRowList.size();
    }

    @Override
    public int getChildrenCount(int groupPosition) {
        return parentRowList.get(groupPosition).getChildlist().size();
    }

    @Override
    public Object getGroup(int groupPosition) {
        return parentRowList.get(groupPosition);
    }

    @Override
    public Object getChild(int groupPosition, int childPosition) {
        return parentRowList.get(groupPosition).getChildlist().get(childPosition);
    }

    @Override
    public long getGroupId(int groupPosition) {
        return groupPosition;
    }

    @Override
    public long getChildId(int groupPosition, int childPosition) {
        return childPosition;
    }

    @Override
    public boolean hasStableIds() {
        return true;
    }

    @Override
    public View getGroupView(int groupPosition, boolean isExpanded, View convertView, ViewGroup parent) {
        ParentRow parentRow = (ParentRow)getGroup(groupPosition);

        if(convertView==null){
            LayoutInflater layoutInflater = (LayoutInflater)
                    context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView=layoutInflater.inflate(R.layout.parent_row,null);
        }
        TextView heading = (TextView)convertView.findViewById(R.id.parent_text);

        heading.setText(parentRow.getName().trim());
        return convertView;
    }

    @Override
    public View getChildView(int groupPosition, int childPosition, boolean isLastChild, View convertView, ViewGroup parent) {
        final AnimeData childRow = (AnimeData)getChild(groupPosition, childPosition);
        if(convertView==null){
            LayoutInflater layoutInflater = (LayoutInflater)
                    context.getSystemService(context.LAYOUT_INFLATER_SERVICE);
            convertView = layoutInflater.inflate(R.layout.child_row,null);
        }

        //ImageView childIcon = (ImageView)convertView.findViewById(R.id.child_icon);
        //childIcon.setImageResource(childRow.getIcon()); //all

        UrlImageViewHelper.setUrlDrawable((ImageView) convertView.findViewById(R.id.child_icon), childRow.CoverUrl, R.mipmap.icon_placeholder);

        final TextView childText = (TextView)convertView.findViewById(R.id.child_text);
        childText.setText(childRow.Title.trim());

        final View finalConvertView = convertView;
        childText.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
//                Toast.makeText(finalConvertView.getContext(), childText.getText(), Toast.LENGTH_SHORT).show();

                Intent myIntent = new Intent(MainActivity.activity, AnimeView.class);
                myIntent.putExtra("AnimeData", childRow);
                MainActivity.activity.startActivity(myIntent);
            }
        });
        return convertView;
    }

    @Override
    public boolean isChildSelectable(int groupPosition, int childPosition) {
        return true;
    }

    public void filterData(String query){
        try {
            if (filtcount % 20 != 0)
            {
//                query = query.toLowerCase();
//                parentRowList.clear();
//
//                if (query.isEmpty()) {
//                    parentRowList.addAll(originalList);
//                } else {
//                    for (ParentRow parentRow : originalList) {
//                        List<AnimeData> childList = parentRow.getChildlist();
//                        ArrayList<AnimeData> newList = new ArrayList<AnimeData>();
//
//                        for (AnimeData childRow : childList) {
//                            if (childRow.Title.toLowerCase().contains(query) || childRow.Alternative.toLowerCase().contains(query)) {
//                                newList.add(childRow);
//                            }
//                        }
//                        if (newList.size() > 0) {
//                            ParentRow nParentRow = new ParentRow(parentRow.getName(), newList);
//                            parentRowList.add(nParentRow);
//                        }
//                    }
//                }
            } else {
                LoadSearch(query);

            }

            filtcount++;
            notifyDataSetChanged();
        }catch(Exception e){
            e.printStackTrace();
            ((TextView)MainActivity.activity.findViewById(R.id.textView)).setText("Error: "+e.toString());
        }
    }

    public void LoadSearch(final String query){
        MainActivity.parentList.clear();
        ((TextView)MainActivity.activity.findViewById(R.id.textView)).setText("Searching...");
        new Thread(new Runnable() {
            @Override
            public void run() {
                final List<AnimeData> tempAnimeDataList = new AnimeDaoScraper().Search(query,true);
                System.out.println("fin downloading data!");
                MainActivity.parentList.add(new ParentRow("AnimeDao", tempAnimeDataList));
                MainActivity.activity.runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        MainActivity.myList = (ExpandableListView) MainActivity.activity.findViewById(R.id.expandableListView_search);
                        MainActivity.listAdapter = new MyExpandableListAdapter(MainActivity.activity, MainActivity.parentList);

                        ((TextView) MainActivity.activity.findViewById(R.id.textView)).setText("");
                        MainActivity.myList.setAdapter(MainActivity.listAdapter);
                        MainActivity.expandAll();

                    }
                });

            }
        }).start();
    }
}
