package com.heygo.arunkumar.activity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.TextView;

import com.example.arunkumar.test10.R;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.heygo.arunkumar.activity.ViewHandles.UsersAdapter;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.data.Contact;
import com.heygo.arunkumar.gcm.MessageSender;

import java.util.ArrayList;

/**
 * Created by Arunkumar on 6/1/2015.
 */
public class GCMAccountsActivity extends Activity {
    private ListView lstView = null;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.gcmlistaccount_layout);
        // Construct the data source
        Intent intent = getIntent();
        ArrayList<Contact> arrayOfUsers = (ArrayList<Contact>)intent.getSerializableExtra("contacts");
        // Create the adapter to convert the array to views
        UsersAdapter adapter = new UsersAdapter(this, arrayOfUsers);
        // Attach the adapter to a ListView
        lstView = (ListView) findViewById(R.id.listViewaccounts);
        lstView.setClickable(true);
        lstView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Shared.logInformation("Clicked listitem @ " + position);
                sendMessageToContact(view);
            }
        });
        lstView.setAdapter(adapter);
    }

    private void sendMessageToContact(View view) {
        GoogleCloudMessaging gcm;
        gcm = GoogleCloudMessaging.getInstance(view.getContext());
        MessageSender messageSender = new MessageSender(view.getContext());
        Bundle dataBundle = new Bundle();
        dataBundle.putString("ACTION", "CHAT");
        TextView txtContactName =(TextView) view.findViewById(R.id.txtContactName);
        TextView txtRegCode =(TextView) view.findViewById(R.id.txtRegCode);
        dataBundle.putString("TOUSER", txtRegCode.getText().toString());
        dataBundle.putString("CHATMESSAGE", "This is arunkumar from App");
        messageSender.sendMessage(dataBundle,gcm);
    }
}
