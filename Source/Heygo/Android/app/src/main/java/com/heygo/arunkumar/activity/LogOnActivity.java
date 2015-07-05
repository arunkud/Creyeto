package com.heygo.arunkumar.activity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;
import com.example.arunkumar.test10.R;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.common.Utility;
import com.heygo.arunkumar.common.ServerContactHandler;
import com.heygo.arunkumar.data.Contact;
import com.heygo.arunkumar.gcm.MessageSender;
import java.io.IOException;
import java.util.ArrayList;
import java.util.StringTokenizer;
import java.util.concurrent.atomic.AtomicInteger;

/**
 * Created by Arunkumar on 5/16/2015.
 */
public class LogOnActivity extends Activity {
    private final static int PLAY_SERVICES_RESOLUTION_REQUEST = 9000;

    TextView mDisplay;
    GoogleCloudMessaging gcm;
    AtomicInteger msgId = new AtomicInteger();
    Context _context;
    MessageSender _messageSender;
    EditText editToUser;
    EditText editPhoneNumber;
    Button btnSignIn;
    ProgressBar _progressBar;
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.register);

        _context = getApplicationContext();
        String regid = getRegistrationId(_context);
        if(!regid.isEmpty()){
            Intent intent = new Intent(_context, ChatHomeActivity.class);
            startActivity(intent);
        }

        _progressBar = (ProgressBar)findViewById(R.id.progressBar);
        _progressBar.setVisibility(View.INVISIBLE);
        _messageSender = new MessageSender(_context);

        editToUser = (EditText)findViewById(R.id.editUsername);
        editPhoneNumber = (EditText)findViewById(R.id.editPhoneNumber);

        btnSignIn = (Button)findViewById(R.id.btnSignIn);
        btnSignIn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if(editToUser.getText().toString().isEmpty()) {
                    Toast.makeText(v.getContext(), "Please enter a valid user name", Toast.LENGTH_SHORT).show();
                    return;
                }

                if (checkPlayServices()) {
                    gcm = GoogleCloudMessaging.getInstance(v.getContext());
                    String regid = getRegistrationId(v.getContext());
                    if (regid.isEmpty()) {
                        registerInBackground(editToUser.getText().toString(), editPhoneNumber.getText().toString());
                    }
                } else {
                    Shared.logInformation("No valid Google Play Services APK found.");
                }
            }
        });
    }

    private boolean checkPlayServices() {
        int resultCode = GooglePlayServicesUtil.isGooglePlayServicesAvailable(this);
        if (resultCode != ConnectionResult.SUCCESS) {
            if (GooglePlayServicesUtil.isUserRecoverableError(resultCode)) {
                GooglePlayServicesUtil.getErrorDialog(resultCode, this, PLAY_SERVICES_RESOLUTION_REQUEST).show();
            } else {
                Shared.logInformation("This device is not supported.");
                finish();
            }
            return false;
        }
        return true;
    }

    private String getRegistrationId(Context context) {
        String registrationId = Shared.getConfiguration(context, Shared.CONST_GOOGLE_PROPERTY_REG_ID);
        if (registrationId == null || registrationId.isEmpty()) {
            Shared.logInformation("Registration not found.");
            return "";
        }

        String registeredVersion = Shared.getConfiguration(context, Shared.CONST_APP_VERSION_ID);
        String currentVersion = String.valueOf(Shared.getAppVersion(context));
        if (!registeredVersion.equals(currentVersion)) {
            Shared.logInformation("App version changed.");
            return "";
        }
        return registrationId;
    }

    private boolean storeRegistrationId(Context context, String name, String phoneNumber, String key) throws IOException {
        String appVersion = Shared.getAppVersion(context);
        Shared.logInformation("Saving regId " + key + " on app version " + key);
        Shared.setConfiguration(context, Shared.CONST_GOOGLE_PROPERTY_REG_ID, key);
        Shared.setConfiguration(context, Shared.CONST_APP_VERSION_ID, appVersion);
        Shared.setConfiguration(context, Shared.CONST_PHONE_NUMBER, phoneNumber);
        return saveContactToServer(name, phoneNumber, key);
    }

    private boolean saveContactToServer(String name, String phoneNumber, String value) throws IOException {
        Contact contact = ServerContactHandler.getContact(phoneNumber);
        if(contact != null && contact.RegisterKey.equals(value)) {
            Shared.logInformation("Device is already registered with GCM");
            return false;
        }
        contact = new Contact();
        contact.Name = name;
        contact.PhoneNumber = phoneNumber;
        contact.RegisterKey = value;
        contact.IsActive = true;
        return ServerContactHandler.saveContact(contact.Name, contact.PhoneNumber, contact.RegisterKey);
    }

    private boolean registerInBackground(String userName, String phoneNumber) {
        final Boolean[] bretValue = {false};
        AsyncTask<String, String, Boolean> async = new AsyncTask<String, String, Boolean>() {
            @Override
            protected void onProgressUpdate(String... values) {
                super.onProgressUpdate(values);
            }

            @Override
            protected void onPreExecute() {
                _progressBar.setVisibility(View.VISIBLE);
            }

            @Override
            protected Boolean doInBackground(String... params) {
                StringTokenizer tokens = new StringTokenizer(params[0], "$");
                String userName = tokens.nextToken();// this will contain "Fruit"
                String phoneNumber = tokens.nextToken();// this will contain " they taste good"
                if(userName == null || userName.isEmpty()){
                    Shared.logError("Invalid user name/ context");
                    return false;
                }

                try {
                    if (gcm == null) {
                        gcm = GoogleCloudMessaging.getInstance(_context);
                    }

                    if(gcm == null)
                    {
                        Shared.logError("Failed to initialize google cloud messaging");
                        return false;
                    }

                    String regid = gcm.register(Shared.CONST_GOOGLE_PROJECT_ID);
                    if(regid.isEmpty())
                    {
                        Shared.logError("Failed to register the device to cloud");
                        return false;
                    }

                    Shared.logInformation("Device registered, registration ID=" + regid);

                    // For this demo: we don't need to send it because the device
                    // will send upstream messages to a server that echo back the
                    // message using the 'from' address in the message.
                    // Persist the registration ID - no need to register again.
                    storeRegistrationId(_context, userName, phoneNumber, regid);
                } catch (IOException ex) {
                    Shared.logError("Error :" + ex.getMessage());
                }
                return true;
            }

            @Override
            protected void onPostExecute(Boolean result) {
                super.onPostExecute(result);
                bretValue[0] = result;
                _progressBar.setVisibility(View.INVISIBLE);
                if(bretValue[0]){
                    Intent intent = new Intent(_context, ChatHomeActivity.class);
                    startActivity(intent);
                }
                else{
                    Toast.makeText(_context, "Failed to register to GCM", Toast.LENGTH_SHORT).show();
                }
            }
        }.execute(userName + "$" + phoneNumber, null, null);

        return bretValue[0];
    }
}
