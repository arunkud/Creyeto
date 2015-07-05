package com.heygo.arunkumar.activity;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Toast;

import com.example.arunkumar.test10.R;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.dao.DataContacts;
import com.heygo.arunkumar.common.ServerContactHandler;
import com.heygo.arunkumar.data.Contact;

public class FindContacts extends Activity {
    Button btnSave;
    EditText editPhone;
    DataContacts dtContact;
    Button btnSearch;
    String registrationId;
    Contact _contact;
    ProgressBar progressSearchContact;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.add_contact);

        btnSave = (Button)findViewById(R.id.button);
        btnSave.setEnabled(false);
        editPhone = (EditText)findViewById(R.id.editPhone);
        btnSearch = (Button)findViewById(R.id.btnSearch);
        progressSearchContact = (ProgressBar)findViewById(R.id.progressBarSearchContact);
        progressSearchContact.setVisibility(View.INVISIBLE);
        btnSearch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                AsyncTask<String, Void, Contact> asyncTask = new AsyncTask<String, Void, Contact>() {
                    @Override
                    protected Contact doInBackground(String... params) {
                        return ServerContactHandler.getContact(editPhone.getText().toString());
                    }

                    @Override
                    protected void onPostExecute(Contact contact) {
                        if(contact == null){
                            Toast.makeText(getApplicationContext(), "Failed to retrive the contact", Toast.LENGTH_SHORT).show();
                            return;
                        }
                        _contact = contact;
                        btnSave.setEnabled(true);
                    }
                };
                asyncTask.execute(editPhone.getText().toString(), null, null);
            }
        });
        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(_contact == null) {
                    Toast.makeText(v.getContext(), "Please select a contact", Toast.LENGTH_SHORT).show();
                    return;
                }

                dtContact = new DataContacts(v.getContext());
                dtContact.CreateContact(_contact);
                Intent intent = new Intent(v.getContext(), ChatHomeActivity.class);
                startActivity(intent);
            }
        });
    }
}
