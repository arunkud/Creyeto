package com.heygo.arunkumar.activity;

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.database.Cursor;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.example.arunkumar.test10.R;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.heygo.arunkumar.activity.fragment.ContactCursorAdapter;
import com.heygo.arunkumar.activity.fragment.ConvCursorAdapter;
import com.heygo.arunkumar.common.ServerContactHandler;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.dao.DataContacts;
import com.heygo.arunkumar.dao.DataConversations;
import com.heygo.arunkumar.data.Contact;
import com.heygo.arunkumar.data.Conversation;
import com.heygo.arunkumar.gcm.GcmIntentService;
import com.heygo.arunkumar.gcm.MessageSender;

import java.util.Date;

/**
 * Created by Arunkumar on 5/26/2015.
 */
public class PrivateOneToOne extends Activity {
    private String _registerationId;
    private String _phoneNumber;
    private String _fullName;
    private int _contact_id;
    ListView listViewConv;
    Button btnSend;
    GoogleCloudMessaging gcm;
    EditText editConversation;
    //ProgressBar progressBarSendConv;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.private_one_to_one);
        //progressBarSendConv = (ProgressBar)findViewById(R.id.progressBarSendConv);
        //progressBarSendConv.setVisibility(View.INVISIBLE);
        Intent intent = getIntent();
        _contact_id = Integer.parseInt(intent.getStringExtra("ContactId"));
        _fullName = intent.getStringExtra("Name");
        _phoneNumber = intent.getStringExtra("Phone");
        _registerationId = intent.getStringExtra(Shared.CONST_GOOGLE_PROPERTY_REG_ID);
        if(_fullName == null || _fullName.isEmpty() || _phoneNumber == null || _phoneNumber.isEmpty() || _registerationId == null || _registerationId.isEmpty()) {
            Toast.makeText(this, "Unable to log the profile!", Toast.LENGTH_SHORT).show();
            return;
        }

        listViewConv = (ListView)findViewById(R.id.listViewConv);
        PopulateConversationsList();

        editConversation = (EditText)findViewById(R.id.editConversion);
        gcm = GoogleCloudMessaging.getInstance(this);

        intent = new Intent(this, GcmIntentService.class);
        registerReceiver(broadcastReceiver, new IntentFilter("com.javapapers.android.gcm.chat.userlist"));

        btnSend = (Button)findViewById(R.id.btnSend);
        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String conv_text = editConversation.getText().toString();
                if(conv_text.isEmpty()){
                    Toast.makeText(v.getContext(), "Please enter a text to send", Toast.LENGTH_SHORT).show();
                    return;
                }

                send_background(conv_text);

            }
        });
    }

    private void send_background(String conv_text){
        new AsyncTask<String, Void, Void>() {
            @Override
            protected Void doInBackground(String... params) {
                String phone = Shared.getConfiguration(getApplicationContext(), Shared.CONST_PHONE_NUMBER);
                if(ServerContactHandler.SendMessage(phone, _registerationId, params[0])) {
                    Conversation conversation = new Conversation();
                    conversation.Message = params[0];
                    conversation.OwnerID = _contact_id;
                    conversation.Created = new Date();
                    DataConversations dtConversation = new DataConversations(getApplicationContext());
                    dtConversation.CreateConversation(conversation);
                }
                return null;
            }

            @Override
            protected void onPostExecute(Void aVoid) {
                super.onPostExecute(aVoid);
                //progressBarSendConv.setVisibility(View.VISIBLE);
                PopulateConversationsList();
                editConversation.setText("");
            }
        }.execute(conv_text, null, null);
    }

    private BroadcastReceiver broadcastReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            Shared.logDebug("onReceive: " + intent.getStringExtra("CHATMESSAGE"));
        }
    };

    private void PopulateConversationsList(){
        DataConversations dtConversation = new DataConversations(this);

        Cursor cursor = dtConversation.GetConversationsById(_contact_id);
        try {
            ConvCursorAdapter convCursorAdapter = new ConvCursorAdapter(this, cursor);
            listViewConv.setAdapter(convCursorAdapter);
            listViewConv.setTextFilterEnabled(true);
        }
        catch (Exception ex){
            Log.e("ERROR", ex.getMessage());
        }
    }
}
