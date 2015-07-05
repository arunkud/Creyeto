package com.heygo.arunkumar.gcm;

import android.app.IntentService;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.SystemClock;
import android.support.v4.app.NotificationCompat;
import android.util.Log;
import android.widget.Toast;

import com.example.arunkumar.test10.R;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.heygo.arunkumar.activity.PrivateOneToOne;
import com.heygo.arunkumar.common.Shared;
import com.heygo.arunkumar.dao.DataContacts;
import com.heygo.arunkumar.dao.DataConversations;
import com.heygo.arunkumar.data.Contact;
import com.heygo.arunkumar.data.Conversation;

import java.util.Date;

/**
 * Created by Arunkumar on 5/17/2015.
 */
public class GcmIntentService extends IntentService {
    public static final int NOTIFICATION_ID = 1;
    private NotificationManager mNotificationManager;
    NotificationCompat.Builder builder;

    public GcmIntentService() {
        super("GcmIntentService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        try {
            Bundle extras = intent.getExtras();
            GoogleCloudMessaging gcm = GoogleCloudMessaging.getInstance(this);
            // The getMessageType() intent parameter must be the intent you received
            // in your BroadcastReceiver.
            String messageType = gcm.getMessageType(intent);

            if (!extras.isEmpty()) {  // has effect of unparcelling Bundle
            /*
             * Filter messages based on message type. Since it is likely that GCM
             * will be extended in the future with new message types, just ignore
             * any message types you're not interested in, or that you don't
             * recognize.
             */
                if (GoogleCloudMessaging.MESSAGE_TYPE_SEND_ERROR.equals(messageType)) {
                    sendNotification("Send error: " + extras.toString());
                } else if (GoogleCloudMessaging.MESSAGE_TYPE_DELETED.equals(messageType)) {
                    sendNotification("Deleted messages on server: " + extras.toString());
                    // If it's a regular GCM message, do some work.
                } else if (GoogleCloudMessaging.
                        MESSAGE_TYPE_MESSAGE.equals(messageType)) {

                    Conversation conversation = new Conversation();
                    conversation.Message = extras.getString("message");
                    DataContacts dtContact = new DataContacts(this);
                    Contact contact = dtContact.GetContact(extras.getString("title"));
                    conversation.OwnerID = contact.ID;
                    conversation.Created = new Date();
                    DataConversations dtConversation = new DataConversations(this);
                    dtConversation.CreateConversation(conversation);

                    Shared.logInformation("Completed work @ " + SystemClock.elapsedRealtime());
                    // Post notification of received message.
                    sendNotification("Received: " + extras.toString());
                    Shared.logInformation("Received: " + extras.toString());
                }
            }
            // Release the wake lock provided by the WakefulBroadcastReceiver.
            GcmBroadcastReceiver.completeWakefulIntent(intent);
        }
        catch (Exception ex){
            Shared.logError("Failed processing : " + ex.getMessage());
        }
    }


    // Put the message into a notification and post it.
    // This is just one simple example of what you might choose to do with
    // a GCM message.
    private void sendNotification(String msg) {
        mNotificationManager = (NotificationManager)this.getSystemService(Context.NOTIFICATION_SERVICE);

        PendingIntent contentIntent = PendingIntent.getActivity(this, 0, new Intent(this, PrivateOneToOne.class), 0);

        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(this)
                                            .setSmallIcon(R.drawable.ic_stat_gcm)
                                            .setContentTitle("GCM Notification")
                                            .setStyle(new NotificationCompat.BigTextStyle()
                                                    .bigText(msg))
                                            .setContentText(msg);

        mBuilder.setContentIntent(contentIntent);
        mNotificationManager.notify(NOTIFICATION_ID, mBuilder.build());
    }
}
