package com.heygo.arunkumar.activity.ViewHandles;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.example.arunkumar.test10.R;
import com.heygo.arunkumar.data.Contact;

import java.util.ArrayList;

/**
 * Created by Arunkumar on 6/1/2015.
 */
public class UsersAdapter extends ArrayAdapter<Contact> {
    public UsersAdapter(Context context, ArrayList<Contact> users) {
        super(context, 0, users);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        // Get the data item for this position
        Contact user = getItem(position);
        // Check if an existing view is being reused, otherwise inflate the view
        if (convertView == null) {
            convertView = LayoutInflater.from(getContext()).inflate(R.layout.gcmlistaccount_item_layout, parent, false);
        }
        // Lookup view for data population
        TextView tvName = (TextView) convertView.findViewById(R.id.txtContactName);
        TextView tvHome = (TextView) convertView.findViewById(R.id.txtRegCode);
        // Populate the data into the template view using the data object
        tvName.setText(user.Name);
        tvHome.setText(user.RegisterKey);
        // Return the completed view to render on screen
        return convertView;
    }
}

