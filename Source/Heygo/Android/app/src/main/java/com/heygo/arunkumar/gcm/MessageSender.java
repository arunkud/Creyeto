package com.heygo.arunkumar.gcm;

import android.content.Context;
import android.os.AsyncTask;
import android.os.Bundle;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.heygo.arunkumar.common.Shared;
import java.io.IOException;
import java.util.concurrent.atomic.AtomicInteger;

/**
 * Created by Arunkumar on 5/29/2015.
 */
public class MessageSender {
    AsyncTask<Context, Void, String> sendTask;
    AtomicInteger ccsMsgId = new AtomicInteger();
    Context _context = null;

    public MessageSender(Context context){
        _context = context;
    }

    public void sendMessage(final Bundle data, final GoogleCloudMessaging gcm ) {
        sendTask = new AsyncTask<Context, Void, String>() {
            @Override
            protected String doInBackground(Context... params) {
                String id = Integer.toString(ccsMsgId.incrementAndGet());
                try {
                    String projectId = Shared.CONST_GOOGLE_PROJECT_ID;
                    gcm.send(projectId + "@gcm.googleapis.com", id, data);
                    Shared.logDebug("After gcm.send successful.");
                } catch (IOException e) {
                    Shared.logError("Exception: " + e);
                    e.printStackTrace();
                }
                return "Message ID: "+id+ " Sent.";
            }

            @Override
            protected void onPostExecute(String result) {
                sendTask = null;
                Shared.logInformation("onPostExecute: result: " + result);
            }
        };
        sendTask.execute(_context, null, null);
    }
}
